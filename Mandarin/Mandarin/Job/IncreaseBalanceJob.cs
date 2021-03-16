using Mandarin.Client;
using Mandarin.Data;
using Mandarin.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
      private readonly ILogger<IncreaseBalanceJob> _logger;

      public IncreaseBalanceJob(IServiceScopeFactory serviceScopeFactory, ICoinDeskClient coinDeskClient, ILogger<IncreaseBalanceJob> logger)
      {
         _dbContext = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<MandarinDBContext>();
         _coinDeskClient = coinDeskClient;
         _logger = logger;
      }

      public Task Execute(IJobExecutionContext context)
      {
         try
         {
            _logger.LogInformation($"IncreaseBalance Job started at {DateTime.UtcNow}");
            var info = _dbContext.Infos.FirstOrDefaultAsync().Result;
            var rate = _coinDeskClient.GetUSDdPrice();
            if (info == null) info = new InfoEntity();
            info.BitcoinAmount = +(500 / rate.Result);
            if (info.Id == 0)
            {
               _dbContext.Infos.Add(info);
            }
            else
            {
               _dbContext.Infos.Update(info);
            }
            _dbContext.SaveChanges();
            _logger.LogInformation($"IncreaseBalance Job ended at {DateTime.UtcNow}");
            return Task.CompletedTask;

         }
         catch (Exception e)
         {
            _logger.LogError("IncreaseBalance Job failed", e);
            throw;
         }
      }
   }
}
