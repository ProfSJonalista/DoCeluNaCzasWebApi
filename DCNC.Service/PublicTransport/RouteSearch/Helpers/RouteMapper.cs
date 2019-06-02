using DCNC.Bussiness.PublicTransport.General;
using DCNC.Bussiness.PublicTransport.RouteSearch;
using System.Collections.Generic;
using System.Linq;

namespace DCNC.Service.PublicTransport.RouteSearch.Helpers
{
    public class RouteMapper
    {
        public static Change MapChange(JoinedTripModel joinedTrip, int startStopIndex, int destStopIndex, int changeNo)
        {
            var stop = joinedTrip.Stops.FirstOrDefault(x => !x.MainTrip);
            var routeId = stop?.RouteId ?? joinedTrip.RouteId;
            var tripId = stop?.TripId ?? joinedTrip.TripId;

            return new Change
            {
                BusLineName = joinedTrip.BusLineName,
                RouteId = routeId,
                TripId = tripId,
                ChangeNo = changeNo,
                StopChangeList = MapStops(joinedTrip.Stops, startStopIndex, destStopIndex)
            };
        }

        static List<StopChange> MapStops(List<JoinedStopModel> joinedTripStops, int startStopIndex, int destStopIndex)
        {
            var count = (destStopIndex - startStopIndex) + 1;
            var rangedSublist = joinedTripStops.GetRange(startStopIndex, count);
            var stopSequence = 0;
             
            var stopChangeList = rangedSublist.Select(x =>
            {
                var stopChange = new StopChange
                {
                    Name = x.Name,
                    RouteId = x.RouteId,
                    TripId = x.TripId,
                    StopId = x.StopId,
                    StopLat = x.StopLat,
                    StopLon = x.StopLon,
                    StopSequence = stopSequence,
                    MainTrip = x.MainTrip,
                    OnDemand = x.OnDemand
                };

                stopSequence++;

                return stopChange;
            }).ToList();

            return stopChangeList;
        }
    }
}