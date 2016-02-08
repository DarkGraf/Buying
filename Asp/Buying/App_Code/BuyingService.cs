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
        Comments comment = null;
        if (!string.IsNullOrWhiteSpace(buyingInfo.Comment))
        {
          comment = new Comments { Description = buyingInfo.Comment };
          context.Comments.Add
        }

        Buying buying = new Buying
        {
          Goods = buyingInfo.Goods,
          Priority = buyingInfo.Priority
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

    #endregion
  }
}