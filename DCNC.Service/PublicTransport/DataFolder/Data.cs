using DCNC.Bussiness.PublicTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Service.PublicTransport.DataFolder
{
    public class Data
    {
        public static TripData TripData;
        public static BusLineData BusLineData;
        public static BusStopData BusStopData;
        public static ExpeditionData ExpeditionData;
        public static StopInTripData StopInTripData;
        public static List<JoinedTripsModel> JoinedTrips;
        public static List<StopTripDataModel> TripsWithBusStops;
        public static string JoinedTripsAsJson;
    }
}