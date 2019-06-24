using DCNC.Bussiness.PublicTransport.General;
using DCNC.Bussiness.PublicTransport.RouteSearch;
using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using DCNC.Service.PublicTransport.RouteSearch.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using DCNC.Service.Database.Interfaces;

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

        public List<Route> SearchRoute(int startStopId, int destStopId, bool departure, DateTime desiredTime)
        {
            var tripsWithBusStops = _documentStoreRepository.GetTripsByDayOfWeek(desiredTime.DayOfWeek);
            var routesToReturn = _routeSearcher.GetDirectLines(tripsWithBusStops.Trips, startStopId, destStopId);

            if(routesToReturn.Count <= 0)
                routesToReturn = _routeSearcher.GetLinesWithOneChange(tripsWithBusStops.Trips, startStopId, destStopId);

            routesToReturn = _timeRouteSearcher.GetTimeForRoutes(routesToReturn, departure, desiredTime);
            routesToReturn = routesToReturn.OrderBy(x => x.DepartureTime).ToList();

            return routesToReturn;
        }
    }
}