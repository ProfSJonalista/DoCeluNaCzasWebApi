﻿using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
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

        public TimeTableService()
        {
            _timeService = new TimeService();
            _stopTimesService = new StopTimesService();

            var documentStoreRepository = new DocumentStoreRepository();
            _convertingHelper = new ConvertingHelper(documentStoreRepository);
            _downloadHelper = new DownloadHelper(documentStoreRepository, _timeService);
        }

        public async Task SetTimeTables()
        {
            var stopTimes = await _stopTimesService.GetDataAsJObjectAsync(Urls.StopTimes);

            if(!stopTimes.HasValues) return;

            var convertedStopTimes = (List<StopTimeUrl>) _stopTimesService.Converter(stopTimes);
            convertedStopTimes = _timeService.FilterStopTimesByDate(convertedStopTimes);

            var entitiesThatWerentDownloaded = await _downloadHelper.MassDownloadAndSaveToDb(convertedStopTimes);
            _convertingHelper.ChangeTimeTableJsonsToObjectsAndSaveToDb(convertedStopTimes, entitiesThatWerentDownloaded);
        }
    }
}