using DCNC.Bussiness.PublicTransport.RouteSearch;
using DCNC.Service.Database;
using DCNC.Service.Database.Interfaces;
using DCNC.Service.PublicTransport.RouteSearch;
using DCNC.Service.PublicTransport.RouteSearch.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers.PublicTransport.RouteSearch
{
    public class RouteSearchController : ApiController
    {
        readonly RouteSearchService _routeSearchService;

        public RouteSearchController()
        {
            IDocumentStoreRepository dsr = new DocumentStoreRepository();
            var ts = new TimeSearcher(dsr);
            _routeSearchService = new RouteSearchService(new RouteSearcher(dsr), new TimeRouteSearcher(new RouteCreator(ts)), dsr);
        }

        public async Task<List<Route>> Get(int startStopId, int destStopId, bool departure, DateTime desiredTime)
        {
            return await _routeSearchService.SearchRoute(startStopId, destStopId, departure, desiredTime);
        }
    }
}
