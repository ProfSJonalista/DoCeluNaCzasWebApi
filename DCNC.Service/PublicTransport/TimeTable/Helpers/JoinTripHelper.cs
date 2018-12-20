using DCNC.Bussiness.PublicTransport;
using DCNC.Bussiness.PublicTransport.JoinedTrips;
using System.Collections.Generic;
using System.Linq;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers
{
    public class JoinTripHelper
    {
        MapperService _mapperService;

        public JoinTripHelper()
        {
            _mapperService = new MapperService();
        }

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

        public List<JoinedTripsViewModel> JoinedTripsMapper(List<JoinedTripsModel> joinedTripsModelList)
        {
            List<JoinedTripsViewModel> listToReturn = new List<JoinedTripsViewModel>();

            joinedTripsModelList.ForEach(x => listToReturn.Add(_mapperService.JoinTripsMapper(x)));

            return listToReturn;
        }
    }
}