using DCNC.Bussiness.PublicTransport.JoinedTrips;
using System.Collections.Generic;

namespace DCNC.Bussiness.PublicTransport.GeneralData
{
    public class Data
    {
        public TripData TripData { get; set; }
        public BusLineData BusLineData { get; set; }
        public BusStopData BusStopData { get; set; }
        public ExpeditionData ExpeditionData { get; set; }
        public StopInTripData StopInTripData { get; set; }
        public List<StopTripDataModel> TripsWithBusStops { get; set; }
        public List<JoinedTripsViewModel> JoinedTrips { get; set; }
    }
}