using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using DCNC.Service.PublicTransport.JsonData.Abstracts;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace DCNC.Service.PublicTransport.JsonData.TimeTable
{
    public class StopTimesService : DataAbstractService
    {
        public override object Converter(JToken stopTimes)
        {
            var stopTimeList = new List<StopTime>();

            foreach (var item in stopTimes.Children())
            {
                stopTimeList.Add(new StopTime()
                {
                    RouteId = int.Parse(item.Path),
                    Urls = item.Values().Select(url => url.ToString()).ToList()
                });
            }

            return stopTimeList;
        }
    }
}