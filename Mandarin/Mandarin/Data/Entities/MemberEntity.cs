using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mandarin.Data.Entities
{
   [Table("Members")]
   public class MemberEntity : BaseEntity
   {
      [Required]
      [StringLength(250)]
      public string Email { get; set; }

      public double Balance { get; set; }

      public DateTime LastClaimTime { get; set; }
   }
}