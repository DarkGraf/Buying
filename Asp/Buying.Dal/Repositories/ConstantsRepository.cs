using System;
using System.Data.Entity;
using System.Linq;

using Buying.Dal.Entities;
using Buying.Dal.Interfaces;
using Buying.Dal.EF;

namespace Buying.Dal.Repositories
{
    class ConstantsRepository : IRepository<Constant>
    {
        BuyingContext context;

        public ConstantsRepository(BuyingContext context)
        {
            this.context = context;
        }

        #region IRepository
        
        public IQueryable<Constant> GetAll()
        {
            return context.Constants;
        }

        public Constant Get(Guid id)
        {
            return context.Constants.Find(id);
        }

        public void Create(Constant item)
        {
            context.Constants.Add(item);
        }

        public void Update(Constant item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(Guid id)
        {
            Constant item = context.Constants.Find(id);
            if (item != null)
            {
                context.Constants.Remove(item);
            }
        }

        #endregion
    }
}
