using DCNC.Bussiness.PublicTransport;
using DCNC.DataAccess.PublicTransport;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DCNC.Service.PublicTransport
{
    public class AgencyService
    {
        public async Task<string> GetAgencies()
        {
            return await PublicTransportRepository.GetAgencies();
        }
    }
}