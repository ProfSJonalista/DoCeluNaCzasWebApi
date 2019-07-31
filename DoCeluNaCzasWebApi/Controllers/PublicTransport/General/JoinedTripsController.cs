using DoCeluNaCzasWebApi.Services.UpdateService;
using System.Collections.Generic;
using System.Web.Http;
using DCNC.Bussiness.PublicTransport.JoiningTrips;

namespace DoCeluNaCzasWebApi.Controllers.PublicTransport.General
{
    public class JoinedTripsController : ApiController
    {
        public List<GroupedJoinedModel> Get()
        {
            return UpdateDataService.GetJoinedTrips();
        }
    }
}
