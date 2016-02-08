using System;
using System.Linq;

using DataLayer;

namespace Service
{
  public class BuyingService : IBuyingService
  {
    #region IBuyingService

    public BuyingDataContract[] GetBuyings()
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

      using (BuyingContext context = new BuyingContext("GoodsBuyingConnectionString"))
      {
        BuyingDataContract[] result = (from buying in context.Buyings
                                       join goods in context.Goods on buying.Goods equals goods.Id
                                       join comment in context.Comments on buying.Comment equals comment.Id into leftComments
                                       from subComment in leftComments.DefaultIfEmpty()
                                       select new BuyingDataContract
                                       {
                                         Id = buying.Id,
                                         Goods = goods.Name,
                                         Priority = buying.Priority,
                                         InputDate = buying.InputDate,
                                         Comment = subComment.Description
                                       }).ToArray();
        return result;
      }
    }

    public void AddBuying(BuyingAddDataContract buyingInfo)
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

      using (BuyingContext context = new BuyingContext("GoodsBuyingConnectionString"))
      using (var transaction = context.Database.BeginTransaction())
      {
        // Если комментарий есть, добавим его сначала.
        Comments comment = null;
        if (!string.IsNullOrWhiteSpace(buyingInfo.Comment))
        {
          comment = new Comments { Description = buyingInfo.Comment };
          context.Comments.Add(comment);
          context.SaveChanges();
        }

        Buying buying = new Buying
        {
          Goods = buyingInfo.Goods,
          Priority = buyingInfo.Priority,
          Comment = comment != null ? comment.Id : (Guid?)null
        };
        context.Buyings.Add(buying);
        context.SaveChanges();
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

      using (BuyingContext context = new BuyingContext("GoodsBuyingConnectionString"))
      using (var transaction = context.Database.BeginTransaction())
      {
        // Удаление из таблицы покупок.
        Buying buying = context.Buyings.Find(id);
        context.Buyings.Remove(buying);
        context.SaveChanges();

        // Удаление из таблицы комментариев.
        var deleteComments = (from c in context.Comments
                              join b in context.Buyings on c.Id equals b.Comment into leftBuyings
                              from subBuying in leftBuyings.DefaultIfEmpty()
                              where subBuying == null
                              select c).ToArray();
        if (deleteComments.Count() > 0)
        {
          context.Comments.RemoveRange(deleteComments);
          context.SaveChanges();
        }

        transaction.Commit();
      }
    }

    public GoodsDataContract[] GetGoods()
    {
      #region Sql-Запрос.
      /*select * from Goods order by Name*/
      #endregion

      using (BuyingContext context = new BuyingContext("GoodsBuyingConnectionString"))
      {
        GoodsDataContract[] goods = (from g in context.Goods
                                     orderby g.Name
                                     select new GoodsDataContract
                                     {
                                       Id = g.Id,
                                       Name = g.Name
                                     }).ToArray();

        return goods;
      }
    }

    public GoodsDataContract[] GetNotUsedGoods()
    {
      #region Sql-запрос.
      /*select Goods.Id, Goods.Name 
      from Goods
        left join Buying on Goods.Id = Buying.Goods
      where
        Buying.Goods is null
      order by Name*/
      #endregion

      using (BuyingContext context = new BuyingContext("GoodsBuyingConnectionString"))
      {
        GoodsDataContract[] goods = (from g in context.Goods
                                     join b in context.Buyings on g.Id equals b.Goods into leftBuyings
                                     from subBuying in leftBuyings.DefaultIfEmpty()
                                     where subBuying == null
                                     orderby g.Name
                                     select new GoodsDataContract
                                     {
                                       Id = g.Id,
                                       Name = g.Name
                                     }).ToArray();

        return goods;
      }
    }

    public void AddGoods(GoodsAddDataContract goodsInfo)
    {
      #region Sql-запрос.
      /*if not exists(select 1 from Goods where Name = @Name)
      begin
        insert into Goods (Id, Name) values (newid(), @Name)
      end*/
      #endregion

      using (BuyingContext context = new BuyingContext("GoodsBuyingConnectionString"))
      using (var transaction = context.Database.BeginTransaction())
      {
        if (context.Goods.Where(v => v.Name == goodsInfo.Name).Count() == 0)
        {
          Goods goods = new Goods { Name = goodsInfo.Name };
          context.Goods.Add(goods);
          context.SaveChanges();
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

      using (BuyingContext context = new BuyingContext("GoodsBuyingConnectionString"))
      using (var transaction = context.Database.BeginTransaction())
      {
        Goods goods = context.Goods.Find(id);
        var checkBuying = context.Buyings.FirstOrDefault(v => v.Goods == id);

        if (goods != null && checkBuying == null)
        {
          context.Goods.Remove(goods);
          context.SaveChanges();
        }

        transaction.Commit();
      }
    }

    public void ChangeGoods(GoodsChangeDataContract goodsInfo)
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

      using (BuyingContext context = new BuyingContext("GoodsBuyingConnectionString"))
      using (var transaction = context.Database.BeginTransaction())
      {
        Goods goods = context.Goods.Find(goodsInfo.Id);

        if (goods != null)
        {
          goods.Name = goodsInfo.NewName;
          context.SaveChanges();
        }

        transaction.Commit();
      }
    }

    #endregion
  }
}