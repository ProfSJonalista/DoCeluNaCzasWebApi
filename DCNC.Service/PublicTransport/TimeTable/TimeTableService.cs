using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using DCNC.DataAccess.PublicTransport.Helpers;
using DCNC.Service.PublicTransport.JsonData.TimeTable;
using DCNC.Service.PublicTransport.Time;

namespace DCNC.Service.PublicTransport.TimeTable
{
    public class TimeTableService
    {
        private readonly TimeService _timeService;
        private readonly StopTimesService _stopTimesService;

        public TimeTableService(TimeService timeService)
        {
            _timeService = timeService;
            _stopTimesService = new StopTimesService();
        }

        public async Task SetTimeTables()
        {
            var stopTimes = await _stopTimesService.GetDataAsJObjectAsync(Urls.StopTimes);
            var convertedStopTimes = (List<StopTime>) _stopTimesService.Converter(stopTimes);
            convertedStopTimes = _timeService.FilterStopTimes(convertedStopTimes);
            int i = 0;
        }
    }
}