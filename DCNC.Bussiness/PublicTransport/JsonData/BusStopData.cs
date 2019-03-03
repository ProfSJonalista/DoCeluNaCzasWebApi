using DCNC.Bussiness.PublicTransport.JsonData.Shared;
using System;
using System.Collections.Generic;

namespace DCNC.Bussiness.PublicTransport.JsonData
{
    public class BusStopData : Common
    {
        public List<Stop> Stops { get; set; }
    }

    public class Stop
    {
        public int StopId { get; set; }
        public string StopCode { get; set; }
        public string StopName { get; set; }
        public string StopShortName { get; set; }
        public string StopDesc { get; set; }
        public string SubName { get; set; }
        public DateTime Date { get; set; }
        public double StopLat { get; set; }
        public double StopLon { get; set; }
        public int? ZoneId { get; set; }
        public string ZoneName { get; set; }
        public bool? VirtualBusStop { get; set; }
        public bool? NonPassenger { get; set; }
        public bool? Depot { get; set; }
        public bool? TicketZoneBorder { get; set; }
        public bool? OnDemand { get; set; }
        public DateTime ActivationDate { get; set; }
    }
}