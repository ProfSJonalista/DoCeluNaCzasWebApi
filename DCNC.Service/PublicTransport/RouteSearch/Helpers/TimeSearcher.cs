using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DCNC.Bussiness.PublicTransport.RouteSearch;
using DCNC.Service.Database.Interfaces;

namespace DCNC.Service.PublicTransport.RouteSearch.Helpers
{
    public class TimeSearcher
    {
        readonly IDocumentStoreRepository _documentStoreRepository;

        public TimeSearcher(IDocumentStoreRepository documentStoreRepository)
        {
            _documentStoreRepository = documentStoreRepository;
        }


        public Change GetChangeBefore(Change change, bool departure, DateTime desiredTime)
        {
            var changeToReturn = new Change()
            {
                BusLineName = change.BusLineName,
                RouteId = change.RouteId,
                TripId = change.TripId,
                ChangeNo = change.ChangeNo,
                StopChangeList = new List<StopChange>()
            };

            var timeTableData = _documentStoreRepository.GetTimeTableDataByRouteIdAndDayOfWeek(change.RouteId, desiredTime.DayOfWeek);

            if (timeTableData == null) return changeToReturn;

            foreach (var stopChange in change.StopChangeList)
            {
                var stopChangeToAdd = new StopChange()
                {
                    Name = stopChange.Name,
                    RouteId = stopChange.RouteId,
                    TripId = stopChange.TripId,
                    StopId = stopChange.StopId,
                    StopLat = stopChange.StopLat,
                    StopLon = stopChange.StopLon,
                    StopSequence = stopChange.StopSequence,
                    MainTrip = stopChange.MainTrip,
                    OnDemand = stopChange.OnDemand
                };

                if (departure)
                {
                    var stopTime = timeTableData.StopTimes
                        .OrderBy(x => x.DepartureTime)
                        .LastOrDefault(time =>
                            time.DepartureTime.TimeOfDay.CompareTo(desiredTime.TimeOfDay) < 0 &&
                            desiredTime.TimeOfDay.Subtract(time.DepartureTime.TimeOfDay).TotalMinutes < 240 &&
                            stopChangeToAdd.StopId == time.StopId && stopChangeToAdd.RouteId == time.RouteId &&
                            stopChangeToAdd.TripId == time.TripId);

                    if (stopTime == null)
                    {
                        changeToReturn.StopChangeList.Add(stopChangeToAdd);
                        continue;
                    }

                    stopChangeToAdd.DepartureTime = stopTime.DepartureTime;
                    stopChangeToAdd.ArrivalTime = stopTime.ArrivalTime;
                }
                else
                {
                    var stopTime = timeTableData.StopTimes
                        .OrderBy(x => x.ArrivalTime)
                        .FirstOrDefault(e =>
                            e.ArrivalTime.TimeOfDay.CompareTo(desiredTime.TimeOfDay) >= 0 &&
                            desiredTime.TimeOfDay.Subtract(e.ArrivalTime.TimeOfDay).TotalMinutes < 240 &&
                            stopChangeToAdd.StopId == e.StopId && stopChangeToAdd.RouteId == e.RouteId && stopChangeToAdd.TripId == e.TripId);

                    if (stopTime == null)
                    {
                        changeToReturn.StopChangeList.Add(stopChangeToAdd);
                        continue;
                    }

                    stopChangeToAdd.DepartureTime = stopTime.DepartureTime;
                    stopChangeToAdd.ArrivalTime = stopTime.ArrivalTime;
                }

                changeToReturn.StopChangeList.Add(stopChangeToAdd);
            }

            changeToReturn.DepartureTime = changeToReturn.StopChangeList.First().DepartureTime;
            changeToReturn.ArrivalTime = changeToReturn.StopChangeList.Last().DepartureTime;
            changeToReturn.TimeOfTravel = changeToReturn.ArrivalTime - changeToReturn.DepartureTime;

            return changeToReturn;
        }

        public Change GetChangeAfter(Change change, bool departure, DateTime desiredTime)
        {
            var changeToReturn = new Change()
            {
                BusLineName = change.BusLineName,
                RouteId = change.RouteId,
                TripId = change.TripId,
                ChangeNo = change.ChangeNo,
                StopChangeList = new List<StopChange>()
            };

            var timeTableData = _documentStoreRepository.GetTimeTableDataByRouteIdAndDayOfWeek(change.RouteId, desiredTime.DayOfWeek);

            if (timeTableData == null) return changeToReturn;

            foreach (var stopChange in change.StopChangeList)
            {
                var stopChangeToAdd = new StopChange()
                {
                    Name = stopChange.Name,
                    RouteId = stopChange.RouteId,
                    TripId = stopChange.TripId,
                    StopId = stopChange.StopId,
                    StopLat = stopChange.StopLat,
                    StopLon = stopChange.StopLon,
                    StopSequence = stopChange.StopSequence,
                    MainTrip = stopChange.MainTrip,
                    OnDemand = stopChange.OnDemand
                };

                if (departure)
                {
                    var stopTime = timeTableData.StopTimes
                        .OrderBy(x => x.DepartureTime)
                        .FirstOrDefault(time =>
                            time.DepartureTime.TimeOfDay.CompareTo(desiredTime.TimeOfDay) > 0 &&
                            desiredTime.TimeOfDay.Subtract(time.DepartureTime.TimeOfDay).TotalMinutes < 240 &&
                            stopChangeToAdd.StopId == time.StopId && stopChangeToAdd.RouteId == time.RouteId &&
                            stopChangeToAdd.TripId == time.TripId);

                    if (stopTime == null)
                    {
                        changeToReturn.StopChangeList.Add(stopChangeToAdd);
                        continue;
                    }

                    stopChangeToAdd.DepartureTime = stopTime.DepartureTime;
                    stopChangeToAdd.ArrivalTime = stopTime.ArrivalTime;
                }
                else
                {
                    var stopTime = timeTableData.StopTimes
                        .OrderBy(x => x.ArrivalTime)
                        .LastOrDefault(e =>
                            e.ArrivalTime.TimeOfDay.CompareTo(desiredTime.TimeOfDay) <= 0 &&
                            desiredTime.TimeOfDay.Subtract(e.ArrivalTime.TimeOfDay).TotalMinutes < 240 &&
                            stopChangeToAdd.StopId == e.StopId && stopChangeToAdd.RouteId == e.RouteId && stopChangeToAdd.TripId == e.TripId);

                    if (stopTime == null)
                    {
                        changeToReturn.StopChangeList.Add(stopChangeToAdd);
                        continue;
                    }

                    stopChangeToAdd.DepartureTime = stopTime.DepartureTime;
                    stopChangeToAdd.ArrivalTime = stopTime.ArrivalTime;
                }

                changeToReturn.StopChangeList.Add(stopChangeToAdd);
            }

            changeToReturn.DepartureTime = changeToReturn.StopChangeList.First().DepartureTime;
            changeToReturn.ArrivalTime = changeToReturn.StopChangeList.Last().DepartureTime;
            changeToReturn.TimeOfTravel = changeToReturn.ArrivalTime - changeToReturn.DepartureTime;

            return changeToReturn;
        }
    }
}