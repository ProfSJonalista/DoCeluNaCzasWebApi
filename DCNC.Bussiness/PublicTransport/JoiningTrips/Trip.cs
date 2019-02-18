using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Bussiness.PublicTransport.JoiningTrips
{
    public class Trip
    {
        public string Id { get; set; }
        public int TripId { get; set; }
        public int RouteId { get; set; }
        public string BusLineName { get; set; }
        public bool MainRoute { get; set; }
        public string TripHeadsign { get; set; }
        public int DirectionId { get; set; }
        public List<Stop> Stops { get; set; }
    }

    public class Stop
    {
        public string Name { get; set; }
        public int RouteId { get; set; }
        public int TripId { get; set; }
        public int StopId { get; set; }
        public double StopLat { get; set; }
        public double StopLon { get; set; }
        public bool? TicketZoneBorder { get; set; }
        public bool? OnDemand { get; set; }
        public string ZoneName { get; set; }
        public int StopSequence { get; set; }
    }
}