using System;
using System.Collections.Generic;

namespace DCNC.Bussiness.PublicTransport.JoiningTrips
{
    public class TripsWithBusStops
    {
        public DateTime Day { get; set; }
        public List<Trip> Trips { get; set; }
    }
}