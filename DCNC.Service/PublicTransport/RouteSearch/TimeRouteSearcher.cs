using DCNC.Bussiness.PublicTransport.RouteSearch;
using DCNC.Service.PublicTransport.RouteSearch.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCNC.Service.PublicTransport.RouteSearch
{
    public class TimeRouteSearcher
    {
        readonly TimeSearcher _timeSearcher;
        public TimeRouteSearcher(TimeSearcher timeSearcher)
        {
            _timeSearcher = timeSearcher;
        }

        public List<Route> GetTimeForRoutes(List<Route> routes, bool departure, DateTime desiredTime)
        {
            var routesWithTime = new List<Route>();

            foreach (var route in routes)
            {
                var routeOneToAdd = new Route { ChangeList = new List<Change>() };
                var routeTwoToAdd = new Route { ChangeList = new List<Change>() };

                var changeListCount = route.ChangeList.Count;

                if (changeListCount == 0) continue;
                if (changeListCount == 1)
                {
                    var changeToLookTimeFor = route.ChangeList.First();
                    var changeOneToAdd = _timeSearcher.GetChangeBefore(changeToLookTimeFor, departure, desiredTime);

                    if (changeOneToAdd.TimeOfTravel.Minutes != 0)
                        routeOneToAdd.ChangeList.Add(changeOneToAdd);
                    
                    var changeTwoToAdd = _timeSearcher.GetChangeAfter(changeToLookTimeFor, departure, desiredTime);

                    if (changeTwoToAdd.TimeOfTravel.Minutes != 0)
                        routeTwoToAdd.ChangeList.Add(changeTwoToAdd);
                }

                if (changeListCount > 1)
                {
                    var firstEl = route.ChangeList.First();
                    var lastEl = route.ChangeList.Last();

                    Change firstElWithTimeOne;
                    Change lastElWithTimeOne;

                    Change firstElWithTimeTwo;
                    Change lastElWithTimeTwo;

                    if (departure)
                    {
                        firstElWithTimeOne = _timeSearcher.GetChangeBefore(firstEl, true, desiredTime);
                        lastElWithTimeOne = _timeSearcher.GetChangeAfter(lastEl, true, firstElWithTimeOne.ArrivalTime);

                        firstElWithTimeTwo = _timeSearcher.GetChangeAfter(firstEl, true, desiredTime);
                        lastElWithTimeTwo = _timeSearcher.GetChangeAfter(lastEl, true, firstElWithTimeTwo.ArrivalTime);
                    }
                    else
                    {
                        lastElWithTimeOne = _timeSearcher.GetChangeBefore(firstEl, false, desiredTime);
                        firstElWithTimeOne = _timeSearcher.GetChangeBefore(lastEl, false, lastElWithTimeOne.DepartureTime);

                        lastElWithTimeTwo = _timeSearcher.GetChangeAfter(firstEl, false, desiredTime);
                        firstElWithTimeTwo = _timeSearcher.GetChangeBefore(lastEl, false, lastElWithTimeOne.DepartureTime);
                    }

                    if (firstElWithTimeOne.TimeOfTravel.Minutes != 0 || lastElWithTimeOne.TimeOfTravel.Minutes != 0)
                    {
                        routeOneToAdd.ChangeList.Add(firstElWithTimeOne);
                        routeOneToAdd.ChangeList.Add(lastElWithTimeOne);
                    }

                    if (firstElWithTimeTwo.TimeOfTravel.Minutes != 0 || lastElWithTimeTwo.TimeOfTravel.Minutes != 0)
                    {
                        routeTwoToAdd.ChangeList.Add(firstElWithTimeTwo);
                        routeTwoToAdd.ChangeList.Add(lastElWithTimeTwo);
                    }
                }

                routeOneToAdd.ChangeList = routeOneToAdd.ChangeList.OrderBy(x => x.ChangeNo).ToList();
                routeTwoToAdd.ChangeList = routeTwoToAdd.ChangeList.OrderBy(x => x.ChangeNo).ToList();

                if (routeOneToAdd.ChangeList.Count > 0)
                {
                    routeOneToAdd = SetTimeInRoute(routeOneToAdd);
                    routesWithTime.Add(routeOneToAdd);
                }

                if (routeTwoToAdd.ChangeList.Count <= 0) continue;

                routeTwoToAdd = SetTimeInRoute(routeTwoToAdd);
                routesWithTime.Add(routeTwoToAdd);
            }

            return routesWithTime;
        }

        static Route SetTimeInRoute(Route route)
        {
            route.DepartureTime = route.ChangeList.First().DepartureTime;
            route.ArrivalTime = route.ChangeList.Last().ArrivalTime;
            route.FullTimeOfTravel = route.ArrivalTime - route.DepartureTime;
            route.Buses = "";

            route.ChangeList.ForEach(x =>
            {
                route.Buses += x.BusLineName + ", ";
            });

            route.Buses = string.IsNullOrEmpty(route.Buses)
                ? ""
                : route.Buses.Remove(route.Buses.Length - 2);

            return route;
        }
    }
}
