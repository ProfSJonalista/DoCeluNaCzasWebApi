using DCNC.Bussiness.PublicTransport.General;
using DCNC.Bussiness.PublicTransport.RouteSearch;
using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using DCNC.Service.PublicTransport.RouteSearch.Helpers;

namespace DCNC.Service.PublicTransport.RouteSearch
{
    public class RouteSearchService
    {
        readonly RouteSearcher _routeSearcher;
        public RouteSearchService(RouteSearcher routeSearcher)
        {
            _routeSearcher = routeSearcher;
        }

        public List<Route> SearchRoute(int startStopId, int destStopId, bool departure, DateTime time)
        {
            var groupedJoinedModels = CacheService.GetData<List<GroupedJoinedModel>>(CacheKeys.GROUPED_JOINED_MODEL_LIST);

            var routesToReturn = _routeSearcher.GetDirectLines(groupedJoinedModels, startStopId, destStopId, 0);
            
            if (routesToReturn.Count > 0)
            {
                //dopasować jakoś godziny
                return routesToReturn;
            }

            routesToReturn = _routeSearcher.GetLinesWithOneChange(groupedJoinedModels, startStopId, destStopId);

            return routesToReturn;
        }
    }
}