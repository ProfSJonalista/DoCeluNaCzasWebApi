using DCNC.Bussiness.PublicTransport;
using DCNC.DataAccess.PublicTransport;
using DCNC.Service.PublicTransport.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;

namespace DCNC.Service.PublicTransport
{
    public class BusLineService
    {
        static readonly ObjectCache _cache = MemoryCache.Default;

        public async static Task<BusLineData> GetBusLineData()
        {
            var json = await PublicTransportRepository.GetBusLines();
            JObject lines = (JObject)JsonConvert.DeserializeObject(json);
            BusLineData busLineDataToReturn;

            if (!lines.HasValues)
            {
                List<BusLineData> busLineDatas = _cache[CacheKeys.BUS_LINE_DATA_LIST_KEY] as List<BusLineData>;
                busLineDataToReturn = GetDataForCurrentDay(busLineDatas);

                return busLineDataToReturn;
            }

            busLineDataToReturn = CacheBusLineDataAndGetCurrentDayResult(lines);

            return busLineDataToReturn;
        }

        private static BusLineData CacheBusLineDataAndGetCurrentDayResult(JObject lines)
        {
            List<BusLineData> busLineDatasToCache = new List<BusLineData>();

            foreach (var item in lines.Children())
            {
                busLineDatasToCache.Add(BusLineConverter(item));
            }

            _cache.Set(CacheKeys.BUS_LINE_DATA_LIST_KEY, busLineDatasToCache, new CacheItemPolicy());

            return GetDataForCurrentDay(busLineDatasToCache);
        }

        private static BusLineData GetDataForCurrentDay(List<BusLineData> busLineDataList)
        {
            return busLineDataList.Where(x => x.Day.Date == DateTime.Now.Date).SingleOrDefault();
        }
        
        private static BusLineData BusLineConverter(JToken busLine)
        {
            BusLineData busLineData = new BusLineData()
            {
                Day = DateTime.Parse(busLine.Path),
                Routes = new List<Route>()
            };

            foreach (var item in busLine.Children())
            {
                busLineData.LastUpdate = item.Value<DateTime>("lastUpdate");

                var routeList = item.Value<JArray>("routes");

                foreach (JObject line in routeList.Children<JObject>())
                {
                    Route routeToAdd = new Route()
                    {
                        RouteId = line.Value<int>("routeId"),
                        AgencyId = line.Value<int>("agencyId"),
                        RouteShortName = line.Value<string>("routeShortName"),
                        RouteLongName = line.Value<string>("routeLongName"),
                        ActivationDate = line.Value<DateTime>("activationDate")
                    };

                    busLineData.Routes.Add(routeToAdd);
                }
            }

            return busLineData;
        }
    }
}