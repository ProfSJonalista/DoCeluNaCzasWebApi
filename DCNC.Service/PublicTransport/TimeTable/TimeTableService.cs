using DCNC.Bussiness.PublicTransport;
using DCNC.Service.PublicTransport.Helpers;
using DCNC.Service.PublicTransport.TimeTable.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DCNC.Service.PublicTransport.TimeTable
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

            if (_trips == null/* || (_trips.Day.Equals(DateTime.Now.ToString("yyyy-MM-dd"))) && DateTime.Now.Hour <= 6*/)
                _trips = await TripService.GetTripData();

            if (_busLines == null)
                _busLines = await BusLineService.GetBusLineData();

            if (_busStops == null)
                _busStops = await BusStopService.GetBusStopData();

            if (_stopsInTrips == null)
                _stopsInTrips = await StopInTripService.GetStopInTripData();

            if (_tripsWithBusStops == null)
            {
                _tripsWithBusStops = new List<StopTripDataModel>();
                TripsWithBusStopsMapper();
            }

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
                        ActivationDate = trip.ActivationDate,
                        Stops = new List<StopTripModel>()
                    };

                    var stops = _stopsInTrips.StopsInTrip.Where(x => x.RouteId == trip.RouteId && x.TripId == trip.TripId).ToList();

                    stops.ForEach(stop => tripToAdd.Stops.Add(StopHelper.Mapper(busLine, trip, stop, _busStops)));

                    tripToAdd.Stops = tripToAdd.Stops.OrderBy(x => x.StopSequence).ToList();
                    _tripsWithBusStops.Add(tripToAdd);

                }
            }
        }

        private static void JoinTrips()
        {
            var distinctBusLines = _busLines.Routes.GroupBy(x => x.RouteShortName).Select(x => x.FirstOrDefault()).ToList();

            distinctBusLines.ForEach(busLine => JoinTripsForEachBusLine(busLine));
        }

        private static void JoinTripsForEachBusLine(Route busLine)
        {
            var tripsWithBusStopsForBusLine = _tripsWithBusStops.Where(x => x.BusLineName.Equals(busLine.RouteShortName)).ToList();

            var grouped = DictionaryOrganizer.GroupByDirectionAndOrderByDescending(tripsWithBusStopsForBusLine);

            foreach (var direction in grouped)
            {
                var longestTrip = direction.Value.First();
                var joinedTrip = new StopTripDataModel()
                {
                    BusLineName = busLine.RouteShortName,
                    Stops = new List<StopTripModel>()
                };

                //dla każdego tripa, porównać przystanki z longestTrip
                //każdą (całkowicie) połączoną listę, dodać do głównego obiektu

                foreach (var trip in direction.Value)
                {
                    foreach (var stop in longestTrip.Stops)
                    {
                        foreach (var stopToCheck in trip.Stops)
                        {
                            var isSame = StopHelper.CheckIfStopsAreTheSame(stop, stopToCheck);

                            if (!isSame)
                            {

                            }
                            else
                            {
                                var alreadyExist = joinedTrip.Stops.Any(x => x.RouteShortName.Equals(stopToCheck.RouteShortName) && x.StopLat == stopToCheck.StopLat && x.StopLon == stopToCheck.StopLon);

                                if (!alreadyExist)
                                    joinedTrip.Stops.Add(stopToCheck);

                                continue;
                            }
                        }
                    }
                }
            }
        }
    }
}
