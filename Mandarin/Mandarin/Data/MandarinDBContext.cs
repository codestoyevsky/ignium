using Mandarin.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Mandarin.Data
{

   public class MandarinDBContext : DbContext
   {
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<BalanceEntity>();
         modelBuilder.Entity<MemberEntity>();
         base.OnModelCreating(modelBuilder);
      }
      public DbSet<BalanceEntity> Balances { get; set; }

      public DbSet<MemberEntity> Members { get; set; }

      public MandarinDBContext([NotNull] DbContextOptions options) : base(options)
      {
      }
   }
}
