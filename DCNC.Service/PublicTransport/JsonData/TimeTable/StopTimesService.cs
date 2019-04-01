using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using DCNC.Service.Database;
using DCNC.Service.PublicTransport.JsonData.Abstracts;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace DCNC.Service.PublicTransport.JsonData.TimeTable
{
    public class StopTimesService : DataAbstractService
    {
        public StopTimesService(DocumentStoreRepository documentStoreRepository) : base(documentStoreRepository) { }

        public override object Converter(JToken stopTimes)
        {
            return new StopTimeUrl()
            {
                RouteId = int.Parse(stopTimes.Path),
                Urls = stopTimes.Values().Select(url => url.ToString()).ToList()
            };
        }
    }
}