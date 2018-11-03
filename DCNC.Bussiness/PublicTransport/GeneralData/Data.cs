using DCNC.Bussiness.PublicTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Bussiness.PublicTransport.GeneralData
{
    public class Data
    {
        public TripData TripData { get; set; }
        public BusLineData BusLineData { get; set; }
        public BusStopData BusStopData { get; set; }
        public ExpeditionData ExpeditionData { get; set; }
        public StopInTripData StopInTripData { get; set; }
        public List<JoinedTripsModel> JoinedTrips { get; set; }
        public List<StopTripDataModel> TripsWithBusStops { get; set; }
        public string JoinedTripsAsJson { get; set; }
    }
}