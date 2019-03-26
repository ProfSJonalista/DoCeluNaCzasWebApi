using DoCeluNaCzasWebApi.Models.PublicTransport.General;
using DoCeluNaCzasWebApi.Services.UpdateService;
using System.Collections.Generic;
using System.Web.Http;

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
