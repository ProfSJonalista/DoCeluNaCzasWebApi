using DCNC.Bussiness.PublicTransport;
using DCNC.DataAccess.PublicTransport;
using DCNC.Service.PublicTransport.Resources;
using DCNC.Service.PublicTransport.TimeTable.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using DCNC.Bussiness.PublicTransport.JsonData;

namespace DCNC.Service.PublicTransport
{
    public class TripService
    {
        StopHelper _stopHelper;
        PublicTransportRepository _publicTransportRepository;
        static readonly ObjectCache _cache = MemoryCache.Default;

        public TripService()
        {
            _stopHelper = new StopHelper();
            _publicTransportRepository = new PublicTransportRepository();
        }

        public async Task<TripData> GetData()
        {
            var json = await _publicTransportRepository.GetTrips();
            JObject trips = (JObject)JsonConvert.DeserializeObject(json);
            TripData tripDataToReturn;

            if (!trips.HasValues)
            {
                List<TripData> tripDatas = _cache[CacheKeys.TRIP_DATA_LIST_KEY] as List<TripData>;
                tripDataToReturn = GetDataForCurrentDay(tripDatas);

                return tripDataToReturn;
            }

            tripDataToReturn = CacheDataAndGetCurrentDayResult(trips);

            return TripConverter(trips.First);
        }

        private TripData CacheDataAndGetCurrentDayResult(JObject trips)
        {
            List<TripData> tripDataToCache = new List<TripData>();

            foreach (var item in trips.Children())
            {
                tripDataToCache.Add(TripConverter(item));
            }

            _cache.Set(CacheKeys.TRIP_DATA_LIST_KEY, tripDataToCache, new CacheItemPolicy());

            return GetDataForCurrentDay(tripDataToCache);
        }

        private TripData GetDataForCurrentDay(List<TripData> dataList)
        {
            return dataList.Where(x => x.Day.Date == DateTime.Now.Date).SingleOrDefault();
        }

        private TripData TripConverter(JToken trips)
        {
            var tripData = new TripData()
            {
                Day = DateTime.Parse(trips.Path),
                Trips = new List<Trip>()
            };

            foreach (JObject item in trips.Children<JObject>())
            {
                tripData.LastUpdate = item.Value<DateTime>("lastUpdate");

                var tripList = item.Value<JArray>("trips");

                foreach (JObject trip in tripList.Children<JObject>())
                {
                    Trip tripToAdd = new Trip()
                    {
                        Id = trip.Value<string>("id"),
                        TripId = trip.Value<int>("tripId"),
                        RouteId = trip.Value<int>("routeId"),
                        TripHeadsign = trip.Value<string>("tripHeadsign"),
                        TripShortName = trip.Value<string>("tripShortName"),
                        DirectionId = trip.Value<int>("directionId"),
                        ActivationDate = trip.Value<DateTime>("activationDate")
                    };

                    tripData.Trips.Add(tripToAdd);
                }
            }

            return tripData;
        }

        public List<StopTripDataModel> TripsWithBusStopsMapper(BusLineData busLineData, TripData tripData, StopInTripData stopInTripData, ExpeditionData expeditionData, BusStopData busStopData)
        {
            var tripsWithBusStopsList = new List<StopTripDataModel>();

            foreach (var busLine in busLineData.Routes)
            {
                var tripListByRouteId = tripData.Trips.Where(x => x.RouteId == busLine.RouteId).ToList();

                foreach (var trip in tripListByRouteId)
                {
                    var expedition = expeditionData.Expeditions
                                               .Where(x => x.RouteId == busLine.RouteId
                                               && x.TripId == trip.TripId
                                               && (x.StartDate == DateTime.Now || x.StartDate < DateTime.Now))
                                               .ToList()
                                               .FirstOrDefault();

                    if (expedition.TechnicalTrip)
                        continue;

                    var tripToAdd = new StopTripDataModel()
                    {
                        BusLineName = busLine.RouteShortName,
                        TripHeadsign = trip.TripHeadsign,
                        MainRoute = expedition.MainRoute,
                        TechnicalTrip = expedition.TechnicalTrip,
                        ActivationDate = trip.ActivationDate,
                        Stops = new List<StopTripModel>()
                    };

                    var stops = stopInTripData.StopsInTrip.Where(x => x.RouteId == trip.RouteId && x.TripId == trip.TripId).ToList();

                    stops.ForEach(stop => tripToAdd.Stops.Add(_stopHelper.Mapper(busLine, trip, stop, busStopData, expedition.MainRoute)));

                    tripToAdd.Stops = tripToAdd.Stops.OrderBy(x => x.StopSequence).ToList();
                    tripsWithBusStopsList.Add(tripToAdd);

                }
            }

            return tripsWithBusStopsList;
        }
    }
}