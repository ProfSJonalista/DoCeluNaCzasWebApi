
using DCNC.Service.PublicTransport;
using System.Threading.Tasks;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers
{
    public class BusStopController : ApiController
    {
        public async Task<string> Get()
        {
            return await BusStopService.GetStopsForCurrentDayAsJson();
        }
    }
}
