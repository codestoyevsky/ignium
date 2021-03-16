using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Xunit;

namespace Mandarin.IntegrationTests.Fixtures
{
   public abstract class IntegrationTest : IClassFixture<ApiWebApplicationFactory>
   {
      protected readonly ApiWebApplicationFactory _factory;
      protected readonly HttpClient _client;
      protected readonly IConfiguration _configuration;

      public IntegrationTest(ApiWebApplicationFactory fixture)
      {
         _factory = fixture;
         _client = _factory.CreateClient();
         _configuration = new ConfigurationBuilder()
               .AddJsonFile("integrationsettings.json")
               .Build();
      }
   }

}
