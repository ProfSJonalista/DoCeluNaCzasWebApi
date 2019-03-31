namespace DCNC.Service.PublicTransport.Caching.Helpers
{
    public static class CacheKeys
    {
        #region Last updates

        public static string TRIP_DATA_LAST_UPDATE = "TripDataLastUpdate";
        public static string EXPEDITION_LAST_UPDATE = "ExpeditionLastUpdate";
        public static string BUS_LINE_DATA_LAST_UPDATE = "BusLineDataLastUpdate";
        public static string BUS_STOP_DATA_LAST_UPDATE = "BusStopDataLastUpdate";
        public static string STOP_IN_TRIP_DATA_LAST_UPDATE = "StopInTripDataLastUpdate";
        
        #endregion

        #region Data to share

        public static string JOINED_TRIP_MODEL_LIST = "JoinedTripModelList";
        public static string JOINED_BUS_STOPS = "JoinedBusStops";

        #endregion

        #region Delays

        public static string TRIP_DATA = "TripData";
        public static string BUS_LINE_DATA = "BusLineData";

        #endregion
    }
}