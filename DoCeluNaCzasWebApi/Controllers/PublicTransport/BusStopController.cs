using DoCeluNaCzasWebApi.Models.PublicTransport;
using DoCeluNaCzasWebApi.Services.UpdateService;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers
{
    public class BusStopController : ApiController
    {
        public BusStopDataModel Get()
        {
            return UpdateDataService.GetBusStops();
        }
    }
}
