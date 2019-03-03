using DCNC.Bussiness.PublicTransport.JoiningTrips;
using System.Collections.Generic;
using System.Linq;

namespace DCNC.Service.PublicTransport.JoiningTrips.Helpers
{
    public class CombineHelper
    {
        private readonly StopComparer _stopComparer;

        public CombineHelper()
        {
            _stopComparer = new StopComparer();
        }
        public Trip GetMainRoute(List<OrganizedTrips> trips, int key)
        {
            var firstDayTripList = trips.FirstOrDefault(x => x.Trips.ContainsKey(key))?.Trips[key];
            var containsMainRoute = firstDayTripList.Any(x => x.MainRoute);

            return containsMainRoute
                   ? firstDayTripList.FirstOrDefault(x => x.MainRoute)
                   : SetMainRoute(firstDayTripList.FirstOrDefault());
        }

        private Trip SetMainRoute(Trip trip)
        {
            trip.MainRoute = true;
            trip.Stops.ForEach(stop => stop.MainTrip = true);
            return trip;
        }

        public List<Stop> GetJoinedStops(List<OrganizedTrips> trips, List<Stop> mainTripStopList, int key)
        {
            var stopListToReturn = new List<Stop>();

            trips.ForEach(organizedTrip =>
            {
                organizedTrip.Trips[key].ForEach(trip =>
                {
                    mainTripStopList.ForEach(stop =>
                    {
                        trip.Stops.ForEach(possibleStopToAdd =>
                        {
                            var alreadyExists = stopListToReturn.Contains(possibleStopToAdd, _stopComparer);

                            if (alreadyExists) return;

                            if (stopListToReturn.Count > 0)
                            {
                                var index = trip.Stops.IndexOf(possibleStopToAdd);

                                if (index > stopListToReturn.Count)
                                    stopListToReturn.Add(possibleStopToAdd);
                                else
                                    stopListToReturn.Insert(index, possibleStopToAdd);
                            }
                            else
                                stopListToReturn.Add(possibleStopToAdd);
                        });
                    });
                });
            });

            return stopListToReturn;
        }
    }
}