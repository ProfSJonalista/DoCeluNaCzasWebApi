using DCNC.Bussiness.PublicTransport.JoiningTrips;
using DCNC.Bussiness.PublicTransport.JsonData.General;
using System.Collections.Generic;
using System.Linq;

namespace DCNC.Service.PublicTransport.JoiningTrips
{
    public class CombineTripService
    {
        private readonly Combiner _combiner;

        public CombineTripService()
        {
            _combiner = new Combiner();
        }

        public List<CombinedTripModel> JoinTrips(List<OrganizedTrips> organizedTrips, List<Route> routes)
        {
            var combinedTripList = new List<CombinedTripModel>();

            routes.ForEach(route => combinedTripList
                                            .Add(Combine(organizedTrips
                                            .Where(x => x.BusLineName.Equals(route.RouteShortName))
                                            .ToList())));

            return combinedTripList.OrderBy(x => x.BusLineName).ToList();
        }

        private CombinedTripModel Combine(List<OrganizedTrips> trips)
        {
            var bothWayTrip = trips.Any(x => x.Trips.Values.Count > 1);
            var oneWayTrip = trips.Any(x => x.Trips.Values.Count == 1);

            if (oneWayTrip && bothWayTrip)
                return _combiner.CombineForEveryOption(trips);
            else if (bothWayTrip)
                return _combiner.CombineForBothWays(trips);
            else
                return _combiner.CombineForOneWay(trips);
        }
    }
}