using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Bussiness.Public_Transport
{
    public class BusLineData
    {
        public string Day { get; set; }
        public string LastUpdate { get; set; }
        public List<Route> Routes { get; set; }
    }

    public class Route
    {
        public int RouteId { get; set; }
        public int AgencyId { get; set; }
        public string RouteShortName { get; set; }
        public string RouteLongName { get; set; }
        public string ActivationDate { get; set; }
    }
}