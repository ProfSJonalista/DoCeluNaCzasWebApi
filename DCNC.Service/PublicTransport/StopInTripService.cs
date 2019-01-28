using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.DataAccess.PublicTransport;
using DCNC.Service.PublicTransport.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace DCNC.Service.PublicTransport
{
    public class StopInTripService
    {
        static readonly ObjectCache _cache = MemoryCache.Default;
        PublicTransportRepository _publicTransportRepository;

        public StopInTripService()
        {
            _publicTransportRepository = new PublicTransportRepository();
        }

        public async Task<StopInTripData> GetStopInTripData()
        {
            
            var json = await _publicTransportRepository.GetStopsInTrips();
            JObject stops = (JObject)JsonConvert.DeserializeObject(json);
            StopInTripData stopInTripDataToReturn;

            if (!stops.HasValues)
            {
                List<StopInTripData> stopInTripDatas = _cache[CacheKeys.STOP_IN_TRIP_DATA_LIST_KEY] as List<StopInTripData>;
                stopInTripDataToReturn = GetDataForCurrentDay(stopInTripDatas);

                return stopInTripDataToReturn;
            }

            stopInTripDataToReturn = CacheStopInTripDataAndGetCurrentDayResult(stops);

            return stopInTripDataToReturn;
        }

        private StopInTripData CacheStopInTripDataAndGetCurrentDayResult(JObject stops)
        {
            List<StopInTripData> stopInTripDataToCache = new List<StopInTripData>();

            foreach(var item in stops.Children())
            {
                stopInTripDataToCache.Add(StopInTripConverter(item));
            }

            _cache.Set(CacheKeys.STOP_IN_TRIP_DATA_LIST_KEY, stopInTripDataToCache, new CacheItemPolicy());

            return GetDataForCurrentDay(stopInTripDataToCache);
        }
        private StopInTripData GetDataForCurrentDay(List<StopInTripData> dataList)
        {
            return dataList.Where(x => x.Day.Date == DateTime.Now.Date).SingleOrDefault();
        }

        private StopInTripData StopInTripConverter(JToken stops)
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