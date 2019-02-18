using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DCNC.Bussiness.PublicTransport.JsonData;
using Stop = DCNC.Bussiness.PublicTransport.JoiningTrips.Stop;

namespace DCNC.Service.PublicTransport.JoiningTrips.Helpers
{
    public class StopHelper
    {
        public List<Stop> GetStopList(Trip tripByRouteId, List<StopInTrip> stopsInTrip, bool expeditionMainRoute, List<Bussiness.PublicTransport.JsonData.Stop> stops)
        {
            return stopsInTrip.Select(stopInTrip =>
            {
                var stop = stops.FirstOrDefault(x => x.StopId == stopInTrip.StopId);

                {
                    return new Stop()
                    {
                        Name = stop.StopDesc,
                        RouteId = tripByRouteId.RouteId,
                        TripId = tripByRouteId.TripId,
                        StopId = stop.StopId,
                        StopLat = stop.StopLat,
                        StopLon = stop.StopLon,
                        TicketZoneBorder = stop.TicketZoneBorder ?? false,
                        OnDemand = stop.OnDemand ?? false,
                        ZoneName = stop.ZoneName,
                        StopSequence = stopInTrip.StopSequence
                    };
                }
            }).ToList();
        }

        //internal bool CheckIfStopsAreTheSame(StopTripModel stop, StopTripModel stopToCheck)
        //{
        //    return stop.RouteShortName.Equals(stopToCheck.RouteShortName) && stop.StopLat == stopToCheck.StopLat && stop.StopLon == stopToCheck.StopLon;
        //}
    }
}