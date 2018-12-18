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
        #region TODO REMOVE LATER
        public string JoinedTripsAsJson { get; set; }
        public string BusStopsAsJson { get; set; }
        public string BusLinesAsJson { get; set; }
        public List<JoinedTripsModel> JoinedTrips { get; set; }
        #endregion
    }
}