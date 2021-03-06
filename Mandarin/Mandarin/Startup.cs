using Mandarin.Classes;
using Mandarin.Client;
using Mandarin.Data;
using Mandarin.Job;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Mandarin
{
   public class Startup
   {
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddDbContext<MandarinDBContext>(options => options.UseSqlite(Configuration.GetConnectionString("Default")));
         services.AddHttpClient<ICoinDeskClient, CoinDeskClient>();

         // Add Quartz services
         //services.AddSingleton<MandarinDBContext>();
         services.AddSingleton<IJobFactory, SingletonJobFactory>();
         services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
         services.AddHostedService<QuartzHostedService>();

         services.AddSingleton<IncreaseBalanceJob>();
         services.AddSingleton(new JobSchedule(
             jobType: typeof(IncreaseBalanceJob),
             cronExpression: Configuration["Quartz:IncreaseBalanceJob"]));

         services.AddSingleton<NotifyAdminJob>();
         services.AddSingleton(new JobSchedule(
             jobType: typeof(NotifyAdminJob),
             cronExpression: Configuration["Quartz:NotifyAdminJob"]));

         services.AddControllers();
         services.AddSwaggerGen();
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseHttpsRedirection();

         // Enable middleware to serve generated Swagger as a JSON endpoint.
         app.UseSwagger();

         // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
         // specifying the Swagger JSON endpoint.
         app.UseSwaggerUI(c =>
         {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
         });

         app.UseRouting();

         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();
         });

         loggerFactory.AddFile(Configuration["LogFilePath"]);

         // NOTE: this must go at the end of Configure
         var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
         using var serviceScope = serviceScopeFactory.CreateScope();
         var dbContext = serviceScope.ServiceProvider.GetService<MandarinDBContext>();
         dbContext.Database.EnsureCreated();
      }
   }
}
