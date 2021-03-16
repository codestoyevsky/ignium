using FluentAssertions;
using Mandarin.IntegrationTests.Fixtures;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Mandarin.IntegrationTests.Tests
{
   public class MemberControllerTests : IntegrationTest
   {
      public MemberControllerTests(ApiWebApplicationFactory fixture)
        : base(fixture) { }

      [Fact]
      public async Task Get_Should_Return_Ok_For_Existing_user()
      {
         var response = await _client.GetAsync("api/Member/Info?Email=tallinn@ignium.io");
         response.StatusCode.Should().Be(HttpStatusCode.OK);
      }

      [Fact]
      public async Task Get_Should_Return_Ok_For_Not_Existing_user()
      {
         var response = await _client.GetAsync("api/Member/Info?Email=tallinna@ignium.io");
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }
   }
}
