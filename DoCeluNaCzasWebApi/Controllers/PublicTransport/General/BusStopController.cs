using DoCeluNaCzasWebApi.Models.PublicTransport.General;
using DoCeluNaCzasWebApi.Services.UpdateService;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers.PublicTransport.General
{
    public class BusStopController : ApiController
    {
        public BusStopDataModel Get()
        {
            return UpdateDataService.GetBusStops();
        }
    }
}
