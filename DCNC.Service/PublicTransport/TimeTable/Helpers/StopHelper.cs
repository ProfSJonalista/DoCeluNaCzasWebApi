using DCNC.Bussiness.PublicTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers
{
    public class StopHelper
    {
        internal StopTripModel Mapper(Route busLine, Trip trip, StopInTrip stop, BusStopData busStops, bool belongsToMainRoute)
        {
            var stopByStopId = busStops.Stops.Where(x => x.StopId == stop.StopId).SingleOrDefault();

            return new StopTripModel()
            {
                RouteId = busLine.RouteId,
                TripId = trip.TripId,
                AgencyId = busLine.AgencyId,
                DirectionId = trip.DirectionId,
                StopId = stop.StopId,
                OnDemand = stopByStopId.OnDemand ?? false,
                StopName = stopByStopId.StopDesc,
                TripHeadsign = trip.TripHeadsign,
                StopLat = stopByStopId.StopLat,
                StopLon = stopByStopId.StopLon,
                StopSequence = stop.StopSequence,
                RouteShortName = busLine.RouteShortName,
                BelongsToMainTrip = belongsToMainRoute
            };
        }

        internal bool CheckIfStopsAreTheSame(StopTripModel stop, StopTripModel stopToCheck)
        {
            return stop.RouteShortName.Equals(stopToCheck.RouteShortName) && stop.StopLat == stopToCheck.StopLat && stop.StopLon == stopToCheck.StopLon;
        }
    }
}