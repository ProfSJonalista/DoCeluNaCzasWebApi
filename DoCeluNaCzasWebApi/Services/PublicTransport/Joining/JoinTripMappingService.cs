using DCNC.Bussiness.PublicTransport.JoiningTrips;
using DoCeluNaCzasWebApi.Services.PublicTransport.Joining.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.Joining
{
    public class JoinTripMappingService
    {
        public List<JoinedTripsModel> Map(List<CombinedTripModel> joinedTripList)
        {
            return joinedTripList.Select(trip => new JoinedTripsModel()
            {
                BusLineName = trip.BusLineName,
                ContainsMultiplyTrips = trip.Trips.Count > 1,
                JoinedTrips = GetMappedTrips(trip.Trips)
            }).OrderBy(x => x.BusLineName)
              .ToList();
        }

        List<JoinedTripModel> GetMappedTrips(List<Trip> trips)
        {
            return trips.Select(x => new JoinedTripModel()
            {
                BusLineName = x.BusLineName,
                TripId = x.TripId,
                RouteId = x.RouteId,
                AgencyId = x.AgencyId,
                FirstStopName = JoinTripHelper.GetFirstStopName(x.TripHeadsign),
                DestinationStopName = JoinTripHelper.GetDestinationStopName(x.TripHeadsign),
                MainRoute = x.MainRoute,
                DirectionId = x.DirectionId,
                Stops = GetMappedStops(x.Stops)
            }).ToList();
        }

        List<JoinedStopModel> GetMappedStops(List<Stop> stops)
        {
            return stops.Select(x => new JoinedStopModel()
            {
                Name = x.Name,
                RouteId = x.RouteId,
                TripId = x.TripId,
                StopId = x.StopId,
                StopLat = x.StopLat,
                StopLon = x.StopLon,
                TicketZoneBorder = x.TicketZoneBorder,
                OnDemand = x.OnDemand,
                StopSequence = x.StopSequence,
                MainTrip = x.MainTrip
            }).ToList();
        }
    }
}