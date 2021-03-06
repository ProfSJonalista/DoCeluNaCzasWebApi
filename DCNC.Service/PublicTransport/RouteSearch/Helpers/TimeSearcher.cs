﻿using DCNC.Bussiness.PublicTransport.RouteSearch;
using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.DataAccess.PublicTransport;
using DCNC.Service.Database.Interfaces;
using DCNC.Service.PublicTransport.Delays;
using DCNC.Service.PublicTransport.JsonData.Delays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCNC.Service.PublicTransport.RouteSearch.Helpers
{
    public class TimeSearcher
    {
        static readonly int totalMinutes = 240;

        DelayService _delayService = new DelayService(new DelayJsonService(new PublicTransportRepository()));

        readonly IDocumentStoreRepository _documentStoreRepository;

        public TimeSearcher(IDocumentStoreRepository documentStoreRepository)
        {
            _documentStoreRepository = documentStoreRepository;
        }

        public async Task<Change> GetChangeWithTime(Change change, DateTime desiredTime, bool departure, bool before)
        {
            var changeToReturn = new Change
            {
                BusLineName = change.BusLineName,
                RouteId = change.RouteId,
                ChangeNo = change.ChangeNo,
                FirstStop = new StopChange(),
                LastStop = new StopChange(),
                StopChangeList = new List<StopChange>()
            };

            var timeTableData = _documentStoreRepository.GetTimeTableDataByRouteIdAndDayOfWeek(change.RouteId, desiredTime.DayOfWeek);

            if (timeTableData == null)
                return changeToReturn;

            var (startingStopTime, stopTimeIndex) = GetStartingStopTime(before, departure, timeTableData.StopTimes, desiredTime, change.StopChangeList);

            if (stopTimeIndex < 0)
                return changeToReturn;

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

                var delayModel = await _delayService.GetOneDelay(stopChangeToAdd);

                if (delayModel != null)
                    stopChangeToAdd.EstimatedTime = delayModel.EstimatedTime;

                changeToReturn.StopChangeList.Add(stopChangeToAdd);
            }

            changeToReturn.TripId = startingStopTime.TripId;
            changeToReturn.FirstStop = changeToReturn.StopChangeList.First();
            changeToReturn.LastStop = changeToReturn.StopChangeList.Last();
            changeToReturn.TimeOfTravel = changeToReturn.LastStop.ArrivalTime - changeToReturn.FirstStop.DepartureTime;

            return changeToReturn;
        }

        static (StopTime, int) GetStartingStopTime(bool before, bool departure, IList<StopTime> stopTimes, DateTime desiredTime, IReadOnlyCollection<StopChange> stopList)
        {
            StopTime startingStopTime;

            var firstStop = stopList.First();
            var lastStop = stopList.Last();

            if (before)
            {
                if (departure)
                {
                    startingStopTime =
                        stopTimes
                        .OrderBy(x => x.DepartureTime)
                        .LastOrDefault(time =>
                            time.DepartureTime.TimeOfDay.CompareTo(desiredTime.TimeOfDay) < 0
                            && desiredTime.TimeOfDay.Subtract(time.DepartureTime.TimeOfDay).TotalMinutes < totalMinutes
                            && firstStop.StopId == time.StopId && firstStop.RouteId == time.RouteId && firstStop.TripId == time.TripId) ??
                        stopTimes
                        .OrderBy(x => x.DepartureTime)
                        .LastOrDefault(time =>
                            time.DepartureTime.TimeOfDay.CompareTo(desiredTime.TimeOfDay) < 0
                            && desiredTime.TimeOfDay.Subtract(time.DepartureTime.TimeOfDay).TotalMinutes < totalMinutes
                            && firstStop.StopId == time.StopId && firstStop.RouteId == time.RouteId);
                }
                else
                {
                    var lastStopTime = 
                        stopTimes
                            .OrderBy(x => x.ArrivalTime)
                            .LastOrDefault(time =>
                                time.ArrivalTime.TimeOfDay.CompareTo(desiredTime.TimeOfDay) < 0
                                && desiredTime.TimeOfDay.Subtract(time.ArrivalTime.TimeOfDay).TotalMinutes < totalMinutes
                                && lastStop.StopId == time.StopId && lastStop.RouteId == time.RouteId && lastStop.TripId == time.TripId) ??
                        stopTimes
                            .OrderBy(x => x.ArrivalTime)
                            .LastOrDefault(time =>
                                time.ArrivalTime.TimeOfDay.CompareTo(desiredTime.TimeOfDay) < 0
                                && desiredTime.TimeOfDay.Subtract(time.ArrivalTime.TimeOfDay).TotalMinutes < totalMinutes
                                && lastStop.StopId == time.StopId && lastStop.RouteId == time.RouteId);

                    startingStopTime = GetStartingStopTime(lastStopTime, stopTimes, stopList.Count);
                }
            }
            else
            {
                if (departure)
                {
                    startingStopTime = 
                        stopTimes
                            .OrderBy(x => x.DepartureTime)
                            .FirstOrDefault(time =>
                                time.DepartureTime.TimeOfDay.CompareTo(desiredTime.TimeOfDay) >= 0
                                && desiredTime.TimeOfDay.Subtract(time.DepartureTime.TimeOfDay).TotalMinutes < totalMinutes
                                && firstStop.StopId == time.StopId && firstStop.RouteId == time.RouteId && firstStop.TripId == time.TripId) ??
                        stopTimes
                            .OrderBy(x => x.DepartureTime)
                            .FirstOrDefault(time =>
                                time.DepartureTime.TimeOfDay.CompareTo(desiredTime.TimeOfDay) >= 0
                                && desiredTime.TimeOfDay.Subtract(time.DepartureTime.TimeOfDay).TotalMinutes < totalMinutes
                                && firstStop.StopId == time.StopId && firstStop.RouteId == time.RouteId);
                }
                else
                {
                    var lastStopTime = 
                        stopTimes
                            .OrderBy(x => x.ArrivalTime)
                            .FirstOrDefault(time =>
                                time.ArrivalTime.TimeOfDay.CompareTo(desiredTime.TimeOfDay) >= 0
                                && desiredTime.TimeOfDay.Subtract(time.ArrivalTime.TimeOfDay).TotalMinutes < totalMinutes
                                && lastStop.StopId == time.StopId && lastStop.RouteId == time.RouteId && lastStop.StopId == time.StopId) ??
                        stopTimes
                            .OrderBy(x => x.ArrivalTime)
                            .FirstOrDefault(time =>
                                time.ArrivalTime.TimeOfDay.CompareTo(desiredTime.TimeOfDay) >= 0
                                && desiredTime.TimeOfDay.Subtract(time.ArrivalTime.TimeOfDay).TotalMinutes < totalMinutes
                                && lastStop.StopId == time.StopId && lastStop.RouteId == time.RouteId);

                    startingStopTime = GetStartingStopTime(lastStopTime, stopTimes, stopList.Count);
                }
            }

            return (startingStopTime, stopTimes.IndexOf(startingStopTime));
        }

        static StopTime GetStartingStopTime(StopTime lastStopTime, IList<StopTime> stopTimes, int stopListCount)
        {
            if (lastStopTime == null)
                return new StopTime();

            var lastStopTimeIndex = stopTimes.IndexOf(lastStopTime);
            var startingStopTimeIndex = lastStopTimeIndex - stopListCount;
            var startingStopTime = stopTimes[startingStopTimeIndex];

            return startingStopTime;
        }
    }
}