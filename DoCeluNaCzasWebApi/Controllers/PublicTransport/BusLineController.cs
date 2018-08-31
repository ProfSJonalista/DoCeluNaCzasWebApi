using DCNC.Service.Public_Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers.PublicTransport
{
    public class BusLineController : ApiController
    {
        public string Get()
        {
            return BusLineService.GetLinesForCurrentDayAsJson();
        }
    }
}
