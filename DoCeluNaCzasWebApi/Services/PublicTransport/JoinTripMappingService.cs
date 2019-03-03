using DCNC.Bussiness.PublicTransport.JoiningTrips;
using DoCeluNaCzasWebApi.Models.PublicTransport;
using DoCeluNaCzasWebApi.Services.PublicTransport.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace DoCeluNaCzasWebApi.Services.PublicTransport
{
    public class JoinTripMappingService
    {
        private readonly JoinTripHelper _joinTripHelper;
        public JoinTripMappingService()
        {
            _joinTripHelper = new JoinTripHelper();
        }
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

        private List<JoinedTripModel> GetMappedTrips(List<Trip> trips)
        {
            return trips.Select(x => new JoinedTripModel()
            {
                BusLineName = x.BusLineName,
                TripId = x.TripId,
                RouteId = x.RouteId,
                FirstStopName = _joinTripHelper.GetFirstStopName(x.TripHeadsign),
                DestinationStopName = _joinTripHelper.GetDestinationStopName(x.TripHeadsign),
                MainRoute = x.MainRoute,
                DirectionId = x.DirectionId,
                Stops = GetMappedStops(x.Stops)
            }).ToList();
        }

        private List<JoinedStopModel> GetMappedStops(List<Stop> stops)
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