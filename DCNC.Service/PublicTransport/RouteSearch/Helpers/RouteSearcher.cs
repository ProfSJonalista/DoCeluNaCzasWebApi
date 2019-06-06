using System;
using DCNC.Bussiness.PublicTransport.General;
using DCNC.Bussiness.PublicTransport.RouteSearch;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DCNC.Bussiness.PublicTransport.Delays;
using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;

namespace DCNC.Service.PublicTransport.RouteSearch.Helpers
{
    public class RouteSearcher
    {
        public List<Route> GetDirectLines(List<GroupedJoinedModel> groupedJoinedModels, int startStopId, int destStopId, int changeNo)
        {
            var routesToReturn = new List<Route>();

            foreach (var groupedJoinedModel in groupedJoinedModels)
            {
                foreach (var group in groupedJoinedModel.JoinedTripModels)
                {
                    foreach (var joinedTrip in group.JoinedTrips)
                    {
                        var startStopIndex = joinedTrip.Stops.FindIndex(x => x.StopId == startStopId);
                        var destStopIndex = joinedTrip.Stops.FindIndex(x => x.StopId == destStopId);

                        if (startStopIndex < 0 || destStopIndex < 0 || startStopIndex >= destStopIndex) continue;

                        var routeToAdd = new Route { ChangeList = new List<Change>() };
                        var changeToAdd = RouteMapper.MapChange(joinedTrip, startStopIndex, destStopIndex, changeNo);

                        routeToAdd.ChangeList.Add(changeToAdd);
                        routesToReturn.Add(routeToAdd);
                    }
                }
            }

            return routesToReturn;
        }

        public List<Route> GetLinesWithOneChange(List<GroupedJoinedModel> allGroupedJoinedModels, int startStopId, int destStopId)
        {
            var routesToReturn = new List<Route>();
            var stopListWithConnectedBusLines =
                CacheService.GetData<ObservableCollection<ChooseBusStopModel>>(CacheKeys
                    .CHOOSE_BUS_STOP_MODEL_OBSERVABALE_COLLECTION);

            foreach (var groupedJoinedModel in allGroupedJoinedModels)
            {
                foreach (var group in groupedJoinedModel.JoinedTripModels)
                {
                    foreach (var joinedTrip in group.JoinedTrips)
                    {
                        var startStopIndex = joinedTrip.Stops.FindIndex(x => x.StopId == startStopId);

                        if (startStopIndex < 0) continue;

                        var count = (joinedTrip.Stops.Count - startStopIndex);
                        var stopSubList = joinedTrip.Stops.GetRange(startStopIndex, count);

                        foreach (var joinedStopModel in stopSubList)
                        {
                            var stopsWithBuses =
                                stopListWithConnectedBusLines.SingleOrDefault(x => x.StopId == joinedStopModel.StopId);

                            var busLineNamesOnCurrentStop = stopsWithBuses.BusLineNames.Split(new[] { ", " }, StringSplitOptions.None).ToList();

                            var possibleChanges = allGroupedJoinedModels
                                .SelectMany(joinedModel => joinedModel.JoinedTripModels)
                                .SelectMany(tripsModel => tripsModel.JoinedTrips)
                                .Where(tripsModel => busLineNamesOnCurrentStop
                                    .Any(busLineName => busLineName.Equals(tripsModel.BusLineName)))
                                .Where(tripModel => tripModel.Stops
                                    .Any(stopModel => stopModel.StopId == destStopId)).ToList();

                            var changeStopIndex = stopSubList.FindIndex(x => x.StopId == joinedStopModel.StopId);
                            var stopToChange = stopSubList[changeStopIndex];

                            if (changeStopIndex < 0 || startStopIndex >= changeStopIndex || stopToChange.StopId == destStopId) continue;

                            foreach (var change in possibleChanges)
                            {
                                if (changeStopIndex < 0) continue;

                                var secondChangeStopIndex = change.Stops.FindIndex(x => x.Name.Equals(stopToChange.Name));
                                if (secondChangeStopIndex < 0) continue;

                                var secondStopToChange = change.Stops[secondChangeStopIndex];

                                if (!secondStopToChange.Name.Equals(stopToChange.Name)) continue;

                                var destStop = stopListWithConnectedBusLines.SingleOrDefault(x => x.StopId == destStopId);
                                var destStopIndex = destStop != null
                                                             ? change.Stops.FindIndex(x => x.Name.Equals(destStop.StopDesc))
                                                             : -1;

                                if (destStopIndex < 0 || secondChangeStopIndex >= destStopIndex) continue;

                                var changeOne = RouteMapper.MapChange(joinedTrip, startStopIndex, changeStopIndex + startStopIndex, 0);
                                var changeTwo = RouteMapper.MapChange(change, secondChangeStopIndex, destStopIndex, 1);

                                var routeToCheck = routesToReturn.FirstOrDefault(x =>
                                {
                                    var first = x.ChangeList.FirstOrDefault();
                                    var last = x.ChangeList.LastOrDefault();

                                    return 
                                        first!= null && 
                                        last != null &&
                                        first.BusLineName.Equals(changeOne.BusLineName) &&
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

                                    if (changeTwoToCheck != null && 
                                       (changeOneToCheck != null && 
                                       (changeOneToCheck.StopChangeList.Count <= changeOne.StopChangeList.Count &&
                                        changeTwoToCheck.StopChangeList.Count <= changeTwo.StopChangeList.Count)))
                                    {
                                        continue;
                                    }

                                    routesToReturn.Remove(routeToCheck);
                                }

                                var routeToAdd = new Route() { ChangeList = new List<Change>() };
                                routeToAdd.ChangeList.Add(changeOne);
                                routeToAdd.ChangeList.Add(changeTwo);

                                routesToReturn.Add(routeToAdd);
                            }
                        }
                    }
                }
            }

            return routesToReturn;
        }
    }
}