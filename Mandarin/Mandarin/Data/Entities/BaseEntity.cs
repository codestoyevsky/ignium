using System.ComponentModel.DataAnnotations;

namespace Mandarin.Data.Entities
{
   public class BaseEntity
   {
      [Key]
      public int Id { get; set; }
   }
}