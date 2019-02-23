using System;
using System.Collections.Generic;

namespace DCNC.Bussiness.PublicTransport.JoiningTrips
{
    public class CombinedTripModel
    {
        public string BusLineName { get; set; }
        public List<Trip> Trips { get; set; }
    }
}