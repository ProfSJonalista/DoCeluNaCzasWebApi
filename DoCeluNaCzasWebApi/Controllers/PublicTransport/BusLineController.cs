using DCNC.Service.PublicTransport.UpdateService;
using System.Threading.Tasks;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers.PublicTransport
{
    public class BusLineController : ApiController
    {
        public string Get(bool hasData)
        {
            return UpdateDataService.GetBusLines(hasData);
        }
    }
}
