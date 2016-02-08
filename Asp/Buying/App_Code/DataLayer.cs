using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace DataLayer
{
  [Table("Buying")]
  class Buying
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid Goods { get; set; }
    public int Priority { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime InputDate { get; set; }
    public Guid? Comment { get; set; }
  }

  class Comments
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Description { get; set; }
  }

  class Goods
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Name { get; set; }
  }

  class BuyingContext : DbContext
  {
    public BuyingContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

    public DbSet<Buying> Buyings { get; set; }
    public DbSet<Comments> Comments { get; set; }
    public DbSet<Goods> Goods { get; set; }
  }
}