using DCNC.Service.PublicTransport.RouteSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers
{
    public class RouteSearchController : ApiController
    {
        RouteSearchService _routeSearchService;
        public RouteSearchController()
        {
            _routeSearchService = new RouteSearchService();
        }
        public List<string> Get(int startStopId, int destinationStopId)
        {
            //return _routeSearchService.SearchRoute(startStopId, destinationStopId);
            return new List<string>();
        }
    }
}
