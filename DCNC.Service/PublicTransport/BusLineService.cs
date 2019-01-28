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
using DCNC.Bussiness.PublicTransport.JsonData;

namespace DCNC.Service.PublicTransport
{
    public class BusLineService
    {
        static readonly ObjectCache _cache = MemoryCache.Default;
        readonly PublicTransportRepository _publicTransportRepository;

        public BusLineService()
        {
            _publicTransportRepository = new PublicTransportRepository();
        }

        public async Task<BusLineData> GetBusLineData()
        {
            var json = await _publicTransportRepository.GetBusLines();
            var lines = (JObject)JsonConvert.DeserializeObject(json);
            BusLineData busLineDataToReturn;

            if (!lines.HasValues)
            {
                var busLineDatas = _cache[CacheKeys.BUS_LINE_DATA_LIST_KEY] as List<BusLineData>;
                busLineDataToReturn = GetDataForCurrentDay(busLineDatas);

                return busLineDataToReturn;
            }

            busLineDataToReturn = CacheBusLineDataAndGetCurrentDayResult(lines);

            return busLineDataToReturn;
        }

        private BusLineData CacheBusLineDataAndGetCurrentDayResult(JObject lines)
        {
            var busLineDatasToCache = new List<BusLineData>();

            foreach (var item in lines.Children())
            {
                busLineDatasToCache.Add(BusLineConverter(item));
            }

            _cache.Set(CacheKeys.BUS_LINE_DATA_LIST_KEY, busLineDatasToCache, new CacheItemPolicy());

            return GetDataForCurrentDay(busLineDatasToCache);
        }

        private BusLineData GetDataForCurrentDay(IEnumerable<BusLineData> busLineDataList)
        {
            return busLineDataList.SingleOrDefault(x => x.Day.Date == DateTime.Now.Date);
        }
        
        private BusLineData BusLineConverter(JToken busLine)
        {
            var busLineData = new BusLineData()
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
                    var routeToAdd = new Route()
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