using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using DCNC.Service.Database;
using DCNC.Service.PublicTransport.JsonData.Abstracts;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace DCNC.Service.PublicTransport.JsonData.TimeTable
{
    public class StopTimesService : DataAbstractService
    {
        public StopTimesService(DocumentStoreRepository documentStoreRepository) : base(documentStoreRepository) { }

        public override object Converter(JToken stopTimes)
        {
            var stopTimeList = new List<StopTimeUrl>();

            foreach (var item in stopTimes.Children())
            {
                stopTimeList.Add(new StopTimeUrl()
                {
                    RouteId = int.Parse(item.Path),
                    Urls = item.Values().Select(url => url.ToString()).ToList()
                });
            }

            return stopTimeList;
        }
    }
}