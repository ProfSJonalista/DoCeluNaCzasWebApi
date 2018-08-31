using DCNC.Bussiness.Public_Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DCNC.Service.Public_Transport
{
    public class TimeTableService
    {
        public async static Task<string> GetStopsLinkedWithTrips()
        {
            var data = "";

            var trips = await TripService.GetTripData();
            var busLines = await BusLineService.GetBusLineData();
            var busStops = await BusStopService.GetBusStopData();
            var stopsInTrips = await StopInTripService.GetStopInTripData();
            var tripsWithBusStops = new List<StopTripDataModel>();
            foreach (var busLine in busLines.Routes)
            {
                var tripListByRouteId = trips.Trips.Where(x => x.RouteId == busLine.RouteId).ToList();

                foreach(var trip in tripListByRouteId)
                {
                    var tripToAdd = new StopTripDataModel()
                    {
                        BusLineName = busLine.RouteShortName,
                        TripHeadsign = trip.TripHeadsign,
                        Stops = new List<StopTripModel>()
                    };

                    var stops = stopsInTrips.StopsInTrip.Where(x => x.RouteId == trip.RouteId && x.TripId == trip.TripId).ToList();

                    foreach (var stop in stops)
                    {
                        
                        var stopByStopId = busStops.Stops.Where(x => x.StopId == stop.StopId).Single();

                        var stopToAdd = new StopTripModel()
                        {
                            RouteId = busLine.RouteId,
                            TripId = trip.TripId,
                            AgencyId = busLine.AgencyId,
                            DirectionId = trip.DirectionId,
                            StopId = stop.StopId,
                            OnDemand = stopByStopId.OnDemand.HasValue ? stopByStopId.OnDemand.Value : false,
                            StopName = stopByStopId.StopDesc,
                            TripHeadsign = trip.TripHeadsign,
                            StopLat = stopByStopId.StopLat,
                            StopLon = stopByStopId.StopLon,
                            StopSequence = stop.StopSequence,
                            RouteShortName = busLine.RouteShortName
                        };

                        tripToAdd.Stops.Add(stopToAdd);
                    }
                    
                    tripToAdd.Stops = tripToAdd.Stops.OrderBy(x => x.StopSequence).ToList();
                    tripsWithBusStops.Add(tripToAdd);
                }
            }

            var sevensixty = tripsWithBusStops.Where(x => x.BusLineName.Equals("770")).ToList();


            return data;
        }
    }
}