using System;
using System.Data.Entity;
using System.Linq;

using Buying.Dal.Entities;
using Buying.Dal.EF;
using Buying.Dal.Interfaces;

namespace Buying.Dal.Repositories
{
    class BuyingRepository : IRepository<Entities.Buying>
    {
        BuyingContext context;

        public BuyingRepository(BuyingContext context)
        {
            this.context = context;
        }

        #region IRepository

        public void Create(Entities.Buying item)
        {
            context.Buyings.Add(item);
        }

        public void Delete(Guid id)
        {
            Entities.Buying item = context.Buyings.Find(id);
            if (item != null)
            {
                context.Buyings.Remove(item);
            }
        }

        public Entities.Buying Get(Guid id)
        {
            return context.Buyings.Find(id);
        }

        public IQueryable<Entities.Buying> GetAll()
        {
            return context.Buyings;
        }

        public void Update(Entities.Buying item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        #endregion
    }
}
