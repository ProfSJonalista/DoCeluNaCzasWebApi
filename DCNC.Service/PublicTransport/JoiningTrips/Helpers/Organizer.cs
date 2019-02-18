using System.Collections.Generic;
using DCNC.Bussiness.PublicTransport.JoiningTrips;

namespace DCNC.Service.PublicTransport.JoiningTrips.Helpers
{
    public class Organizer
    {
        public Dictionary<bool, List<Trip>> GetTrips(List<Trip> busLineTrips)
        {
            var orgTrips = new Dictionary<bool, List<Trip>>();

            busLineTrips.ForEach(trip =>
            {
                var key = trip.DirectionId == 1;

                if (!orgTrips.ContainsKey(key))
                {
                    orgTrips.Add(key, new List<Trip>() { trip
                    });
                }
                else
                {
                    orgTrips[key].Add(trip);
                }
            });

            return orgTrips;
        }

        //private Dictionary<bool, List<StopTripDataModel>> Order(Dictionary<bool, List<StopTripDataModel>> groupedByDirection)
        //{
        //    var orderedDictionary = new Dictionary<bool, List<StopTripDataModel>>();

        //    foreach (var direction in groupedByDirection)
        //    {
        //        var hasMainRoute = direction.Value.Any(x => x.MainRoute);
        //        var orderedList = new List<StopTripDataModel>();
        //        if (hasMainRoute)
        //        {
        //            orderedList = direction.Value.OrderByDescending(x => x.MainRoute).ToList();
        //        }
        //        else
        //        {
        //            orderedList = direction.Value.OrderByDescending(x => x.Stops.Count).ToList();
        //            var mainRoute = orderedList.FirstOrDefault();

        //            orderedList.Remove(mainRoute);

        //            mainRoute.MainRoute = true;
        //            mainRoute.Stops.ForEach(x => x.BelongsToMainTrip = true);

        //            orderedList.Insert(0, mainRoute);                    
        //        }

        //        orderedDictionary.Add(direction.Key, orderedList);
        //    }

        //    return orderedDictionary;
        //}
    }
}