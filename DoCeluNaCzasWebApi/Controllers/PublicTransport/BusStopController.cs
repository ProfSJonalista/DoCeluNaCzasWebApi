
using DCNC.Service.Public_Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers
{
    public class BusStopController : ApiController
    {
        public async Task<string> Get()
        {
            return BusStopService.GetStopsForCurrentDayAsJson();
        }
    }
}
