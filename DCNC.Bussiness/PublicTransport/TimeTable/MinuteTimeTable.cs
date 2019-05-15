using System.Collections.Generic;
using DCNC.Bussiness.PublicTransport.TimeTable.Shared;

namespace DCNC.Bussiness.PublicTransport.TimeTable
{
    public class MinuteTimeTable : Common
    {
        public string BusLineName { get; set; }
        public int StopId { get; set; }
        public List<int> RouteIds { get; set; }
        public Dictionary<DayType, Dictionary<int, List<int>>> MinuteDictionary { get; set; }
    }

    public enum DayType
    {
        Weekday, Saturday, SundayAndHoliday
    }
}