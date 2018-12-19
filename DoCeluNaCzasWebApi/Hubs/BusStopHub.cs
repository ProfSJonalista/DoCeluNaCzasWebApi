using DCNC.Bussiness.PublicTransport;
using DCNC.Bussiness.PublicTransport.GeneralData;
using DCNC.Service.PublicTransport.Resources;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace DoCeluNaCzasWebApi.Hubs
{
    public class BusStopHub : Hub
    {
        static readonly ObjectCache _cache = MemoryCache.Default;

        public List<BusStopData> GetBusStopDataList()
        {
            return _cache[CacheKeys.BUS_STOP_DATA_LIST_KEY] as List<BusStopData>;
        }

        public BusStopData GetBusLineData()
        {
            var data = _cache[CacheKeys.GENERAL_DATA_KEY] as Data;
            return data.BusStopData;
        }
    }
}