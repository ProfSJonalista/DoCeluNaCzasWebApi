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
    public class BusStopService
    {
        static readonly ObjectCache _cache = MemoryCache.Default;

        public async static Task<BusStopData> GetData()
        {
            var json = await PublicTransportRepository.GetBusStops();
            JObject stops = (JObject)JsonConvert.DeserializeObject(json);
            BusStopData busStopDataToReturn;

            if (!stops.HasValues)
            {
                List<BusStopData> busStopDatas = _cache[CacheKeys.BUS_STOP_DATA_LIST_KEY] as List<BusStopData>;
                busStopDataToReturn = GetDataForCurrentDay(busStopDatas);

                return busStopDataToReturn;
            }

            busStopDataToReturn = CacheDataAndGetCurrentDayResult(stops);

            return busStopDataToReturn;
        }

        private static BusStopData CacheDataAndGetCurrentDayResult(JObject stops)
        {
            List<BusStopData> busStopDataToCache = new List<BusStopData>();

            foreach(var item in stops.Children())
            {
                busStopDataToCache.Add(Converter(item));
            }

            _cache.Set(CacheKeys.BUS_STOP_DATA_LIST_KEY, busStopDataToCache, new CacheItemPolicy());

            return GetDataForCurrentDay(busStopDataToCache);
        }

        private static BusStopData GetDataForCurrentDay(List<BusStopData> dataList)
        {
            return dataList.Where(x => x.Day.Date == DateTime.Now.Date).SingleOrDefault();
        }

        private static BusStopData Converter(JToken busStop)
        {
            BusStopData busStopData = new BusStopData()
            {
                Day = DateTime.Parse(busStop.Path),
                Stops = new List<Stop>()
            };

            foreach (var item in busStop.Children())
            {
                busStopData.LastUpdate = item.Value<DateTime>("lastUpdate");

                var stopList = item.Value<JArray>("stops");

                foreach (JObject stop in stopList.Children<JObject>())
                {
                    Stop stopToAdd = new Stop()
                    {
                        StopId = stop.Value<int>("stopId"),
                        StopCode = stop.Value<string>("stopCode"),
                        StopName = stop.Value<string>("stopName"),
                        StopShortName = stop.Value<string>("stopShortName"),
                        StopDesc = stop.Value<string>("stopDesc"),
                        SubName = stop.Value<string>("subName"),
                        Date = stop.Value<DateTime>("date"),
                        StopLat = stop.Value<double>("stopLat"),
                        StopLon = stop.Value<double>("stopLon"),
                        ZoneId = stop.Value<int?>("zoneId") ?? 0,
                        ZoneName = stop.Value<string>("zoneName"),
                        VirtualBusStop = stop.Value<bool?>("virtual") ?? false,
                        NonPassenger = stop.Value<bool?>("nonpassenger") ?? false,
                        Depot = stop.Value<bool?>("depot") ?? false,
                        TicketZoneBorder = stop.Value<bool?>("ticketZoneBorder") ?? false,
                        OnDemand = stop.Value<bool?>("onDemand") ?? false,
                        ActivationDate = stop.Value<DateTime>("activationDate")
                    };

                    busStopData.Stops.Add(stopToAdd);
                }
            }

            return busStopData;
        }
    }
}