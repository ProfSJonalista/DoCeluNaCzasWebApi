using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.DataAccess.PublicTransport.Helpers;
using DCNC.Service.Database;
using DCNC.Service.PublicTransport.JsonData.TimeTable;
using DCNC.Service.PublicTransport.TimeTable.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DCNC.Service.PublicTransport.TimeTable
{
    public class TimeTableService
    {
        private readonly TimeService _timeService;
        private readonly DownloadHelper _downloadHelper;
        private readonly ConvertingHelper _convertingHelper;
        private readonly StopTimesService _stopTimesService;
        private readonly IDocumentStoreRepository _documentStoreRepository;

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

        public List<TimeTableData> GetTimeTableDataByRouteId(int routeId)
        {
            return _documentStoreRepository.GetTimeTableDataByRouteId(routeId);
        }
    }
}