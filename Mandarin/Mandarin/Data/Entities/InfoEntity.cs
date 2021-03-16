using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mandarin.Data.Entities
{
   [Table("Infos")]
   public class InfoEntity : BaseEntity
   {
      [Required]
      [StringLength(250)]
      public string CompanyName { get; set; }

      public double BitcoinAmount { get; set; }
   }
}
