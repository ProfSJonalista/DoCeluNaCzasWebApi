using System.Collections.Generic;

namespace DCNC.Bussiness.PublicTransport.JsonData
{
    public class GeneralData
    {
        public TripData TripData { get; set; }
        public BusLineData BusLineData { get; set; }
        public BusStopData BusStopData { get; set; }
        public ExpeditionData ExpeditionData { get; set; }
        public StopInTripData StopInTripData { get; set; }
        public List<StopTripDataModel> TripsWithBusStops { get; set; }
        //public List<JoinedTripsViewModel> JoinedTrips { get; set; }
    }
}