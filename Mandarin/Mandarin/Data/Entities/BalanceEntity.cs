using System.ComponentModel.DataAnnotations.Schema;

namespace Mandarin.Data.Entities
{
   [Table("Balances")]
   public class BalanceEntity : BaseEntity { 
       
      public double Amount { get; set; }
   }
}
