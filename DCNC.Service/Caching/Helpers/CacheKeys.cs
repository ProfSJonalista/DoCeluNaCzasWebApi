namespace DCNC.Service.Caching.Helpers
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

        public static string BUS_STOP_DATA_MODEL = "BusStopDataModel";
        public static string GROUPED_JOINED_TRIPS = "GroupedJoinedTrips";
        public static string CHOOSE_BUS_STOP_MODEL_OBSERVABALE_COLLECTION = "ChooseBusStolModelObservableCollection";

        #endregion
    }
}