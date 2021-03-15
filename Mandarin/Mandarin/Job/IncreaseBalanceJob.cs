using Mandarin.Client;
using Mandarin.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Mandarin.Job
{
   [DisallowConcurrentExecution]
   public class IncreaseBalanceJob : IJob
   {
      private readonly MandarinDBContext _dbContext;
      private readonly ICoinDeskClient _coinDeskClient;

      public IncreaseBalanceJob(IServiceScopeFactory serviceScopeFactory, ICoinDeskClient coinDeskClient)
      {
         _dbContext = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<MandarinDBContext>();
         _coinDeskClient = coinDeskClient;
      }

      public Task Execute(IJobExecutionContext context)
      {
         _dbContext.Database.EnsureCreated();
         var balance = _dbContext.Balances.FirstOrDefaultAsync();
         var rate = _coinDeskClient.GetUSDdPrice();
         balance.Result.Amount = + (500 / rate.Result);
         _dbContext.Update(balance.Result);
         _dbContext.SaveChanges();
         return Task.CompletedTask;
      }
   }
}
