using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.DataAccess.PublicTransport;
using DCNC.Service.Database;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers
{
    public class DownloadHelper
    {
        private readonly TimeService _timeService;
        private readonly DocumentStoreRepository _documentStoreRepository;
        private readonly PublicTransportRepository _publicTransportRepository;

        public DownloadHelper(DocumentStoreRepository documentStoreRepository, TimeService timeService)
        {
            _timeService = timeService;
            _documentStoreRepository = documentStoreRepository;
            _publicTransportRepository = new PublicTransportRepository();
        }

        public async Task<List<TimeTableDateTime>> MassDownloadAndSaveToDb(List<StopTimeUrl> convertedStopTimes)
        {
            var entitiesThatWerentDownloaded = new List<TimeTableDateTime>();

            foreach (var stopTime in convertedStopTimes)
            {
                using (var client = new HttpClient())
                {
                    var objectsToSaveInDb = new List<TimeTableJson>();

                    foreach (var url in stopTime.Urls)
                    {
                        var json = await _publicTransportRepository.DownloadData(url, client);

                        if (!string.IsNullOrEmpty(json))
                        {
                            objectsToSaveInDb.Add(new TimeTableJson()
                            {
                                RouteId = stopTime.RouteId,
                                Json = json
                            });
                        }
                        else
                        {
                            entitiesThatWerentDownloaded.Add(new TimeTableDateTime()
                            {
                                RouteId = stopTime.RouteId,
                                Date = _timeService.GetDateFromUrl(url)
                            });
                        }
                    }

                    _documentStoreRepository.Save(objectsToSaveInDb);
                }
            }

            return entitiesThatWerentDownloaded;
        }
    }
}