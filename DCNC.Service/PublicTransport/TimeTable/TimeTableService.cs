using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using DCNC.DataAccess.PublicTransport.Helpers;
using DCNC.Service.PublicTransport.JsonData.TimeTable;

namespace DCNC.Service.PublicTransport.TimeTable
{
    public class TimeTableService
    {
        private readonly StopTimesService _stopTimesService;

        public TimeTableService()
        {
            _stopTimesService = new StopTimesService();
        }

        public async Task SetTimeTables()
        {
            var stopTimes = await _stopTimesService.GetDataAsJObjectAsync(Urls.StopTimes);
            var convertedStopTimes = _stopTimesService.Converter(stopTimes);
        }
    }
}