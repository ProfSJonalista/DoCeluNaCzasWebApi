using DCNC.Bussiness.PublicTransport.JoiningTrips;
using System.Collections.Generic;

namespace DCNC.Service.PublicTransport.JoiningTrips.Helpers
{
    public class Organizer
    {
        public Dictionary<int, List<Trip>> GetTrips(List<Trip> busLineTrips)
        {
            var orgTrips = new Dictionary<int, List<Trip>>();

            busLineTrips.ForEach(trip =>
            {
                var key = trip.DirectionId;

                if (!orgTrips.ContainsKey(key))
                {
                    orgTrips.Add(key, new List<Trip> { trip });
                }
                else
                {
                    orgTrips[key].Add(trip);
                }
            });

            return orgTrips;
        }
    }
}