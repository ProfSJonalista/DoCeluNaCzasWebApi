using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.DataAccess.Database;
using DCNC.DataAccess.PublicTransport;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers
{
    public class DownloadHelper
    {
        private readonly PublicTransportRepository _publicTransportRepository;

        public DownloadHelper()
        {
            _publicTransportRepository = new PublicTransportRepository();
        }

        public async Task MassDownloadAndSaveToDb(List<StopTime> convertedStopTimes)
        {
            foreach (var stopTime in convertedStopTimes)
            {
                using (var client = new HttpClient())
                {
                    using (var session = DocumentStoreHolder.Store.OpenSession())
                    {
                        foreach (var url in stopTime.Urls)
                        {
                            var json = await _publicTransportRepository.DownloadData(url, client);

                            if (!string.IsNullOrEmpty(json))
                            {
                                session.Store(new TimeTableJson()
                                {
                                    RouteId = stopTime.RouteId,
                                    Json = json
                                });
                            }
                        }

                        session.SaveChanges();
                    }
                }
            }
        }
    }
}