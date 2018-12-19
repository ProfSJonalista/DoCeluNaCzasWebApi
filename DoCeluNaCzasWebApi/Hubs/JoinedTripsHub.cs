using DCNC.Bussiness.PublicTransport;
using DCNC.Bussiness.PublicTransport.GeneralData;
using DCNC.Service.PublicTransport.Resources;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace DoCeluNaCzasWebApi.Hubs
{
    public class JoinedTripsHub : Hub
    {
        static readonly ObjectCache _cache = MemoryCache.Default;

        public List<JoinedTripsModel> GetJoinedTripsModelList()
        {
            var data = _cache[CacheKeys.GENERAL_DATA_KEY] as Data;
            return data.JoinedTrips;
        }
    }
}