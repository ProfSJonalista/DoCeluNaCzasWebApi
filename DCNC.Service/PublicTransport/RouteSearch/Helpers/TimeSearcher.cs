using DCNC.Bussiness.PublicTransport.RouteSearch;
using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.Service.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCNC.Service.PublicTransport.RouteSearch.Helpers
{
    public class TimeSearcher
    {
        readonly IDocumentStoreRepository _documentStoreRepository;

        public TimeSearcher(IDocumentStoreRepository documentStoreRepository)
        {
            _documentStoreRepository = documentStoreRepository;
        }

        public Change GetChangeWithTime(Change change, bool departure, DateTime desiredTime, bool before)
        {
            var changeToReturn = new Change
            {
                BusLineName = change.BusLineName,
                RouteId = change.RouteId,
                ChangeNo = change.ChangeNo,
                StopChangeList = new List<StopChange>()
            };

            var timeTableData = _documentStoreRepository.GetTimeTableDataByRouteIdAndDayOfWeek(change.RouteId, desiredTime.DayOfWeek);

            if (timeTableData == null)
                return changeToReturn;

            var startingStopTime = GetStartingStopTime(before, departure, timeTableData.StopTimes, desiredTime, change.StopChangeList);

            var stopTimeIndex = timeTableData.StopTimes.IndexOf(startingStopTime);

            if (stopTimeIndex < 0) return changeToReturn;
            var stopTimeList = timeTableData.StopTimes.GetRange(stopTimeIndex, change.StopChangeList.Count);

            for (var i = 0; i < change.StopChangeList.Count; i++)
            {
                var stopChange = change.StopChangeList[i];
                var stopTime = stopTimeList[i];

                var stopChangeToAdd = new StopChange
                {
                    Name = stopChange.Name,
                    RouteId = stopChange.RouteId,
                    TripId = stopTime.TripId,
                    StopId = stopChange.StopId,
                    StopLat = stopChange.StopLat,
                    StopLon = stopChange.StopLon,
                    ArrivalTime = stopTime.ArrivalTime,
                    DepartureTime = stopTime.DepartureTime,
                    StopSequence = stopChange.StopSequence,
                    MainTrip = stopChange.MainTrip,
                    OnDemand = stopChange.OnDemand
                };

                changeToReturn.StopChangeList.Add(stopChangeToAdd);
            }

            changeToReturn.TripId = startingStopTime.TripId;
            changeToReturn.DepartureTime = changeToReturn.StopChangeList.First().DepartureTime;
            changeToReturn.ArrivalTime = changeToReturn.StopChangeList.Last().DepartureTime;
            changeToReturn.TimeOfTravel = changeToReturn.ArrivalTime - changeToReturn.DepartureTime;

            return changeToReturn;
        }

        static StopTime GetStartingStopTime(bool before, bool departure, List<StopTime> stopTimes, DateTime desiredTime, IReadOnlyCollection<StopChange> stopList)
        {
            StopTime startingStopTime;
            var firstStop = stopList.First();
            var lastStop = stopList.Last();

            if (before)
            {
                if (departure)
                {
                    startingStopTime = stopTimes
                        .OrderBy(x => x.DepartureTime)
                        .LastOrDefault(time =>
                            time.DepartureTime.TimeOfDay.CompareTo(desiredTime.TimeOfDay) < 0
                            && desiredTime.TimeOfDay.Subtract(time.DepartureTime.TimeOfDay).TotalMinutes < 240
                            && firstStop.StopId == time.StopId && firstStop.RouteId == time.RouteId);
                }
                else
                {
                    var lastStopTime = stopTimes
                        .OrderBy(x => x.ArrivalTime)
                        .FirstOrDefault(time =>
                            time.ArrivalTime.TimeOfDay.CompareTo(desiredTime.TimeOfDay) >= 0
                            && desiredTime.TimeOfDay.Subtract(time.ArrivalTime.TimeOfDay).TotalMinutes < 240
                            && lastStop.StopId == time.StopId && lastStop.RouteId == time.RouteId);

                    if(lastStopTime == null) return new StopTime();

                    var lastStopTimeIndex = stopTimes.IndexOf(lastStopTime);
                    var startingStopTimeIndex = lastStopTimeIndex - stopList.Count;
                    startingStopTime = stopTimes[startingStopTimeIndex];
                }
            }
            else
            {
                if (departure)
                {
                    startingStopTime = stopTimes
                        .OrderBy(x => x.DepartureTime)
                        .FirstOrDefault(time =>
                            time.DepartureTime.TimeOfDay.CompareTo(desiredTime.TimeOfDay) > 0
                            && desiredTime.TimeOfDay.Subtract(time.DepartureTime.TimeOfDay).TotalMinutes < 240
                            && firstStop.StopId == time.StopId && firstStop.RouteId == time.RouteId);
                }
                else
                {
                    var lastStopTime = stopTimes
                        .OrderBy(x => x.ArrivalTime)
                        .LastOrDefault(time =>
                            time.ArrivalTime.TimeOfDay.CompareTo(desiredTime.TimeOfDay) <= 0
                            && desiredTime.TimeOfDay.Subtract(time.ArrivalTime.TimeOfDay).TotalMinutes < 240
                            && lastStop.StopId == time.StopId && lastStop.RouteId == time.RouteId);

                    if (lastStopTime == null) return new StopTime();

                    var lastStopTimeIndex = stopTimes.IndexOf(lastStopTime);
                    var startingStopTimeIndex = lastStopTimeIndex - stopList.Count;
                    startingStopTime = stopTimes[startingStopTimeIndex];
                }
            }

            return startingStopTime;
        }
    }
}