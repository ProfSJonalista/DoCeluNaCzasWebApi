using System;
using System.Collections.Generic;

namespace DCNC.Bussiness.PublicTransport
{
    public class TimeTableData
    {
        public DateTime LastUpdate { get; set; }
        public IList<StopTime> StopTimes { get; set; }
    }

    public class StopTime
    {
        public int RouteId { get; set; }
        public int TripId { get; set; }
        public int AgencyId { get; set; }
        public int TopologyVersionId { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public int StopId { get; set; }
        public int StopSequence { get; set; }
        public DateTime Date { get; set; }
        public int VariantId { get; set; }
        public string NoteSymbol { get; set; }
        public string NoteDescription { get; set; }
        public string BusServiceName { get; set; }
        public int Order { get; set; }
        public bool NonPassenger { get; set; }
        public bool TicketZoneBorder { get; set; }
        public bool OnDemand { get; set; }
        public bool IsVirtual { get; set; }
        public int IsSlupek { get; set; }
        public bool WheelchairAccessible { get; set; }
        public int StopShortName { get; set; }
    }
}