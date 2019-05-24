using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.DataAccess.PublicTransport.Helpers;
using DCNC.Service.Database;
using DCNC.Service.PublicTransport.JsonData.TimeTable;
using DCNC.Service.PublicTransport.TimeTable.Helpers;
using System.Threading.Tasks;
using DCNC.Service.Database.Interfaces;

namespace DCNC.Service.PublicTransport.TimeTable
{
    public class TimeTableService
    {
        readonly TimeService _timeService;
        readonly DownloadHelper _downloadHelper;
        readonly ConvertingHelper _convertingHelper;
        readonly StopTimesService _stopTimesService;
        readonly IDocumentStoreRepository _documentStoreRepository;

        public TimeTableService(IDocumentStoreRepository dsr, TimeService timeService, ConvertingHelper convertingHelper, StopTimesService stopTimesService, DownloadHelper downloadHelper)
        {
            _documentStoreRepository = dsr;
            _timeService = timeService;
            _convertingHelper = convertingHelper;
            _stopTimesService = stopTimesService;
            _downloadHelper = downloadHelper;
        }

        public async Task SetTimeTables()
        {
            var stopTimes = await _stopTimesService.GetDataAsJObjectAsync(Urls.StopTimes, JsonType.StopTime);

            if(!stopTimes.HasValues) return;

            var convertedStopTimes = _stopTimesService.GetData<StopTimeUrl>(stopTimes);
            convertedStopTimes = _timeService.FilterStopTimesByDate(convertedStopTimes);

            var entitiesThatWerentDownloaded = await _downloadHelper.MassDownloadAndSaveToDb(convertedStopTimes);
            _convertingHelper.ChangeTimeTableJsonsToObjectsAndSaveToDb(convertedStopTimes, entitiesThatWerentDownloaded);
        }

        public MinuteTimeTable GetMinuteTimeTableByRouteIdAndStopId(int routeId, int stopId)
        {
            return _documentStoreRepository.GetMinuteTimeTableByRouteIdAndStopId(routeId, stopId);
        }
    }
}
