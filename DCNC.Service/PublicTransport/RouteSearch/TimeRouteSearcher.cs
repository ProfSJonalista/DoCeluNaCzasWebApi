using DCNC.Bussiness.PublicTransport.RouteSearch;
using DCNC.Service.PublicTransport.RouteSearch.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCNC.Service.PublicTransport.RouteSearch
{
    public class TimeRouteSearcher
    {
        readonly RouteCreator _routeCreator;
        readonly TimeSearcher _timeSearcher;

        public TimeRouteSearcher(TimeSearcher timeSearcher, RouteCreator routeCreator)
        {
            _routeCreator = routeCreator;
            _timeSearcher = timeSearcher;
        }

        public List<Route> GetTimeForRoutes(List<Route> routes, bool departure, DateTime desiredTime)
        {
            var routesWithTime = new List<Route>();

            foreach (var route in routes)
            {
                var changeListCount = route.ChangeList.Count;

                if (changeListCount == 0)
                    continue;

                Route routeOneToAdd = new Route { ChangeList = new List<Change>() };
                Route routeTwoToAdd = new Route { ChangeList = new List<Change>() };

                if (changeListCount == 1)
                {
                    //routeOneToAdd = _routeCreator.CreateRoute

                    var changeToLookTimeFor = route.ChangeList.First();
                    var changeOneToAdd = _timeSearcher.GetChangeWithTime(changeToLookTimeFor, departure, desiredTime, true);

                    if (changeOneToAdd.TimeOfTravel.Minutes != 0)
                        routeOneToAdd.ChangeList.Add(changeOneToAdd);

                    var changeTwoToAdd = _timeSearcher.GetChangeWithTime(changeToLookTimeFor, departure, desiredTime, false);

                    if (changeTwoToAdd.TimeOfTravel.Minutes != 0)
                        routeTwoToAdd.ChangeList.Add(changeTwoToAdd);
                }

                #region Many routes

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
                        //option 1
                        firstElWithTimeOne = _timeSearcher.GetChangeWithTime(firstEl, true, desiredTime, true);

                        var dateTime = firstElWithTimeOne.ArrivalTime.AddMinutes(1);
                        lastElWithTimeOne = _timeSearcher.GetChangeWithTime(lastEl, true, dateTime, false);

                        //option 2
                        firstElWithTimeTwo = _timeSearcher.GetChangeWithTime(firstEl, true, desiredTime, false);

                        var dateTime2 = firstElWithTimeTwo.ArrivalTime.AddMinutes(1);
                        lastElWithTimeTwo = _timeSearcher.GetChangeWithTime(lastEl, true, dateTime2, false);
                    }
                    else
                    {
                        //option 1
                        lastElWithTimeOne = _timeSearcher.GetChangeWithTime(lastEl, false, desiredTime, true);

                        if (lastElWithTimeOne.DepartureTime.Year < 1899)
                            firstElWithTimeOne = new Change();
                        else
                        {
                            var dateTime = lastElWithTimeOne.DepartureTime.Subtract(new TimeSpan(0, 1, 0));
                            firstElWithTimeOne = _timeSearcher.GetChangeWithTime(firstEl, false, dateTime, true);
                        }

                        //option 2
                        lastElWithTimeTwo = _timeSearcher.GetChangeWithTime(lastEl, false, desiredTime, false);

                        if (lastElWithTimeTwo.DepartureTime.Year < 1899)
                            firstElWithTimeTwo = new Change();
                        else
                        {
                            var dateTime2 = lastElWithTimeTwo.DepartureTime.Subtract(new TimeSpan(0, 1, 0));
                            firstElWithTimeTwo = _timeSearcher.GetChangeWithTime(firstEl, false, dateTime2, true);
                        }
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

                #endregion

                routeOneToAdd.ChangeList = routeOneToAdd.ChangeList.OrderBy(x => x.ChangeNo).ToList();
                routeTwoToAdd.ChangeList = routeTwoToAdd.ChangeList.OrderBy(x => x.ChangeNo).ToList();

                if (routeOneToAdd.ChangeList.Count > 0)
                {
                    routeOneToAdd = SetTimeInRoute(routeOneToAdd);

                    if (routeOneToAdd.FullTimeOfTravel.Hours < 5)
                        routesWithTime.Add(routeOneToAdd);
                }

                if (routeTwoToAdd.ChangeList.Count <= 0) continue;

                routeTwoToAdd = SetTimeInRoute(routeTwoToAdd);

                if (routeTwoToAdd.FullTimeOfTravel.Hours < 5)
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

            route.ChangeList.ForEach(x => { route.Buses += x.BusLineName + ", "; });

            route.Buses = !string.IsNullOrEmpty(route.Buses)
                ? route.Buses.Remove(route.Buses.Length - 2)
                : "";

            return route;
        }
    }
}
