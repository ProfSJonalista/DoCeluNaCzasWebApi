using DCNC.Bussiness.PublicTransport.GeneralData;
using DCNC.Service.PublicTransport.Resources;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace DoCeluNaCzasWebApi.Hubs
{
    public class GeneralDataHub : Hub
    {
        static readonly ObjectCache _cache = MemoryCache.Default;

        public Data GetGeneralData()
        {
            return _cache[CacheKeys.GENERAL_DATA_KEY] as Data;
        }
    }
}