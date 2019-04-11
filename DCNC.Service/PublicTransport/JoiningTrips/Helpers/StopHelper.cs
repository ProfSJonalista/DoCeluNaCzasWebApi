using DCNC.Bussiness.PublicTransport.JsonData.General;
using System.Collections.Generic;
using System.Linq;
using Stop = DCNC.Bussiness.PublicTransport.JoiningTrips.Stop;

namespace DCNC.Service.PublicTransport.JoiningTrips.Helpers
{
    public class StopHelper
    {
        public List<Stop> GetStopList(Trip tripByRouteId, List<StopInTrip> stopsInTrip, bool expeditionMainRoute, List<Bussiness.PublicTransport.JsonData.General.Stop> stops)
        {
            return stopsInTrip.Select(stopInTrip =>
            {
                var stop = stops.FirstOrDefault(x => x.StopId == stopInTrip.StopId);
                if (stop == null) return new Stop();
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
                        StopSequence = stopInTrip.StopSequence,
                        MainTrip = expeditionMainRoute
                    };
                }
            }).Where(x => x.StopId != 0)
              .OrderBy(x => x.StopSequence)
              .ToList();
        }
    }
}