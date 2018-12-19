using DCNC.Service.PublicTransport.UpdateService;
using System.Threading.Tasks;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers.PublicTransport
{
    public class JoinedTripsController : ApiController
    {
        public string Get(bool hasData)
        {
            return UpdateDataService.GetJoinedTrips(hasData);
        }
    }
}
