using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Bussiness.PublicTransport
{
    public class BusLineData
    {
        public DateTime Day { get; set; }
        public DateTime LastUpdate { get; set; }
        public List<Route> Routes { get; set; }
    }

    public class Route
    {
        public int RouteId { get; set; }
        public int AgencyId { get; set; }
        public string RouteShortName { get; set; }
        public string RouteLongName { get; set; }
        public DateTime ActivationDate { get; set; }
    }
}