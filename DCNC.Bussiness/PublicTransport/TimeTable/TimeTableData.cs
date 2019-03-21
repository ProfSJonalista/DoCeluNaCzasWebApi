using System;
using System.Collections.Generic;

namespace DCNC.Bussiness.PublicTransport.TimeTable
{
    public class TimeTableData
    {
        public string Id { get; set; }
        public DateTime LastUpdate { get; set; }
        public List<StopTime> StopTimes { get; set; }
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
        public string BusServiceName { get; set; }
        public int Order { get; set; }
    }
}