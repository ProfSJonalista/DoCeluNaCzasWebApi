using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DCNC.Bussiness.PublicTransport;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers
{
    public class JoinTripHelper
    {
        internal static int GetIndexToAdd(List<StopTripModel> stops, int index, StopTripDataModel joinedTrip)
        {
            var previousStop = stops[index - 1];
            var previousStopInMainList = joinedTrip.Stops.Where(x => x.StopId == previousStop.StopId).SingleOrDefault();

            return joinedTrip.Stops.IndexOf(previousStopInMainList);
        }

        public static bool CheckIfBusStopAlreadyExists(string routeShortName, double stopLat, double stopLon, StopTripDataModel joinedTrip)
        {
            return joinedTrip.Stops.Any(x => x.RouteShortName.Equals(routeShortName) && x.StopLat == stopLat && x.StopLon == stopLon);
        }

        internal static StopTripDataModel GetMainTrip(StopTripDataModel mainTrip, List<StopTripDataModel> trips)
        {
            trips = trips.OrderByDescending(x => x.Stops.Count).ToList();
            mainTrip = trips.FirstOrDefault();
            mainTrip.MainRoute = true;
            mainTrip.Stops.ForEach(x => x.BelongsToMainTrip = true);

            return mainTrip;
        }
    }
}