using Mandarin.ApiModels;
using Mandarin.Client;
using Mandarin.Data;
using Mandarin.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mandarin.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
   [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
   public class MemberController : Controller
   {
      public readonly ICoinDeskClient _coinDeskClient;
      public readonly MandarinDBContext _dbContext;
      private readonly ILogger<MemberController> _logger;

      public MemberController(ICoinDeskClient coinDeskClient, MandarinDBContext dBContext, ILogger<MemberController> logger)
      {
         _coinDeskClient = coinDeskClient;
         _dbContext = dBContext;
         _logger = logger;
      }

      [HttpGet("Info")]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Member))] 
      public async Task<ActionResult<Member>> Info([FromQuery] string email)
      {
         _logger.LogInformation($"Member Info requested at {DateTime.UtcNow}, Email: {email}");

         var member = await _dbContext.Members.Where(x => x.Email == email).FirstOrDefaultAsync();
         if (member == null)
         {
            var message = $"Entered email does not exist in our database, Email : {email}";
            _logger.LogError(message);
            return NotFound(new Error { Message = message });
         }
         var usdRate = await _coinDeskClient.GetUSDdPrice();
         var result = new Member() { Email = member.Email, Balance = new Balance { Amount = member.Balance, Rate = member.Balance * usdRate } };
         return Ok(result);
      }

      [HttpPost("Claim")]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Member))] 
      public async Task<ActionResult<Member>> ClaimAsync(Claim claim)
      {
         var info = await _dbContext.Infos.FirstOrDefaultAsync();
         var member = await _dbContext.Members.Where(x => x.Email == claim.Email).FirstOrDefaultAsync();
         if (member == null)
         {
            var message = $"Entered email does not exist in our database!!!, Email: {claim.Email}";
            _logger.LogError(message);
            return NotFound(new Error { Message = message });
         }
         if ((DateTime.UtcNow - member.LastClaimTime).TotalHours < 24)
         {
            return NotFound(new Error
            {
               Message = $"Entered email already claimed at {member.LastClaimTime}, " +
               $"you have one right to claim in every 24 hours, please try later!!!"
            });
         }
         info.BitcoinAmount = info.BitcoinAmount - 0.001;
         if (info.BitcoinAmount < 0)
         {
            var message = $"Do not have enough balance for new claims, Email: {claim.Email}, Current Balance : {info.BitcoinAmount}";
            _logger.LogWarning(message);
            return Ok(new Error { Message = message });
         }
         member.Balance = member.Balance + 0.001;
         member.LastClaimTime = DateTime.UtcNow;
         _dbContext.Members.Update(member);
         _dbContext.Infos.Update(info);
         _dbContext.ClaimHistories.Add(new ClaimHistory { MemberId = member.Id, Amount = 0.001, CreatedDate = DateTime.UtcNow });
         _dbContext.SaveChanges();
         _logger.LogInformation($"Member : {claim.Email} did new claim, Latest Balance : {member.Balance}, Date : {member.LastClaimTime}");
         return Ok(new Member { Email = member.Email, Id = member.Id, Balance = new Balance { Amount = member.Balance } });
      }

      [HttpPost("Member")]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Member))] 
      public async Task<ActionResult<MemberEntity>> Member([FromBody] NewMember data)
      {
         var member = await _dbContext.Members.Where(x => x.Email == data.Email).FirstOrDefaultAsync();
         if (member != null)
         {
            var message = $"Entered email already in use, Email : {data.Email}";
            _logger.LogWarning(message);
            return BadRequest(new Error { Message = message });
         }
         _dbContext.Add(new MemberEntity { Email = data.Email });
         _dbContext.SaveChanges();
         _logger.LogInformation($"New Member added, Email : {member.Email}");
         return Ok(new Member { Email = data.Email, Id = member.Id });
      }
   }
}
