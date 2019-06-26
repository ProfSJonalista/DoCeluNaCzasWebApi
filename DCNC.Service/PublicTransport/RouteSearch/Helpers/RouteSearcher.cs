using DCNC.Bussiness.PublicTransport.Delays;
using DCNC.Bussiness.PublicTransport.JoiningTrips;
using DCNC.Bussiness.PublicTransport.RouteSearch;
using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DCNC.Service.PublicTransport.RouteSearch.Helpers
{
    public class RouteSearcher
    {
        readonly string[] separator = { ", " };

        public List<Route> GetDirectLines(IEnumerable<Trip> trips, int startStopId, int destStopId)
        {
            var routesToReturn = new List<Route>();
            var listToIterate = trips.Where(x =>
                x.Stops.Any(y => y.StopId == startStopId)
                && x.Stops.Any(y => y.StopId == destStopId));

            foreach (var trip in listToIterate)
            {
                var startStopIndex = trip.Stops.FindIndex(x => x.StopId == startStopId);
                var destStopIndex = trip.Stops.FindIndex(x => x.StopId == destStopId);

                if (startStopIndex >= destStopIndex) continue;

                var routeToAdd = new Route { ChangeList = new List<Change>() };
                var changeToAdd = RouteMapper.MapChange(trip, startStopIndex, destStopIndex, 0);

                routeToAdd.ChangeList.Add(changeToAdd);
                routesToReturn.Add(routeToAdd);
            }

            return routesToReturn;
        }

        public List<Route> GetRoutes(List<Trip> trips, int startStopId, int destStopId)
        {
            var routesToReturn = new List<Route>();
            var listToIterate = trips.Where(x => x.Stops.FindIndex(y => y.StopId == startStopId) > -1);
            var stopListWithConnectedBusLines = CacheService.GetData<ObservableCollection<ChooseBusStopModel>>(CacheKeys.CHOOSE_BUS_STOP_MODEL_OBSERVABALE_COLLECTION);

            foreach (var trip in listToIterate)
            {
                var startStopIndex = trip.Stops.FindIndex(x => x.StopId == startStopId);
                var count = (trip.Stops.Count - startStopIndex);
                var stopSubList = trip.Stops.GetRange(startStopIndex, count);

                foreach (var stop in stopSubList)
                {
                    var possibleChanges = GetPossibleChanges(stopListWithConnectedBusLines, stop, trips, destStopId);

                    var currentStopIndex = stopSubList.IndexOf(stop);

                    foreach (var option in possibleChanges)
                    {
                        var secondChangeStopIndex = option.Stops.FindIndex(x => x.Name.Equals(stop.Name));
                        if (secondChangeStopIndex < 0)
                            continue;

                        var destStop = stopListWithConnectedBusLines.SingleOrDefault(x => x.StopId == destStopId);
                        var destStopIndex = destStop != null
                            ? option.Stops.FindIndex(x => x.Name.Equals(destStop.StopDesc))
                            : -1;

                        if (destStopIndex < 0 || secondChangeStopIndex >= destStopIndex)
                            continue;

                        var changeOne = RouteMapper.MapChange(trip, startStopIndex, currentStopIndex + startStopIndex, 0);
                        var changeTwo = RouteMapper.MapChange(option, secondChangeStopIndex, destStopIndex, 1);

                        var routeToCheck = routesToReturn.FirstOrDefault(x =>
                        {
                            var first = x.ChangeList.First();
                            var last = x.ChangeList.Last();

                            return first.BusLineName.Equals(changeOne.BusLineName) &&
                                   last.BusLineName.Equals(changeTwo.BusLineName);
                        });

                        if (routeToCheck != null)
                        {
                            var changeOneToCheck =
                                routeToCheck.ChangeList.FirstOrDefault(x =>
                                    x.BusLineName.Equals(changeOne.BusLineName));
                            var changeTwoToCheck =
                                routeToCheck.ChangeList.FirstOrDefault(x =>
                                    x.BusLineName.Equals(changeTwo.BusLineName));

                            if (changeTwoToCheck != null
                                && changeOneToCheck != null
                                && changeOneToCheck.StopChangeList.Count <= changeOne.StopChangeList.Count
                                && changeTwoToCheck.StopChangeList.Count <= changeTwo.StopChangeList.Count)
                            {
                                continue;
                            }
                        }

                        routesToReturn.Remove(routeToCheck);

                        var routeToAdd = new Route { ChangeList = new List<Change>() };
                        routeToAdd.ChangeList.Add(changeOne);
                        routeToAdd.ChangeList.Add(changeTwo);

                        routesToReturn.Add(routeToAdd);
                    }
                }
            }

            return routesToReturn;
        }

        IEnumerable<Trip> GetPossibleChanges(IEnumerable<ChooseBusStopModel> stopListWithConnectedBusLines, Stop stop, IEnumerable<Trip> trips, int destStopId)
        {
            var stopsWithBuses = stopListWithConnectedBusLines.SingleOrDefault(x => x.StopId == stop.StopId);

            if (stopsWithBuses == null)
                return new List<Trip>();

            var busLineNamesOnCurrentStop = stopsWithBuses.BusLineNames.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList();

            var possibleChanges = trips.Where(tripModel => busLineNamesOnCurrentStop.Any(busLineName => busLineName.Equals(tripModel.BusLineName)))
                .Where(tripModel => tripModel.Stops.Any(stopModel => stopModel.StopId == destStopId));

            return possibleChanges;
        }
    }
}