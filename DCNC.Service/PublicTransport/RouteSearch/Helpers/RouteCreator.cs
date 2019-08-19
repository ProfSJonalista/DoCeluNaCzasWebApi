using DCNC.Bussiness.PublicTransport.RouteSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCNC.Service.PublicTransport.RouteSearch.Helpers
{
    public class RouteCreator
    {
        readonly TimeSearcher _timeSearcher;

        public RouteCreator(TimeSearcher timeSearcher)
        {
            _timeSearcher = timeSearcher;
        }

        public async Task<(Change, Change)> GetDepartureTime(Change firstEl, Change lastEl, DateTime desiredTime, bool departure, bool firstElBefore, bool lastElBefore)
        {
            var firstElWithTime = await _timeSearcher.GetChangeWithTime(firstEl, desiredTime, departure, firstElBefore);

            var arrivalTime = firstElWithTime.LastStop.ArrivalTime.AddMinutes(1);
            var lastElWithTime = await _timeSearcher.GetChangeWithTime(lastEl, arrivalTime, departure, lastElBefore);

            return (firstElWithTime, lastElWithTime);
        }

        public async Task<(Change, Change)> GetArrivalTime(Change firstEl, Change lastEl, DateTime desiredTime, bool departure, bool firstElBefore, bool lastElBefore)
        {
            Change firstElWithTime;
            var lastElWithTime = await _timeSearcher.GetChangeWithTime(lastEl, desiredTime, departure, lastElBefore);

            if (lastElWithTime.FirstStop.DepartureTime.Year < 1899)
                firstElWithTime = new Change();
            else
            {
                var substractedDepTime = lastElWithTime.FirstStop.DepartureTime.Subtract(new TimeSpan(0, 1, 0));
                firstElWithTime = await _timeSearcher.GetChangeWithTime(firstEl, substractedDepTime, departure, firstElBefore);
            }

            return (firstElWithTime, lastElWithTime);
        }

        public async Task SetDirect(Route route, Change changeToLookTimeFor, DateTime desiredTime, bool departure, bool before)
        {
            var changeToAdd = await _timeSearcher.GetChangeWithTime(changeToLookTimeFor, desiredTime, departure, before);

            if (changeToAdd.TimeOfTravel.Minutes != 0)
                route.ChangeList.Add(changeToAdd);
        }

        public void CheckTime(Route route, Change firstElWithTime, Change lastElWithTime)
        {
            if (firstElWithTime.TimeOfTravel.Minutes <= 0 && lastElWithTime.TimeOfTravel.Minutes <= 0) return;

            route.ChangeList.Add(firstElWithTime);
            route.ChangeList.Add(lastElWithTime);
        }

        internal void CheckTime(List<Route> routesWithTime, Route route)
        {
            if (route.ChangeList.Count <= 0) return;

            SetTime(route);

            if (route.FullTimeOfTravel.Hours < 5 && route.FullTimeOfTravel > TimeSpan.Zero)
                routesWithTime.Add(route);
        }

        public void SetTime(Route route)
        {
            route.FirstStop = route.ChangeList.First().FirstStop;
            route.LastStop = route.ChangeList.Last().LastStop;
            route.FullTimeOfTravel = route.LastStop.ArrivalTime - route.FirstStop.DepartureTime;
            route.Buses = "";

            route.ChangeList.ForEach(x => { route.Buses += x.BusLineName + ", "; });

            route.Buses = !string.IsNullOrEmpty(route.Buses)
                ? route.Buses.Remove(route.Buses.Length - 2)
                : "";
        }
    }
}