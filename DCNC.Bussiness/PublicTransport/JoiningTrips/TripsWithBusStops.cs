using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Configuration;

namespace DCNC.Bussiness.PublicTransport.JoiningTrips
{
    public class TripsWithBusStops
    {
        public DateTime Day { get; set; }
        public List<Trip> Trips { get; set; }
    }
}