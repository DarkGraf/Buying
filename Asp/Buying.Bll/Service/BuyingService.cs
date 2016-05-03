using System;
using System.Linq;
using System.ServiceModel.Web;

using Buying.Bll.Dto;
using Buying.Dal.Entities;
using Buying.Dal.Repositories;

namespace Buying.Bll.Service
{
    public class BuyingService : IBuyingService
    {
        const string SyncDateFieldName = "SyncDate";

        private void UpdateSyncDate(EFUnitOfWork uow)
        {
            Constant constant = uow.Constants.GetAll().Where(v => v.Name == SyncDateFieldName).FirstOrDefault();
            // Если не существует, то создание, иначе обновление.
            if (constant == null)
            {
                constant = new Constant { Name = SyncDateFieldName, Value = DateTime.Now.ToString() };
                uow.Constants.Create(constant);
            }
            else
            {
                constant.Value = DateTime.Now.ToString();
                uow.Constants.Update(constant);
            }
        }

        #region IBuyingService

        public DateTime GetSyncDate()
        {
            using (EFUnitOfWork uow = new EFUnitOfWork("GoodsBuyingConnectionString"))
            {
                Constant constant = uow.Constants.GetAll().Where(v => v.Name == SyncDateFieldName).FirstOrDefault();
                return constant != null ? DateTime.Parse(constant.Value) : new DateTime(2000, 1, 1);
            }
        }

        public BuyingDto[] GetBuyings()
        {
            #region Sql-Запрос.
            /*select
              cast(Buying.Id as nchar(36)) as Id,
              Goods.Name,
              Buying.Priority,
              Buying.InputDate,
              isnull(Comments.Description, '') as Comments
            from Buying
              inner join Goods on Buying.Goods = Goods.Id
              left join Comments on Buying.Comment = Comments.Id
            order by Buying.InputDate*/
            #endregion

            using (EFUnitOfWork uow = new EFUnitOfWork("GoodsBuyingConnectionString"))
            {
                BuyingDto[] result = (from buying in uow.Buyings.GetAll()
                                      join goods in uow.Goods.GetAll() on buying.Goods equals goods.Id
                                      join comment in uow.Comments.GetAll() on buying.Comment equals comment.Id into leftComments
                                      from subComment in leftComments.DefaultIfEmpty()
                                      orderby buying.Priority descending, goods.Name
                                      select new BuyingDto
                                      {
                                          Id = buying.Id,
                                          Goods = goods.Name.TrimEnd(),
                                          Priority = buying.Priority,
                                          InputDate = buying.InputDate,
                                          Comment = subComment.Description.TrimEnd()
                                      }).ToArray();
                return result;
            }
        }

        public void AddBuying(BuyingAddDto buyingInfo)
        {
            #region Sql-Запрос.
            /*declare @CommentId uniqueidentifier
            if @Comment is not null and rtrim(@Comment) <> ''
            begin
              set @CommentId = newid()
            end

            insert into Buying (Id, Goods, Priority, InputDate, Comment)
            values (newid(), @Goods, @Priority, getdate(), @CommentId)

            if @CommentId is not null
            begin
              insert into Comments(Id, Description)
              values (@CommentId, @Comment)
            end*/
            #endregion

            using (EFUnitOfWork uow = new EFUnitOfWork("GoodsBuyingConnectionString"))
            using (var transaction = uow.BeginTransaction())
            {
                // Если комментарий есть, добавим его сначала.
                Comments comment = null;
                if (!string.IsNullOrWhiteSpace(buyingInfo.Comment))
                {
                    comment = new Comments { Description = buyingInfo.Comment };
                    uow.Comments.Create(comment);
                    uow.Save();
                }

                Dal.Entities.Buying buying = new Dal.Entities.Buying
                {
                    Goods = buyingInfo.Goods,
                    Priority = buyingInfo.Priority,
                    Comment = comment != null ? comment.Id : (Guid?)null
                };
                uow.Buyings.Create(buying);
                // Обновим дату синхронизации.
                UpdateSyncDate(uow);
                uow.Save();
                transaction.Commit();
            }
        }

