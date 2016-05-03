using System;
using System.Data.Entity;
using System.Linq;

using Buying.Dal.Entities;
using Buying.Dal.EF;
using Buying.Dal.Interfaces;


namespace Buying.Dal.Repositories
{
    class GoodsRepository : IRepository<Goods>
    {
        BuyingContext context;

        public GoodsRepository(BuyingContext context)
        {
            this.context = context;
        }

        #region IRepository

        public void Create(Goods item)
        {
            context.Goods.Add(item);
        }

        public void Delete(Guid id)
        {
            Goods item = context.Goods.Find(id);
            if (item != null)
            {
                context.Goods.Remove(item);
            }
        }

        public Goods Get(Guid id)
        {
            return context.Goods.Find(id);
        }

        public IQueryable<Goods> GetAll()
        {
            return context.Goods;
        }

        public void Update(Goods item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        #endregion
    }
}
