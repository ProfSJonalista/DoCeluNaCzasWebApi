using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.DataAccess.PublicTransport;
using DCNC.Service.Database;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DCNC.DataAccess.PublicTransport.Interfaces;
using DCNC.Service.Database.Interfaces;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers
{
    public class DownloadHelper
    {
        readonly TimeService _timeService;
        readonly IDocumentStoreRepository _documentStoreRepository;
        readonly IPublicTransportRepository _publicTransportRepository;

        public DownloadHelper(IDocumentStoreRepository documentStoreRepository, TimeService timeService, IPublicTransportRepository publicTransportRepository)
        {
            _timeService = timeService;
            _documentStoreRepository = documentStoreRepository;
            _publicTransportRepository = publicTransportRepository;
        }

        public async Task<List<TimeTableDateTime>> MassDownloadAndSaveToDb(List<StopTimeUrl> convertedStopTimes)
        {
            var entitiesThatWerentDownloaded = new List<TimeTableDateTime>();

            foreach (var stopTime in convertedStopTimes)
            {
                var objectsToSaveInDb = new List<TimeTableJson>();

                using (var client = new HttpClient())
                {
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
                }

                _documentStoreRepository.Save(objectsToSaveInDb);
            }

            return entitiesThatWerentDownloaded;
        }
    }
}