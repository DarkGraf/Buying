using System;
using System.Data.Entity;

using Buying.Dal.Entities;

namespace Buying.Dal.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Entities.Buying> Buyings { get; }
        IRepository<Comments> Comments { get; }
        IRepository<Goods> Goods { get; }
        IRepository<Constant> Constants { get; }
        void Save();

        DbContextTransaction BeginTransaction();
    }
}
