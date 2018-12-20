using DCNC.Bussiness.PublicTransport;
using DCNC.Service.PublicTransport.UpdateService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers.PublicTransport
{
    public class JoinedTripsController : ApiController
    {
        public List<JoinedTripsModel> Get(bool hasData)
        {
            return UpdateDataService.GetJoinedTrips(hasData);
        }
    }
}
