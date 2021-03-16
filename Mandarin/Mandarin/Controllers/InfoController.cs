using Mandarin.ApiModels;
using Mandarin.Client;
using Mandarin.Data;
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
   public class InfoController : ControllerBase
   {
      public readonly ICoinDeskClient _coinDeskClient;
      public readonly MandarinDBContext _dbContext;
      private readonly ILogger<MemberController> _logger;

      public InfoController(ICoinDeskClient coinDeskClient, MandarinDBContext dBContext, ILogger<MemberController> logger)
      {
         _coinDeskClient = coinDeskClient;
         _dbContext = dBContext;
         _logger = logger;
      }

      [HttpGet("BitcoinRate")]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(double))]
      public async Task<ActionResult<double>> BitcoinRate()
      {
         var result = await _coinDeskClient.GetUSDdPrice();
         return Ok(result);
      }

      [HttpGet("Balance")]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Balance))]
      public async Task<ActionResult<Balance>> Balance()
      {
         _logger.LogInformation($"Company Info requested at {DateTime.UtcNow}");
         var info = await _dbContext.Infos.FirstOrDefaultAsync();
         var usdRate = await _coinDeskClient.GetUSDdPrice();
         var balance = new Balance() { Amount = info.BitcoinAmount, Rate = usdRate * info.BitcoinAmount };
         return Ok(balance);
      }


      [HttpGet("TodayClaims")]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Balance))]
      public async Task<ActionResult<Balance>> TodayClaims()
      {
         _logger.LogInformation($"Company Claims for Today requested at {DateTime.UtcNow}");
         var amount =  _dbContext.ClaimHistories.Where(x => x.CreatedDate > DateTime.UtcNow.Date).Sum(x=>x.Amount);
         var usdRate = await _coinDeskClient.GetUSDdPrice();
         var balance = new Balance() { Amount = amount, Rate = usdRate * amount };
         return Ok(balance);
      }
   }
}
