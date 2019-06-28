using DCNC.Bussiness.PublicTransport.General;
using DCNC.Bussiness.PublicTransport.RouteSearch;
using System.Collections.Generic;
using System.Linq;
using DCNC.Bussiness.PublicTransport.JoiningTrips;

namespace DCNC.Service.PublicTransport.RouteSearch.Helpers
{
    public class RouteMapper
    {
        public static Change MapChange(Trip trip, int startStopIndex, int destStopIndex, int changeNo)
        {
            return new Change
            {
                BusLineName = trip.BusLineName,
                RouteId = trip.RouteId,
                TripId = trip.TripId,
                ChangeNo = changeNo,
                StopChangeList = MapStops(trip.Stops, startStopIndex, destStopIndex)
            };
        }

        static List<StopChange> MapStops(List<Stop> tripStops, int startStopIndex, int destStopIndex)
        {
            var count = (destStopIndex - startStopIndex) + 1;
            var rangedSublist = tripStops.GetRange(startStopIndex, count);
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