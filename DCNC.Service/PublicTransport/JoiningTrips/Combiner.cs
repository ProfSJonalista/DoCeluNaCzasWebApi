using DCNC.Bussiness.PublicTransport.JoiningTrips;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCNC.Service.PublicTransport.JoiningTrips
{
    public class Combiner
    {
        public CombinedTripModel CombineForEveryOption(List<OrganizedTrips> trips)
        {
            var containsOneWay = trips.Any(x => x.Trips.Values.Count == 1);
            var bothWayTrip = trips.Any(x => x.Trips.Values.Count > 1);
            var bus = trips.FirstOrDefault().BusLineName;
            var tripModel = containsOneWay
                ? CombineForOneWay(trips.Where(x => x.Trips.Values.Count == 1).ToList())
                : new CombinedTripModel();
            
            return tripModel;
        }

        public CombinedTripModel CombineForBothWays(List<OrganizedTrips> trips)
        {
            var tripModel = new CombinedTripModel();



            return tripModel;
        }

        public CombinedTripModel CombineForOneWay(List<OrganizedTrips> trips)
        {
            var firstTripDictionary = trips
                            .FirstOrDefault()
                            .Trips
                            .FirstOrDefault();
            var firstDayTripList = firstTripDictionary.Value;

            var containsMainRoute = firstDayTripList.Any(x => x.MainRoute);

            var firstTrip = containsMainRoute
                ? firstDayTripList.FirstOrDefault(x => x.MainRoute)
                : firstDayTripList.FirstOrDefault();

            var combinedTripModel = new CombinedTripModel()
            {
                BusLineName = firstTrip.BusLineName,
                Trips = new List<Trip>()
            };

            trips.ForEach(trip =>
            {
                foreach (var tripsValue in trip.Trips.Values)
                {
                    tripsValue.ForEach(x =>
                    {

                    });
                }
            });


            return combinedTripModel;
        }
    }
}