using System;
using System.Collections.Generic;
using DCNC.Bussiness.PublicTransport.JsonData.Shared;

namespace DCNC.Bussiness.PublicTransport.JsonData
{
    public class TripData : Common
    {
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
        public DateTime ActivationDate { get; set; }
    }
}