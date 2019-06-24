using DCNC.Bussiness.PublicTransport.JoiningTrips;
using DCNC.Bussiness.PublicTransport.JsonData.General;
using DCNC.Service.PublicTransport.JoiningTrips.Helpers;
using System.Collections.Generic;
using System.Linq;
using Trip = DCNC.Bussiness.PublicTransport.JoiningTrips.Trip;

namespace DCNC.Service.PublicTransport.JoiningTrips
{
    public class TripsWithBusStopsService
    {
        readonly Organizer _organizer;
        readonly StopHelper _stopHelper;

        public TripsWithBusStopsService(Organizer organizer, StopHelper stopHelper)
        {
            _organizer = organizer;
            _stopHelper = stopHelper;
        }

        public List<TripsWithBusStops> GetTripsWithBusStops(List<TripData> tripDataList, List<BusStopData> busStopDataList, List<BusLineData> busLineDataList, List<StopInTripData> stopInTripDataList, ExpeditionData expeditionObject)
        {
            var tripsWithBusStops = new List<TripsWithBusStops>();

            busLineDataList.ForEach(busLine => tripsWithBusStops.Add(
                Map(busLine, expeditionObject,
                tripDataList.SingleOrDefault(trip => trip.Day == busLine.Day),
                busStopDataList.SingleOrDefault(stop => stop.Day == busLine.Day),
                stopInTripDataList.SingleOrDefault(stopInTrip => stopInTrip.Day == busLine.Day)
                )));

            return tripsWithBusStops;
        }

        TripsWithBusStops Map(BusLineData busLine, ExpeditionData expeditionObject, TripData tripData, BusStopData busStopData, StopInTripData stopInTripData)
        {
            var tripsWithBusStops = new TripsWithBusStops()
            {
                Day = busLine.Day,
                Trips = new List<Trip>()
            };

            busLine.Routes.ForEach(route =>
            {
                var tripListByRouteId = tripData.Trips.Where(x => x.RouteId == route.RouteId).ToList();

                tripListByRouteId.ForEach(tripByRouteId =>
                {
                    var expedition = expeditionObject.Expeditions
                                        .FirstOrDefault(exp => exp.RouteId == tripByRouteId.RouteId &&
                                                                exp.TripId == tripByRouteId.TripId &&
                                                                (exp.StartDate.Date == busLine.Day.Date || exp.StartDate.Date < busLine.Day.Date));

                    if (expedition == null || expedition.TechnicalTrip)
                        return;

                    tripsWithBusStops.Trips.Add(
                        new Trip()
                        {
                            Id = tripByRouteId.Id,
                            TripId = tripByRouteId.TripId,
                            RouteId = tripByRouteId.RouteId,
                            AgencyId = route.AgencyId,
                            BusLineName = route.RouteShortName,
                            MainRoute = expedition.MainRoute,
                            TripHeadsign = tripByRouteId.TripHeadsign,
                            DirectionId = tripByRouteId.DirectionId,
                            Stops = _stopHelper.GetStopList(tripByRouteId, stopInTripData.StopsInTrip.Where(x => x.RouteId == tripByRouteId.RouteId && x.TripId == tripByRouteId.TripId).ToList(), expedition.MainRoute, busStopData.Stops)
                        });
                });
            });

            tripsWithBusStops.Trips = tripsWithBusStops.Trips.OrderBy(x => x.BusLineName).ToList();

            return tripsWithBusStops;
        }

        public List<OrganizedTrips> OrganizeTrips(List<TripsWithBusStops> tripsWithBusStops, List<BusLineData> busLineDataList)
        {
            var organizedTrips = new List<OrganizedTrips>();
            tripsWithBusStops.ForEach(day =>
            {
                var distinctRoutes = busLineDataList
                                     .SingleOrDefault(x => x.Day.Date == day.Day.Date)
                                     .Routes
                                     .GroupBy(x => x.RouteShortName)
                                     .Select(x => x.FirstOrDefault())
                                     .ToList();

                distinctRoutes.ForEach(route =>
                {
                    var busLineTrips = day.Trips
                                          .Where(trip => trip.BusLineName.Equals(route.RouteShortName))
                                          .ToList();

                    organizedTrips.Add(new OrganizedTrips()
                    {
                        Day = day.Day,
                        BusLineName = route.RouteShortName,
                        Trips = _organizer.GetTrips(busLineTrips)
                    });
                });
            });

            return organizedTrips.OrderBy(x => x.BusLineName).ToList();
        }
    }
}
