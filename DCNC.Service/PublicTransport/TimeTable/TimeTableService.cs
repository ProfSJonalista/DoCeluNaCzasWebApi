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
        static ExpeditionData _expeditionData;
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

            if (_expeditionData == null)
                _expeditionData = await ExpeditionService.GetExpeditionData();

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
                    var expedition = _expeditionData.Expeditions
                                                    .Where(x => x.RouteId == busLine.RouteId
                                                             && x.TripId == trip.TripId
                                                             && x.StartDate.ToString("yyyy-MM-dd").Equals(DateTime.Now.ToString("yyyy-MM-dd")))
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

                    var stops = _stopsInTrips.StopsInTrip.Where(x => x.RouteId == trip.RouteId && x.TripId == trip.TripId).ToList();

                    stops.ForEach(stop => tripToAdd.Stops.Add(StopHelper.Mapper(busLine, trip, stop, _busStops, expedition.MainRoute)));

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

            var grouped = DictionaryOrganizer.GroupAndOrder(tripsWithBusStopsForBusLine);
            
            foreach (var direction in grouped)
            {
                var hasMainRoute = direction.Value.Any(x => x.MainRoute);
                var mainTrip = new StopTripDataModel();

                if (hasMainRoute)
                    mainTrip = direction.Value.FirstOrDefault();
                else
                {
                    mainTrip = JoinTripHelper.GetMainTrip(mainTrip, direction.Value);
                }

                var joinedTrip = new StopTripDataModel()
                {
                    BusLineName = mainTrip.BusLineName,
                    TripHeadsign = mainTrip.TripHeadsign,
                    Stops = new List<StopTripModel>()
                };

                foreach (var trip in direction.Value)
                {
                    foreach (var stop in mainTrip.Stops)
                    {
                        foreach (var stopToCheck in trip.Stops)
                        {
                            var isSame = StopHelper.CheckIfStopsAreTheSame(stop, stopToCheck);
                            var alreadyExist = JoinTripHelper.CheckIfBusStopAlreadyExists(stopToCheck.RouteShortName, stopToCheck.StopLat, stopToCheck.StopLon, joinedTrip);

                            var indexToAdd = 0;

                            if (!isSame && !alreadyExist)
                            {
                                if (joinedTrip.Stops.Count == 0)
                                    joinedTrip.Stops.Add(stopToCheck);
                                else
                                {
                                    var index = trip.Stops.IndexOf(stopToCheck);

                                    if (index == 0)
                                    {
                                        indexToAdd = index;
                                    }
                                    else
                                    {
                                        indexToAdd = JoinTripHelper.GetIndexToAdd(trip.Stops, index, joinedTrip);
                                    }
                                }

                                joinedTrip.Stops.Insert(indexToAdd, stopToCheck);
                            }
                        }
                    }
                }
            }
        }
    }
}
