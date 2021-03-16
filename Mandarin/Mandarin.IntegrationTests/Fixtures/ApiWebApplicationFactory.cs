using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Mandarin.IntegrationTests.Fixtures
{
   public class ApiWebApplicationFactory : WebApplicationFactory<Startup>
   {
      protected override void ConfigureWebHost(IWebHostBuilder builder)
      {
         builder.ConfigureAppConfiguration(config =>
         {
            var integrationConfig = new ConfigurationBuilder()
              .AddJsonFile("integrationsettings.json")
              .Build();

            config.AddConfiguration(integrationConfig);
         });
      }
   }
}
