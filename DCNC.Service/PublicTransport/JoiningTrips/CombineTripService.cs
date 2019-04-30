using DCNC.Bussiness.PublicTransport.JoiningTrips;
using DCNC.Bussiness.PublicTransport.JsonData.General;
using System.Collections.Generic;
using System.Linq;

namespace DCNC.Service.PublicTransport.JoiningTrips
{
    public class CombineTripService
    {
        private readonly Combiner _combiner;

        public CombineTripService(Combiner combiner)
        {
            _combiner = combiner;
        }

        public List<CombinedTripModel> JoinTrips(List<OrganizedTrips> organizedTrips, List<Route> routes)
        {
            return routes.Select(route =>
            {
                var organizedList = organizedTrips.Where(x => x.BusLineName.Equals(route.RouteShortName)).ToList();
                return Combine(organizedList);
            })
            .OrderBy(x => x.BusLineName)
            .ToList();
        }

        private CombinedTripModel Combine(List<OrganizedTrips> trips)
        {
            var bothWayTrip = trips.Any(x => x.Trips.Values.Count > 1);
            var oneWayTrip = trips.Any(x => x.Trips.Values.Count == 1);

            if (oneWayTrip && bothWayTrip)
                return _combiner.CombineForEveryOption(trips);

            return bothWayTrip
                ? _combiner.CombineForBothWays(trips)
                : _combiner.CombineForOneWay(trips);
        }
    }
}