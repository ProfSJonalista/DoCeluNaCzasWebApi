using System;

namespace DoCeluNaCzasWebApi.Models.PublicTransport.Delay
{
    public class DelayModel
    {
        public int RouteId { get; set; }
        public int TripId { get; set; }
        public string BusLineName { get; set; }
        public string Headsign { get; set; }
        public int DelayInSeconds { get; set; }
        public DateTime TheoreticalTime { get; set; }
        public DateTime EstimatedTime { get; set; }
    }
}