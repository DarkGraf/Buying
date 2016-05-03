using System;
using System.Data.Entity;
using System.Linq;

using Buying.Dal.Entities;
using Buying.Dal.EF;
using Buying.Dal.Interfaces;

namespace Buying.Dal.Repositories
{
    class CommentsRepository : IRepository<Comments>
    {
        BuyingContext context;

        public CommentsRepository(BuyingContext context)
        {
            this.context = context;
        }

        #region IRepository

        public void Create(Comments item)
        {
            context.Comments.Add(item);
        }

        public void Delete(Guid id)
        {
            Comments item = context.Comments.Find(id);
            if (item != null)
            {
                context.Comments.Remove(item);
            }
        }

        public Comments Get(Guid id)
        {
            return context.Comments.Find(id);
        }

        public IQueryable<Comments> GetAll()
        {
            return context.Comments;
        }

        public void Update(Comments item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        #endregion
    }
}
