using DCNC.Bussiness.PublicTransport.General;
using DCNC.Bussiness.PublicTransport.RouteSearch;
using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using DCNC.Service.PublicTransport.RouteSearch.Helpers;
using System;
using System.Collections.Generic;

namespace DCNC.Service.PublicTransport.RouteSearch
{
    public class RouteSearchService
    {
        readonly RouteSearcher _routeSearcher;
        readonly TimeRouteSearcher _timeRouteSearcher;

        public RouteSearchService(RouteSearcher routeSearcher, TimeRouteSearcher timeRouteSearcher)
        {
            _routeSearcher = routeSearcher;
            _timeRouteSearcher = timeRouteSearcher;
        }

        public List<Route> SearchRoute(int startStopId, int destStopId, bool departure, DateTime desiredTime)
        {
            var groupedJoinedModels = CacheService.GetData<List<GroupedJoinedModel>>(CacheKeys.GROUPED_JOINED_MODEL_LIST);

            var routesToReturn = _routeSearcher.GetDirectLines(groupedJoinedModels, startStopId, destStopId, 0);
            
            routesToReturn.AddRange(_routeSearcher.GetLinesWithOneChange(groupedJoinedModels, startStopId, destStopId));
            routesToReturn = _timeRouteSearcher.GetTimeForRoutes(routesToReturn, departure, desiredTime);

            return routesToReturn;
        }
    }
}