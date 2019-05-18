using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DCNC.Bussiness.PublicTransport.TimeTable;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.TimeTable.Helpers
{
    public class MinuteTimeTableBuilder
    {
        private readonly DateChecker _dateChecker;

        public MinuteTimeTableBuilder(DateChecker dateChecker)
        {
            _dateChecker = dateChecker;
        }
        //todo modify combine all stoptimes by line from all routeIds
        public Dictionary<DayType, Dictionary<int, List<int>>> Build(DayType dayType, List<TimeTableData> timeTableDataList,
            Dictionary<DayType, Dictionary<int, List<int>>> minuteDictionary, int tripStopStopId)
        {
            var day = _dateChecker.GetProperDate(dayType, timeTableDataList);

            if (day == null) return minuteDictionary;

            Dictionary<int, List<int>> hourAndMinuteDictionary;

            if (minuteDictionary.ContainsKey(dayType))
            {
                hourAndMinuteDictionary = minuteDictionary[dayType];
            }
            else
            {
                hourAndMinuteDictionary = new Dictionary<int, List<int>>();
                minuteDictionary.Add(dayType, hourAndMinuteDictionary);
            }

            for (var hour = 0; hour < 24; hour++)
            {
                var containsHour = hourAndMinuteDictionary.ContainsKey(hour);
                var stopTimesByHour = day.StopTimes.Where(x => x.StopId == tripStopStopId && x.DepartureTime.Hour == hour).ToList();
                var minuteList = stopTimesByHour.Select(x => x.DepartureTime.Minute).OrderBy(y => y).ToList();
                 
                if (!containsHour)
                {
                    hourAndMinuteDictionary.Add(hour, new List<int>());
                }

                hourAndMinuteDictionary[hour] = minuteList;
            }

            return minuteDictionary;
        }
    }
}