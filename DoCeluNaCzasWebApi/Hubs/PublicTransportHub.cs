using DCNC.Bussiness.PublicTransport.General;
using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using DoCeluNaCzasWebApi.Services.UpdateService;
using Microsoft.AspNet.SignalR;

namespace DoCeluNaCzasWebApi.Hubs
{
    public class PublicTransportHub : Hub
    {
        public MinuteTimeTable GetTimeTableDataByRouteId(int id, int stopId)
        {
            return UpdateTimeTableService.GetMinuteTimeTableByRouteIdAndStopId(id, stopId);
        }
    }
}