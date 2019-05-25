using DCNC.Bussiness.PublicTransport.TimeTable;
using DoCeluNaCzasWebApi.Services.UpdateService;
using System.Collections.Generic;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers.PublicTransport.TimeTable
{
    public class TimeTableController : ApiController
    {
        public MinuteTimeTable GetByRouteIdAndStopId(int id, int stopId)
        {
            return UpdateTimeTableService.GetMinuteTimeTableByRouteIdAndStopId(id, stopId);
        }

        public List<MinuteTimeTable> GetByBusLineName(string busLineName)
        {
            return UpdateTimeTableService.GetByBusLineName(busLineName);
        }
    }
}
