using DCNC.Bussiness.PublicTransport;
using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Service.PublicTransport.JsonData.Abstracts;
using DCNC.Service.PublicTransport.TimeTable.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DCNC.Bussiness.PublicTransport.JsonData.Shared;

namespace DCNC.Service.PublicTransport.JsonData
{
    public class TripService : DataAbstractService
    {
        StopHelper _stopHelper;

        public TripService()
        {
            _stopHelper = new StopHelper();
        }

        public override Common Converter(JToken trips)
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