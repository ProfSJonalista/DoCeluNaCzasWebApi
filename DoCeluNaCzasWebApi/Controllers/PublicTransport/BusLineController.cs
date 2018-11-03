using DCNC.Service.PublicTransport;
using DCNC.Service.PublicTransport.UpdateService;
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
        public async Task<string> Get(bool hasData)
        {
            return await UpdateDataService.GetActualBusLines(hasData);
        }
    }
}
