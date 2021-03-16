using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mandarin.Data.Entities
{
   [Table("ClaimHistories")]
   public class ClaimHistory : BaseEntity
   {
      [Required]
      [ForeignKey("Member")]
      public int MemberId { get; set; }

      public double Amount { get; set; }

      public DateTime CreatedDate { get; set; }

      public MemberEntity Member{ get; set; }
   }
}