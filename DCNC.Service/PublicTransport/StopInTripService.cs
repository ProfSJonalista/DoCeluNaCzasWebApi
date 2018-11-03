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
    public class StopInTripService
    {
        static readonly ObjectCache _cache = MemoryCache.Default;

        public async static Task<StopInTripData> GetStopInTripData()
        {
            
            var json = await PublicTransportRepository.GetStopsInTrips();
            JObject stops = (JObject)JsonConvert.DeserializeObject(json);
            StopInTripData stopInTripDataToReturn;

            if (!stops.HasValues)
            {
                List<StopInTripData> stopInTripDatas = _cache[CacheKeys.STOP_IN_TRIP_DATA_LIST_KEY] as List<StopInTripData>;
                stopInTripDataToReturn = stopInTripDatas.Where(x => x.Day == DateTime.Now).SingleOrDefault();

                return stopInTripDataToReturn;
            }

            stopInTripDataToReturn = CacheStopInTripDataAndGetFirstResult(stops);

            return stopInTripDataToReturn;
        }

        private static StopInTripData CacheStopInTripDataAndGetFirstResult(JObject stops)
        {
            List<StopInTripData> stopInTripDataToCache = new List<StopInTripData>();

            foreach(var item in stops.Children())
            {
                stopInTripDataToCache.Add(StopInTripConverter(item));
            }

            _cache.Set(CacheKeys.STOP_IN_TRIP_DATA_LIST_KEY, stopInTripDataToCache, new CacheItemPolicy());

            return stopInTripDataToCache.FirstOrDefault();
        }

        private static StopInTripData StopInTripConverter(JToken stops)
        {
            StopInTripData stopInTripData = new StopInTripData()
            {
                Day = DateTime.Parse(stops.Path),
                StopsInTrip = new List<StopInTrip>()
            };

            foreach(var item in stops.Children())
            {
                stopInTripData.LastUpdate = item.Value<DateTime>("lastUpdate");

                var stopList = item.Value<JArray>("stopsInTrip");

                foreach(JObject stop in stopList)
                {
                    StopInTrip stopInTripToAdd = new StopInTrip()
                    {
                        RouteId = stop.Value<int>("routeId"),
                        TripId = stop.Value<int>("tripId"),
                        StopId = stop.Value<int>("stopId"),
                        StopSequence = stop.Value<int>("stopSequence"),
                        AgencyId = stop.Value<int>("agencyId"),
                        TopologyVersionId = stop.Value<int>("topologyVersionId"),
                        TripActivationDate = stop.Value<DateTime>("tripActivationDate"),
                        StopActivationDate = stop.Value<DateTime>("stopActivationDate")
                    };

                    stopInTripData.StopsInTrip.Add(stopInTripToAdd);
                }
            }

            return stopInTripData;
        }
    }
}