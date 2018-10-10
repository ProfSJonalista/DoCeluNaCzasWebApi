using DCNC.Service.PublicTransport.TimeTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers.PublicTransport
{
    public class JoinedTripsController : ApiController
    {
        public async Task<string> Get()
        {
            return await JoinTripService.GetStopsLinkedWithTrips();
        }
    }
}
