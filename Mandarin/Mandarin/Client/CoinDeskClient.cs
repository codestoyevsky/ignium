using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mandarin.Client
{
   public class CoinDeskClient : ICoinDeskClient
   {
      public readonly HttpClient _client;

      public CoinDeskClient(HttpClient client) {

         _client = client;
      }

      public async Task<double> GetUSDdPrice()
      {
         HttpResponseMessage response = await _client.GetAsync("https://api.coindesk.com/v1/bpi/currentprice.json");
         var jsonString = await response.Content.ReadAsStringAsync();
         var root = JsonConvert.DeserializeObject<Root>(jsonString);
         return root.bpi.USD.rate_float;
      }
   }

   // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
   public class Time
   {
      public string updated { get; set; }
      public DateTime updatedISO { get; set; }
      public string updateduk { get; set; }
   }

   public class USD
   {
      public string code { get; set; }
      public string symbol { get; set; }
      public string rate { get; set; }
      public string description { get; set; }
      public double rate_float { get; set; }
   }

   public class GBP
   {
      public string code { get; set; }
      public string symbol { get; set; }
      public string rate { get; set; }
      public string description { get; set; }
      public double rate_float { get; set; }
   }

   public class EUR
   {
      public string code { get; set; }
      public string symbol { get; set; }
      public string rate { get; set; }
      public string description { get; set; }
      public double rate_float { get; set; }
   }

   public class Bpi
   {
      public USD USD { get; set; }
      public GBP GBP { get; set; }
      public EUR EUR { get; set; }
   }

   public class Root
   {
      public Time time { get; set; }
      public string disclaimer { get; set; }
      public string chartName { get; set; }
      public Bpi bpi { get; set; }
   }
}
