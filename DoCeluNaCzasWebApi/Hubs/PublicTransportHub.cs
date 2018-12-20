using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using DCNC.Bussiness.PublicTransport;
using DCNC.Bussiness.PublicTransport.GeneralData;
using DCNC.Service.PublicTransport.Resources;
using Microsoft.AspNet.SignalR;

namespace DoCeluNaCzasWebApi.Hubs
{
    public class PublicTransportHub : Hub
    {
        static readonly ObjectCache _cache = MemoryCache.Default;

        #region JOINED TRIPS

        public List<JoinedTripsModel> GetJoinedTripsModelList()
        {
            var data = _cache[CacheKeys.GENERAL_DATA_KEY] as Data;
            return data.JoinedTrips;
        }

        #endregion

        #region BUS STOPS

        public List<BusStopData> GetBusStopDataList()
        {
            return _cache[CacheKeys.BUS_STOP_DATA_LIST_KEY] as List<BusStopData>;
        }

        public BusStopData GetBusStopData()
        {
            var data = _cache[CacheKeys.GENERAL_DATA_KEY] as Data;
            return data.BusStopData;
        }

        #endregion

        #region BUS LINES

        public List<BusLineData> GetBusLineDataList()
        {
            return _cache[CacheKeys.BUS_LINE_DATA_LIST_KEY] as List<BusLineData>;
        }

        public BusLineData GetBusLineData()
        {
            var data = _cache[CacheKeys.GENERAL_DATA_KEY] as Data;
            return data.BusLineData;
        }

        #endregion
    }
}