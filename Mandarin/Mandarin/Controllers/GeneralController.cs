using Mandarin.Client;
using Mandarin.Data;
using Mandarin.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Mandarin.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class GeneralController : ControllerBase
   {
      public readonly ICoinDeskClient _coinDeskClient;

      public readonly MandarinDBContext _dbContext;

      public GeneralController(ICoinDeskClient coinDeskClient, MandarinDBContext dBContext) {
         _coinDeskClient = coinDeskClient;
         _dbContext = dBContext;
      }

      [HttpGet]
      public async Task<double> GetBitCoinRate()
      {
         var client = await _coinDeskClient.GetUSDdPrice();
         return client;
      }


      [HttpGet("Balance")]
      public async Task<double> Balance()
      {
         _dbContext.Database.EnsureCreated();
         var balance = new BalanceEntity { Amount = 0 };
         _dbContext.Add(balance);
         _dbContext.SaveChanges();
         var result = await _dbContext.Balances.FirstOrDefaultAsync();
         return result.Amount;
      }

      [HttpGet("Claim/{Email}")]
      public async Task<double> Claim([FromQuery] string email)
      {
         _dbContext.Database.EnsureCreated();
         var balance = await _dbContext.Balances.FirstOrDefaultAsync();
         balance.Amount = balance.Amount - 0.001;
         _dbContext.Update(balance);
         _dbContext.SaveChanges();
         var member = await _dbContext.Members.Where(x=>x.Email == email).FirstOrDefaultAsync();
         member.Balance = member.Balance + 0.001;
         _dbContext.Update(balance);
         _dbContext.Update(member);
         _dbContext.SaveChanges();
         return member.Balance;
      }

      [HttpGet("Member/{Email}")]
      public async Task<MemberEntity> Member(string email)
      {
         _dbContext.Database.EnsureCreated();
         var member = await _dbContext.Members.Where(x => x.Email == email).FirstOrDefaultAsync();
         return member;
      }


      [HttpPost("Member/{Email}")]
      public async Task<MemberEntity> CreateMember(string email)
      {
         _dbContext.Database.EnsureCreated();
         var member = await _dbContext.Members.Where(x => x.Email == email).FirstOrDefaultAsync();
         if (member == null) {
            _dbContext.Add(new MemberEntity { Email = email });
            _dbContext.SaveChanges();
         }
         return member;
      }
   }
}
