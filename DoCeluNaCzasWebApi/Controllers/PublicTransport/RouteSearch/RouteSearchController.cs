using System.Collections.Generic;
using System.Web.Http;
using DCNC.Service.PublicTransport.RouteSearch;

namespace DoCeluNaCzasWebApi.Controllers.PublicTransport.RouteSearch
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
