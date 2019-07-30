using DCNC.Bussiness.PublicTransport.TimeTable;
using System;
using System.Collections.Generic;
using System.Linq;
using DCNC.Bussiness.PublicTransport.JoiningTrips;

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
                                  StopId = stopModel.StopId,
                                  StopName = stopModel.Name,
                                  BusLineName = tripModel.BusLineName,
                                  RouteIds = routeIds,
                                  MinuteDictionary = new Dictionary<DayType, Dictionary<int, List<int>>>()
                              };

                    foreach (var dayType in (DayType[])Enum.GetValues(typeof(DayType)))
                    {
                        var containsMdKey = mtt.MinuteDictionary.ContainsKey(dayType);

                        if (!containsMdKey) mtt.MinuteDictionary.Add(dayType, new Dictionary<int, List<int>>());

                        switch (dayType)
                        {
                            case DayType.Weekday:
                                mtt.MinuteDictionary[dayType] = Build(mtt.MinuteDictionary[dayType], stopModel, weekdayStopTimes);
                                break;
                            case DayType.Saturday:
                                mtt.MinuteDictionary[dayType] = Build(mtt.MinuteDictionary[dayType], stopModel, saturdayStopTimes);
                                break;
                            case DayType.Sunday:
                                mtt.MinuteDictionary[dayType] = Build(mtt.MinuteDictionary[dayType], stopModel, sundayStopTimes);
                                break;
                        }
                    }

                    int minuteTimeTableIndex;

                    if (mtt.Id == null) minuteTimeTableIndex = -1;
                    else minuteTimeTableIndex = minuteTimeTableList.FindIndex(x => x.Id.Equals(mtt.Id));

                    if (minuteTimeTableIndex >= 0)
                        minuteTimeTableList[minuteTimeTableIndex] = mtt;
                    else
                        minuteTimeTableList.Add(mtt);
                }
            }

            return minuteTimeTableList;
        }

        static Dictionary<int, List<int>> Build(Dictionary<int, List<int>> dayTypeDictionary, JoinedStopModel stopModel, IReadOnlyCollection<StopTime> stopTimes)
        {
            for (var hour = 0; hour < 24; hour++)
            {
                if (!dayTypeDictionary.ContainsKey(hour)) dayTypeDictionary.Add(hour, new List<int>());
                if (stopTimes.Count <= 0) continue;

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