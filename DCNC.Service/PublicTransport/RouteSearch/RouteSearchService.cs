using DCNC.Bussiness.PublicTransport.RouteSearch;
using DCNC.Service.Database.Interfaces;
using DCNC.Service.PublicTransport.RouteSearch.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCNC.Service.PublicTransport.RouteSearch
{
    public class RouteSearchService
    {
        readonly RouteSearcher _routeSearcher;
        readonly TimeRouteSearcher _timeRouteSearcher;
        readonly IDocumentStoreRepository _documentStoreRepository;

        public RouteSearchService(RouteSearcher routeSearcher, TimeRouteSearcher timeRouteSearcher, IDocumentStoreRepository documentStoreRepository)
        {
            _routeSearcher = routeSearcher;
            _timeRouteSearcher = timeRouteSearcher;
            _documentStoreRepository = documentStoreRepository;
        }

        public async Task<List<Route>> SearchRoute(int startStopId, int destStopId, bool departure, DateTime desiredTime)
        {
            var tripsWithBusStops = _documentStoreRepository.GetTripsByDayOfWeek(desiredTime.DayOfWeek);
            var routesToReturn = _routeSearcher.GetDirectLines(tripsWithBusStops.Trips, startStopId, destStopId);

            if (routesToReturn.Count <= 0)
                routesToReturn = _routeSearcher.GetRoutes(tripsWithBusStops.Trips, startStopId, destStopId);

            routesToReturn = await _timeRouteSearcher.GetTimeForRoutesAsync(routesToReturn, departure, desiredTime);

            routesToReturn = routesToReturn.OrderBy(x => x.FirstStop.DepartureTime).ToList();

            //routesToReturn = (
            //    from o in routesToReturn
            //    orderby o.DepartureTime, o.Buses
            //    group o by o.DepartureTime
            //    into g
            //    select g.First()
            //).ToList();

            return routesToReturn;
        }
    }
}