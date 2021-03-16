using Mandarin.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Mandarin.Data
{

   public class MandarinDBContext : DbContext
   {
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<InfoEntity>().HasData(new InfoEntity { Id = 1, CompanyName = "Mandarin OU", BitcoinAmount = 0.0088 });
         modelBuilder.Entity<MemberEntity>().HasData(
            new MemberEntity { Id = 1, Email = "tallinn@ignium.io", Balance = 0, LastClaimTime = DateTime.UtcNow.AddDays(-2)},
            new MemberEntity { Id = 2, Email = "riga@ignium.io", Balance = 0, LastClaimTime = DateTime.UtcNow.AddDays(-2) },
            new MemberEntity { Id = 3, Email = "vilnius@ignium.io", Balance = 0, LastClaimTime = DateTime.UtcNow.AddDays(-2) }
            );
         base.OnModelCreating(modelBuilder);
      }
      public DbSet<InfoEntity> Infos { get; set; }

      public DbSet<MemberEntity> Members { get; set; }

      public DbSet<ClaimHistory> ClaimHistories { get; set; }

      public MandarinDBContext([NotNull] DbContextOptions options) : base(options)
      {
      }
   }
}
