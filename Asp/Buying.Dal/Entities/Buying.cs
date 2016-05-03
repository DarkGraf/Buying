using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Buying.Dal.Entities
{
    [Table("Buying")]
    public class Buying
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
}
