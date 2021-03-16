using FluentAssertions;
using Mandarin.ApiModels;
using Mandarin.IntegrationTests.Fixtures;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Mandarin.IntegrationTests.Tests
{
   public class InfoControllerTests : IntegrationTest
   {
      public InfoControllerTests(ApiWebApplicationFactory fixture)
        : base(fixture) { }

      [Fact]
      public async Task Get_Should_Return_BitcoinRate()
      {
         var response = await _client.GetAsync("api/info/BitcoinRate");
         response.StatusCode.Should().Be(HttpStatusCode.OK);

         var rate = JsonConvert.DeserializeObject<double>(
           await response.Content.ReadAsStringAsync()
         );
         rate.Should().BeGreaterThan(0);
      }

      [Fact]
      public async Task Get_Should_Return_Balance()
      {
         var response = await _client.GetAsync("api/info/Balance");
         response.StatusCode.Should().Be(HttpStatusCode.OK);

         var balance = JsonConvert.DeserializeObject<Balance>(
           await response.Content.ReadAsStringAsync()
         );
         balance.Amount.Should().BeGreaterThan(0);
      }
   }
}
