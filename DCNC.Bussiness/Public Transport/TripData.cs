using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Bussiness.Public_Transport
{
    public class TripData
    {
        public string Day { get; set; }
        public string LastUpdate { get; set; }
        public IList<Trip> Trips { get; set; }
    }

    public class Trip
    {
        public string Id { get; set; }
        public int TripId { get; set; }
        public int RouteId { get; set; }
        public string TripHeadsign { get; set; }
        public string TripShortName { get; set; }
        public int DirectionId { get; set; }
        public string ActivationDate { get; set; }
    }
}