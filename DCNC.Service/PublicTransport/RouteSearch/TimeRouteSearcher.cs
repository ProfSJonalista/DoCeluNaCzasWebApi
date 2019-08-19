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

        public TimeRouteSearcher(RouteCreator routeCreator)
        {
            _routeCreator = routeCreator;
        }

        public async System.Threading.Tasks.Task<List<Route>> GetTimeForRoutesAsync(List<Route> routes, bool departure, DateTime desiredTime)
        {
            var routesWithTime = new List<Route>();

            foreach (var route in routes)
            {
                var changeListCount = route.ChangeList.Count;

                if (changeListCount == 0)
                    continue;

                var routeOneToAdd = new Route { ChangeList = new List<Change>() };
                var routeTwoToAdd = new Route { ChangeList = new List<Change>() };

                if (changeListCount == 1)
                {
                    var changeToLookTimeFor = route.ChangeList.First();

                    await _routeCreator.SetDirect(routeOneToAdd, changeToLookTimeFor, desiredTime, departure, before: true);
                    await _routeCreator.SetDirect(routeTwoToAdd, changeToLookTimeFor, desiredTime, departure, before: false);
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
                        (firstElWithTimeOne, lastElWithTimeOne) = await _routeCreator.GetDepartureTime(firstEl, lastEl, desiredTime, departure: true, firstElBefore: true, lastElBefore: false);
                        (firstElWithTimeTwo, lastElWithTimeTwo) = await _routeCreator.GetDepartureTime(firstEl, lastEl, desiredTime, departure: true, firstElBefore: false, lastElBefore: false);
                    }
                    else
                    {
                        (firstElWithTimeOne, lastElWithTimeOne) = await _routeCreator.GetArrivalTime(firstEl, lastEl, desiredTime, departure: false, firstElBefore: true, lastElBefore: true);
                        (firstElWithTimeTwo, lastElWithTimeTwo) = await _routeCreator.GetArrivalTime(firstEl, lastEl, desiredTime, departure: false, firstElBefore: true, lastElBefore: false);
                    }

                    _routeCreator.CheckTime(routeOneToAdd, firstElWithTimeOne, lastElWithTimeOne);
                    _routeCreator.CheckTime(routeTwoToAdd, firstElWithTimeTwo, lastElWithTimeTwo);
                }

                routeOneToAdd.ChangeList = routeOneToAdd.ChangeList.OrderBy(x => x.ChangeNo).ToList();
                routeTwoToAdd.ChangeList = routeTwoToAdd.ChangeList.OrderBy(x => x.ChangeNo).ToList();

                _routeCreator.CheckTime(routesWithTime, routeOneToAdd);
                _routeCreator.CheckTime(routesWithTime, routeTwoToAdd);
            }

            return routesWithTime;
        }
    }
}
