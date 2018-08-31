using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Bussiness.Public_Transport
{
    public class StopTripDataModel
    {
        public string BusLineName { get; set; }
        public string TripHeadsign { get; set; }
        public List<StopTripModel> Stops { get; set; }
    }

    public class StopTripModel
    {
        public int RouteId { get; set; }
        public int TripId { get; set; }
        public int AgencyId { get; set; }
        public int DirectionId { get; set; }
        public int StopId { get; set; }
        public string StopName { get; set; }
        public string TripHeadsign { get; set; }
        public bool OnDemand { get; set; }
        public double StopLat { get; set; }
        public double StopLon { get; set; }
        public int StopSequence { get; set; }
        public string RouteShortName { get; set; }
    }
}