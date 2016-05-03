using System;
using System.Data.Entity;

using Buying.Dal.Entities;
using Buying.Dal.Interfaces;
using Buying.Dal.EF;

namespace Buying.Dal.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        BuyingContext context;
        BuyingRepository buyingRepository;
        GoodsRepository goodsRepository;
        CommentsRepository commentsRepository;
        ConstantsRepository constantsRepository;

        public EFUnitOfWork(string nameOrConnectionString)
        {
            context = new BuyingContext(nameOrConnectionString);
        }

        #region IUnitOfWork

        public IRepository<Entities.Buying> Buyings
        {
            get
            {
                if (buyingRepository == null)
                {
                    buyingRepository = new BuyingRepository(context);
                }
                return buyingRepository;
            }
        }

        public IRepository<Comments> Comments
        {
            get
            {
                if (commentsRepository == null)
                {
                    commentsRepository = new CommentsRepository(context);
                }
                return commentsRepository;
            }
        }

        public IRepository<Goods> Goods
        {
            get
            {
                if (goodsRepository == null)
                {
                    goodsRepository = new GoodsRepository(context);
                }
                return goodsRepository;
            }
        }

        public IRepository<Constant> Constants
        {
            get
            {
                if (constantsRepository == null)
                {
                    constantsRepository = new ConstantsRepository(context);
                }
                return constantsRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public DbContextTransaction BeginTransaction()
        {
            return context.Database.BeginTransaction();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            context.Dispose();
        }

        #endregion
    }
}
