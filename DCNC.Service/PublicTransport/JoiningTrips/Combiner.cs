using DCNC.Bussiness.PublicTransport.JoiningTrips;
using DCNC.Service.PublicTransport.JoiningTrips.Helpers;
using DCNC.Service.PublicTransport.JoiningTrips.Helpers.Keys;
using System.Collections.Generic;
using System.Linq;

namespace DCNC.Service.PublicTransport.JoiningTrips
{
    public class Combiner
    {
        readonly CombineHelper _combineHelper;

        public Combiner(CombineHelper combineHelper)
        {
            _combineHelper = combineHelper;
        }

        public CombinedTripModel CombineForEveryOption(List<OrganizedTrips> trips)
        {
            var oneWayModel = CombineForOneWay(trips.Where(x => x.Trips.ContainsKey(TripKey.START)).ToList());
            var bothWayModel = CombineForBothWays(trips.Where(x => x.Trips.ContainsKey(TripKey.START) && x.Trips.ContainsKey(TripKey.RETURN)).ToList());

            var stopsToJoin = oneWayModel.Trips.FirstOrDefault(x => x.DirectionId == TripKey.START).Stops;
            var startStops = bothWayModel.Trips.FirstOrDefault(x => x.DirectionId == TripKey.START).Stops;
            bothWayModel.Trips[TripKey.START].Stops = _combineHelper.GetStops(startStops, startStops, stopsToJoin);

            return bothWayModel;
        }

        public CombinedTripModel CombineForBothWays(List<OrganizedTrips> trips)
        {
            var firstMainStartTrip = _combineHelper.GetMainRoute(trips, TripKey.START);
            var firstMainReturnTrip = _combineHelper.GetMainRoute(trips, TripKey.RETURN);

            firstMainStartTrip.Stops = _combineHelper.GetJoinedStops(trips.Where(x => x.Trips.ContainsKey(TripKey.START)).ToList(), firstMainStartTrip.Stops, TripKey.START);
            firstMainReturnTrip.Stops = _combineHelper.GetJoinedStops(trips.Where(x => x.Trips.ContainsKey(TripKey.RETURN)).ToList(), firstMainReturnTrip.Stops, TripKey.RETURN);

            return new CombinedTripModel()
            {
                BusLineName = firstMainStartTrip.BusLineName,
                Trips = new List<Trip>() { firstMainStartTrip, firstMainReturnTrip }
            };
        }

        public CombinedTripModel CombineForOneWay(List<OrganizedTrips> trips)
        {
            var firstMainTrip = _combineHelper.GetMainRoute(trips, TripKey.START);
            firstMainTrip.Stops = _combineHelper.GetJoinedStops(trips.Where(x => x.Trips.ContainsKey(TripKey.START)).ToList(), firstMainTrip.Stops, TripKey.START);

            return new CombinedTripModel()
            {
                BusLineName = firstMainTrip.BusLineName,
                Trips = new List<Trip>() { firstMainTrip }
            };
        }
    }
}