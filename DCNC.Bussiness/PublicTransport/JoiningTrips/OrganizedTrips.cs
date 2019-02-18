using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Bussiness.PublicTransport.JoiningTrips
{
    public class OrganizedTrips
    {
        public DateTime Day { get; set; }
        public string BusLineName { get; set; }
        public Dictionary<bool, List<Trip>> Trips { get; set; }
    }
}