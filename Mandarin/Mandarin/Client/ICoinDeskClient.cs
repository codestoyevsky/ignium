using System.Threading.Tasks;

namespace Mandarin.Client
{
   public interface ICoinDeskClient
   {
      Task<double> GetUSDdPrice();
   }
}