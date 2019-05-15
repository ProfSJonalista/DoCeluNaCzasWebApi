using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using DoCeluNaCzasWebApi.Models.PublicTransport.General;
using DoCeluNaCzasWebApi.Services.UpdateService;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;

namespace DoCeluNaCzasWebApi.Hubs
{
    public class PublicTransportHub : Hub
    {
        public BusStopDataModel GetBusStopData()
        {
            return CacheService.GetData<BusStopDataModel>(CacheKeys.BUS_STOP_DATA_MODEL);
        }

        public MinuteTimeTable GetTimeTableDataByRouteId(int id, int stopId)
        {
            return UpdateTimeTableService.GetTimeTableDataByRouteIdAndStopId(id, stopId);
        }
    }
}