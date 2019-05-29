using DCNC.Bussiness.PublicTransport.General;
using DCNC.Bussiness.PublicTransport.TimeTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.TimeTable.Helpers
{
    public class MinuteTimeTableBuilder
    {
        readonly StopTimesFetcher _stopTimesFetcher;

        public MinuteTimeTableBuilder(StopTimesFetcher stopTimesFetcher)
        {
            _stopTimesFetcher = stopTimesFetcher;
        }

        public List<MinuteTimeTable> BuildList(List<MinuteTimeTable> minuteTimeTableList, List<int> routeIds, List<JoinedTripModel> joinedTrips)
        {
            var (weekdayStopTimes, saturdayStopTimes, sundayStopTimes) = _stopTimesFetcher.FetchStopTimes(routeIds);

            foreach (var tripModel in joinedTrips)
            {
                foreach (var stopModel in tripModel.Stops)
                {
                    var mtt = minuteTimeTableList.FirstOrDefault(x => x.StopId == stopModel.StopId) ??
                              new MinuteTimeTable
                              {
                                  BusLineName = tripModel.BusLineName,
                                  StopId = stopModel.StopId,
                                  RouteIds = routeIds,
                                  MinuteDictionary = new Dictionary<DayType, Dictionary<int, List<int>>>(),
                                  ModMinuteDictionary = new Dictionary<DayType, Dictionary<int, string>>()
                              };

                    foreach (var dayType in (DayType[])Enum.GetValues(typeof(DayType)))
                    {
                        var containsMdKey = mtt.MinuteDictionary.ContainsKey(dayType);
                        var containsMmdKey = mtt.ModMinuteDictionary.ContainsKey(dayType);

                        if (!containsMdKey)
                            mtt.MinuteDictionary.Add(dayType, new Dictionary<int, List<int>>());

                        if (!containsMmdKey)
                            mtt.ModMinuteDictionary.Add(dayType, new Dictionary<int, string>());

                        switch (dayType)
                        {
                            case DayType.Weekday:
                                mtt.MinuteDictionary[dayType] = Build(mtt.MinuteDictionary[dayType], stopModel, weekdayStopTimes);
                                mtt.ModMinuteDictionary[dayType] = Build(mtt.MinuteDictionary[dayType]);
                                break;
                            case DayType.Saturday:
                                mtt.MinuteDictionary[dayType] = Build(mtt.MinuteDictionary[dayType], stopModel, saturdayStopTimes);
                                mtt.ModMinuteDictionary[dayType] = Build(mtt.MinuteDictionary[dayType]);
                                break;
                            case DayType.Sunday:
                                mtt.MinuteDictionary[dayType] = Build(mtt.MinuteDictionary[dayType], stopModel, sundayStopTimes);
                                mtt.ModMinuteDictionary[dayType] = Build(mtt.MinuteDictionary[dayType]);
                                break;
                        }
                    }

                    var minuteTimeTableIndex = minuteTimeTableList.FindIndex(x => x.Id == mtt.Id);

                    if (minuteTimeTableIndex > 0)
                        minuteTimeTableList[minuteTimeTableIndex] = mtt;
                    else
                        minuteTimeTableList.Add(mtt);
                }
            }

            return minuteTimeTableList;
        }

        static Dictionary<int, string> Build(Dictionary<int, List<int>> hourAndMinuteDictionary)
        {
            var sb = new StringBuilder();
            var modDic = new Dictionary<int, string>();

            for (var hour = 0; hour < 24; hour++)
            {
                var minutes = hourAndMinuteDictionary[hour];

                if (minutes.Count > 0)
                {
                    foreach (var item in minutes)
                    {
                        var itemAsString = item.ToString().PadLeft(2, '0');
                        sb.Append(itemAsString);
                        sb.Append("    "); //4 spaces
                    }

                    var index = sb.Length - 4;
                    sb.Remove(index, 4);
                }

                modDic.Add(hour, sb.ToString());
                sb.Clear();
            }

            return modDic;
        }

        static Dictionary<int, List<int>> Build(Dictionary<int, List<int>> dayTypeDictionary, JoinedStopModel stopModel, IReadOnlyCollection<StopTime> stopTimes)
        {
            for (var hour = 0; hour < 24; hour++)
            {
                if (stopTimes.Count <= 0) continue;

                var containsHour = dayTypeDictionary.ContainsKey(hour);

                if (!containsHour)
                    dayTypeDictionary.Add(hour, new List<int>());

                var minutes = stopTimes
                    .Where(x =>
                        x.RouteId == stopModel.RouteId &&
                        x.TripId == stopModel.TripId &&
                        x.StopId == stopModel.StopId &&
                        x.DepartureTime.Hour == hour)
                    .Select(y => y.DepartureTime.Minute)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                dayTypeDictionary[hour] = minutes;
            }

            return dayTypeDictionary;
        }
    }
}