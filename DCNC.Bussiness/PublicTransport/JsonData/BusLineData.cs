﻿using System;
using System.Collections.Generic;
using DCNC.Bussiness.PublicTransport.JsonData.Shared;

namespace DCNC.Bussiness.PublicTransport.JsonData
{
    public class BusLineData : Common
    {
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