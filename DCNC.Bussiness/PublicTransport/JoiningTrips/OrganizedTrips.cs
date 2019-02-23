using System;
using System.Collections.Generic;

namespace DCNC.Bussiness.PublicTransport.JoiningTrips
{
    public class OrganizedTrips
    {
        public DateTime Day { get; set; }
        public string BusLineName { get; set; }
        public Dictionary<int, List<Trip>> Trips { get; set; }
    }
}