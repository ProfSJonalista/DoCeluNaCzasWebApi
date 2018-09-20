using DCNC.Bussiness.Public_Transport;
using DCNC.Service.Public_Transport.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DCNC.Service.Public_Transport
{
    public class TimeTableService
    {
        static TripData _trips;
        static BusLineData _busLines;
        static BusStopData _busStops;
        static StopInTripData _stopsInTrips;
        static List<StopTripDataModel> _tripsWithBusStops;

        public async static Task<string> GetStopsLinkedWithTrips()
        {
            var data = "";

            //TODO - checking last updates

            if (_trips == null || (_trips.Day.Equals(DateTime.Now.ToString("yyyy-MM-dd"))) && DateTime.Now.Hour <= 6)
            {
                _trips = await TripService.GetTripData();
                _busLines = await BusLineService.GetBusLineData();
                _busStops = await BusStopService.GetBusStopData();
                _stopsInTrips = await StopInTripService.GetStopInTripData();
                _tripsWithBusStops = new List<StopTripDataModel>();
            }
            
            TripsWithBusStopsMapper();
            JoinTrips();

            return data;
        }

        private static void TripsWithBusStopsMapper()
        {
            foreach (var busLine in _busLines.Routes)
            {
                var tripListByRouteId = _trips.Trips.Where(x => x.RouteId == busLine.RouteId).ToList();

                foreach (var trip in tripListByRouteId)
                {
                    var tripToAdd = new StopTripDataModel()
                    {
                        BusLineName = busLine.RouteShortName,
                        TripHeadsign = trip.TripHeadsign,
                        Stops = new List<StopTripModel>()
                    };

                    var stops = _stopsInTrips.StopsInTrip.Where(x => x.RouteId == trip.RouteId && x.TripId == trip.TripId).ToList();

                    stops.ForEach(stop => tripToAdd.Stops.Add(StopMapper(busLine, trip, stop)));
                    
                    tripToAdd.Stops = tripToAdd.Stops.OrderBy(x => x.StopSequence).ToList();
                    _tripsWithBusStops.Add(tripToAdd);
                }
            }
        }

        private static StopTripModel StopMapper(Route busLine, Trip trip, StopInTrip stop)
        {
            var stopByStopId = _busStops.Stops.Where(x => x.StopId == stop.StopId).Single();

            return new StopTripModel()
            {
                RouteId = busLine.RouteId,
                TripId = trip.TripId,
                AgencyId = busLine.AgencyId,
                DirectionId = trip.DirectionId,
                StopId = stop.StopId,
                OnDemand = stopByStopId.OnDemand.HasValue ? stopByStopId.OnDemand.Value : false,
                StopName = stopByStopId.StopDesc,
                TripHeadsign = trip.TripHeadsign,
                StopLat = stopByStopId.StopLat,
                StopLon = stopByStopId.StopLon,
                StopSequence = stop.StopSequence,
                RouteShortName = busLine.RouteShortName
            };
        }

        private static void JoinTrips()
        {
            var distinctBusLines = _busLines.Routes.Select(x => x).Distinct().ToList();

            distinctBusLines.ForEach(busLine => JoinTripsForEachBusLine(busLine));
        }

        private static void JoinTripsForEachBusLine(Route busLine)
        {
            var tripsWithBusStopsForBusLine = _tripsWithBusStops.Where(x => x.BusLineName.Equals(busLine.RouteShortName)).ToList();

            var first = tripsWithBusStopsForBusLine.First().Stops;
            var merged = new List<StopTripModel>();

            foreach(var trip in tripsWithBusStopsForBusLine)
            {
                merged = first.Intersect(trip.Stops, new RouteComparer()).ToList();
            }
        }
    }
}