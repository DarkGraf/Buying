using Buying.Dal.Entities;
using System.Data.Entity;

namespace Buying.Dal.EF
{
    class BuyingContext : DbContext
    {
        public BuyingContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        public DbSet<Entities.Buying> Buyings { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Goods> Goods { get; set; }
        public DbSet<Constant> Constants { get; set; }
    }
}
