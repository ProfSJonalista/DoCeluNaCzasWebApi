using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using DCNC.DataAccess.PublicTransport;
using DCNC.DataAccess.PublicTransport.Helpers;
using DCNC.Service.PublicTransport.JsonData.TimeTable;
using DCNC.Service.PublicTransport.Time;
using DCNC.Service.PublicTransport.TimeTable.Helpers;

namespace DCNC.Service.PublicTransport.TimeTable
{
    public class TimeTableService
    {
        private readonly TimeService _timeService;
        private readonly StopTimesService _stopTimesService;
        private readonly DownloadHelper _downloadHelper;

        public TimeTableService(TimeService timeService)
        {
            _timeService = timeService;
            _downloadHelper = new DownloadHelper();
            _stopTimesService = new StopTimesService();

        }

        public async Task SetTimeTables()
        {
            var stopTimes = await _stopTimesService.GetDataAsJObjectAsync(Urls.StopTimes);
            var convertedStopTimes = (List<StopTime>) _stopTimesService.Converter(stopTimes);
            convertedStopTimes = _timeService.FilterStopTimes(convertedStopTimes);

            await _downloadHelper.MassDownloadAndSaveToDb(convertedStopTimes);
            int i = 0;
        }
    }
}