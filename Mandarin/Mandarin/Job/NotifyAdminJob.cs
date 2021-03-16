using Mandarin.Client;
using Mandarin.Data;
using Mandarin.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Mandarin.Job
{
   [DisallowConcurrentExecution]
   public class NotifyAdminJob : IJob
   {
      private readonly MandarinDBContext _dbContext;
      private readonly IConfiguration _configuration;
      private readonly ILogger<NotifyAdminJob> _logger;

      public NotifyAdminJob(IServiceScopeFactory serviceScopeFactory, ILogger<NotifyAdminJob> logger, IConfiguration configuration)
      {
         _dbContext = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<MandarinDBContext>();
         _logger = logger;
         _configuration = configuration;
      }

      public Task Execute(IJobExecutionContext context)
      {
         try
         {
            _logger.LogInformation($"NotifyAdmin Job started at {DateTime.UtcNow}");
            var totalClaim = _dbContext.ClaimHistories.Where(x=>x.CreatedDate> DateTime.UtcNow.AddDays(-1)).Sum(x=>x.Amount);
            var smtpClient = new SmtpClient(_configuration["SMTP:Url"])
            {
               Port = Convert.ToInt32(_configuration["SMTP:Port"]),
               Credentials = new NetworkCredential(_configuration["SMTP:Email"], _configuration["SMTP:Password"]),
               EnableSsl = true,
            };

            smtpClient.Send(_configuration["SMTP:Email"], _configuration["AdminEmail"], "Report", $"Total Claimed Amount For Today is : {totalClaim}");
            return Task.CompletedTask;
         }
         catch (Exception e)
         {
            _logger.LogError("NotifyAdmin Job failed", e);
            throw;
         }
      }
   }
}
