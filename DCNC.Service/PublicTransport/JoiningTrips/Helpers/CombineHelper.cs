using DCNC.Bussiness.PublicTransport.JoiningTrips;
using System.Collections.Generic;
using System.Linq;

namespace DCNC.Service.PublicTransport.JoiningTrips.Helpers
{
    public class CombineHelper
    {
        readonly StopComparer _stopComparer;

        public CombineHelper(StopComparer stopComparer)
        {
            _stopComparer = stopComparer;
        }

        public Trip GetMainRoute(List<OrganizedTrips> trips, int key)
        {
            var firstDayTripList = trips.FirstOrDefault(x => x.Trips.ContainsKey(key))?.Trips[key];
            var containsMainRoute = firstDayTripList.Any(x => x.MainRoute);

            return containsMainRoute
                   ? firstDayTripList.FirstOrDefault(x => x.MainRoute)
                   : SetMainRoute(firstDayTripList.FirstOrDefault());
        }

        static Trip SetMainRoute(Trip trip)
        {
            trip.MainRoute = true;
            trip.Stops.ForEach(stop => stop.MainTrip = true);
            return trip;
        }

        public List<Stop> GetJoinedStops(List<OrganizedTrips> trips, List<Stop> mainTripStopList, int key)
        {
            var stopListToReturn = new List<Stop>();

            foreach (var organizedTrip in trips)
            {
                foreach (var trip in organizedTrip.Trips[key])
                {
                    stopListToReturn = GetStops(trip.Stops, stopListToReturn);
                }
            }

            return stopListToReturn;
        }

        public List<Stop> GetStops(List<Stop> stopsToJoin, List<Stop> stopListToReturn)
        {
            foreach (var possibleStopToAdd in stopsToJoin)
            {
                var alreadyExists = stopListToReturn.Contains(possibleStopToAdd, _stopComparer);

                if (alreadyExists) continue;

                if (stopListToReturn.Count < 0)
                {
                    var index = stopsToJoin.IndexOf(possibleStopToAdd);

                    if (index > stopListToReturn.Count)
                        stopListToReturn.Add(possibleStopToAdd);
                    else
                        stopListToReturn.Insert(index, possibleStopToAdd);
                }
                else
                    stopListToReturn.Add(possibleStopToAdd);
            }

            return stopListToReturn;
        }
    }
}