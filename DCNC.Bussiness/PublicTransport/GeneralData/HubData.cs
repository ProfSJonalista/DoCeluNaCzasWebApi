using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Bussiness.PublicTransport.GeneralData
{
    public class HubData
    {
        public BusLineData BusLineData { get; set; }
        public BusStopData BusStopData { get; set; }
        public List<JoinedTripsModel> JoinedTrips { get; set; }
    }
}