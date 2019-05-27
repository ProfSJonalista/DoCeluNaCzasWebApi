using DCNC.Bussiness.PublicTransport.RouteSearch;
using DCNC.Service.PublicTransport.RouteSearch;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers.PublicTransport.RouteSearch
{
    public class RouteSearchController : ApiController
    {
        readonly RouteSearchService _routeSearchService;

        public RouteSearchController()
        {
            _routeSearchService = new RouteSearchService();
        }

        public List<Route> Get(int startStopId, int destStopId, bool departure, DateTime time)
        {
            return _routeSearchService.SearchRoute(startStopId, destStopId, departure, time);
        }
    }
}
