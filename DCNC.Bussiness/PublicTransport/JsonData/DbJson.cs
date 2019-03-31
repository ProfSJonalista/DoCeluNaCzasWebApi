namespace DCNC.Bussiness.PublicTransport.JsonData
{
    public class DbJson
    {
        public string Id { get; set; }
        public string Json { get; set; }
        public JsonType Type { get; set; }
    }

    public enum JsonType
    {
        BusLine, BusStop, Expedition, StopInTrip, Trip, StopTime, Delay
    }
}