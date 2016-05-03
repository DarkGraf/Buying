using System;
using System.Linq;

namespace Buying.Dal.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T Get(Guid id);
        void Create(T item);
        void Update(T item);
        void Delete(Guid id);
    }
}
