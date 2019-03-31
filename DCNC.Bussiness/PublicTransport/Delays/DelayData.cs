using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Bussiness.PublicTransport.Delays
{
    public class DelayData
    {
        public DateTime LastUpdate { get; set; }
        public List<Delay> Delays { get; set; }
    }

    public class Delay
    {
        public string Id { get; set; }
        public int DelayInSeconds { get; set; }
        public DateTime EstimatedTime { get; set; }
        public string Headsign { get; set; }
        public int RouteId { get; set; }
        public int TripId { get; set; }
        public DateTime TheoreticalTime { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}