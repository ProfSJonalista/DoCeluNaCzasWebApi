using System.Collections.Generic;
using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.Service.PublicTransport.Caching;
using DCNC.Service.PublicTransport.Caching.Helpers;
using DoCeluNaCzasWebApi.Models.PublicTransport.General;
using DoCeluNaCzasWebApi.Services.UpdateService;
using Microsoft.AspNet.SignalR;

namespace DoCeluNaCzasWebApi.Hubs
{
    public class PublicTransportHub : Hub
    {
        private readonly CacheService _cacheService;

        public PublicTransportHub()
        {
            _cacheService = new CacheService();
        }

        public BusStopDataModel GetBusStopData()
        {
            return _cacheService.GetData<BusStopDataModel>(CacheKeys.JOINED_BUS_STOPS);
        }

        public List<TimeTableData> GetTimeTableDataByRouteId(int id)
        {
            return UpdateTimeTableService.GetTimeTableData(id);
        }
    }
}