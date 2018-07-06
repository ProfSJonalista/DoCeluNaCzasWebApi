using DCNC.Service.Bus_stops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers
{
    public class PublicTransportController : ApiController
    {
        BusStopService _busStopService = new BusStopService();

        public async Task<string> Get(int id)
        {
            switch (id)
            {
                case 1:
                    return await _busStopService.GetStopsForCurrentDay();
                case 2:
                    return "TimeTable";
                default:
                    return "Nothing";
            }
        }
    }
}
