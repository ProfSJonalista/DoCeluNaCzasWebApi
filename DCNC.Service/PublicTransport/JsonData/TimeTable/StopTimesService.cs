using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using DCNC.Service.Database;
using DCNC.Service.PublicTransport.JsonData.Abstracts;
using Newtonsoft.Json.Linq;
using System.Linq;
using DCNC.DataAccess.PublicTransport;

namespace DCNC.Service.PublicTransport.JsonData.TimeTable
{
    public class StopTimesService : DataAbstractService
    {
        public StopTimesService(IDocumentStoreRepository documentStoreRepository, IPublicTransportRepository publicTransportRepository) : base(documentStoreRepository, publicTransportRepository) { }

        protected override object Converter(JToken stopTimes)
        {
            return new StopTimeUrl()
            {
                RouteId = int.Parse(stopTimes.Path),
                Urls = stopTimes.Values().Select(url => url.ToString()).ToList()
            };
        }
    }
}