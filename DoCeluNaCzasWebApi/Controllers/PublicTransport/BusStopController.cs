using DCNC.Bussiness.PublicTransport;
using DCNC.Service.PublicTransport.UpdateService;
using System.Threading.Tasks;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers
{
    public class BusStopController : ApiController
    {
        public BusStopData Get(bool hasData)
        {
            return UpdateDataService.GetBusStops(hasData);
        }
    }
}