        public void DeleteBuying(Guid id)
        {
            #region Sql-Запрос.
            /*delete from Buying where Id = cast(@Id as uniqueidentifier)
            delete Comments 
            from Comments
              left join Buying on Comments.Id = Buying.Comment
            where Buying.Id is null*/
            #endregion

            using (EFUnitOfWork uow = new EFUnitOfWork("GoodsBuyingConnectionString"))
            using (var transaction = uow.BeginTransaction())
            {
                // Удаление из таблицы покупок.
                uow.Buyings.Delete(id);
                // Обновим дату синхронизации.
                UpdateSyncDate(uow);
                uow.Save();

                // Удаление из таблицы комментариев.
                var deleteComments = from c in uow.Comments.GetAll()
                                     join b in uow.Buyings.GetAll() on c.Id equals b.Comment into leftBuyings
                                     from subBuying in leftBuyings.DefaultIfEmpty()
                                     where subBuying == null
                                     select c.Id;
                if (deleteComments.Count() > 0)
                {
                    foreach (var c in deleteComments)
                    {
                        uow.Comments.Delete(c);
                    }
                    uow.Save();
                }

                transaction.Commit();
            }
        }

        public GoodsDto[] GetGoods()
        {
            #region Sql-Запрос.
            /*select * from Goods order by Name*/
            #endregion

            using (EFUnitOfWork uow = new EFUnitOfWork("GoodsBuyingConnectionString"))
            {
                GoodsDto[] goods = (from g in uow.Goods.GetAll()
                                    orderby g.Name
                                    select new GoodsDto
                                    {
                                        Id = g.Id,
                                        Name = g.Name.TrimEnd()
                                    }).ToArray();

                return goods;
            }
        }

        public GoodsDto[] GetNotUsedGoods()
        {
            #region Sql-запрос.
            /*select Goods.Id, Goods.Name 
            from Goods
              left join Buying on Goods.Id = Buying.Goods
            where
              Buying.Goods is null
            order by Name*/
            #endregion

            using (EFUnitOfWork uow = new EFUnitOfWork("GoodsBuyingConnectionString"))
            {
                GoodsDto[] goods = (from g in uow.Goods.GetAll()
                                    join b in uow.Buyings.GetAll() on g.Id equals b.Goods into leftBuyings
                                    from subBuying in leftBuyings.DefaultIfEmpty()
                                    where subBuying == null
                                    orderby g.Name
                                    select new GoodsDto
                                    {
                                        Id = g.Id,
                                        Name = g.Name.TrimEnd()
                                    }).ToArray();

                return goods;
            }
        }

        public void AddGoods(GoodsAddDto goodsInfo)
        {
            #region Sql-запрос.
            /*if not exists(select 1 from Goods where Name = @Name)
            begin
              insert into Goods (Id, Name) values (newid(), @Name)
            end*/
            #endregion

            using (EFUnitOfWork uow = new EFUnitOfWork("GoodsBuyingConnectionString"))
            using (var transaction = uow.BeginTransaction())
            {
                if (uow.Goods.GetAll().Where(v => v.Name == goodsInfo.Name).Count() == 0)
                {
                    Goods goods = new Goods { Name = goodsInfo.Name };
                    uow.Goods.Create(goods);
                    uow.Save();
                }
                transaction.Commit();
            }
        }

        public void DeleteGoods(Guid id)
        {
            #region Sql-Запрос.
            /*if exists(select 1 from Goods where Id = @Id)
            and not exists(select 1 from Buying where Goods = @Id)
            begin
              delete from Goods
              where Id = @Id
            end*/
            #endregion

            using (EFUnitOfWork uow = new EFUnitOfWork("GoodsBuyingConnectionString"))
            using (var transaction = uow.BeginTransaction())
            {
                Goods goods = uow.Goods.Get(id);
                var checkBuying = uow.Buyings.GetAll().FirstOrDefault(v => v.Goods == id);

                if (goods != null && checkBuying == null)
                {
                    uow.Goods.Delete(goods.Id);
                    uow.Save();
                }

                transaction.Commit();
            }
        }

        public void ChangeGoods(GoodsChangeDto goodsInfo)
        {
            #region Sql-запрос.
            /*if exists(select 1 from Goods where Id = @Id)
            begin
              update Goods
              set
                Name = @Name
              where Id = @Id
            end*/
            #endregion

            using (EFUnitOfWork uow = new EFUnitOfWork("GoodsBuyingConnectionString"))
            using (var transaction = uow.BeginTransaction())
            {
                Goods goods = uow.Goods.Get(goodsInfo.Id);

                if (goods != null)
                {
                    goods.Name = goodsInfo.NewName;
                    uow.Save();
                }

                transaction.Commit();
            }
        }

        #endregion
    }
}