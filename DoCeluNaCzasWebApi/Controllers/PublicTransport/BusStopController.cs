using DCNC.Bussiness.PublicTransport.JsonData;
using DoCeluNaCzasWebApi.Services.UpdateService;
using System.Web.Http;
using DoCeluNaCzasWebApi.Models.PublicTransport;

namespace DoCeluNaCzasWebApi.Controllers
{
    public class BusStopController : ApiController
    {
       public BusStopDataModel Get()
       {
           return UDS.GetBusStops();
       }
    }
}
