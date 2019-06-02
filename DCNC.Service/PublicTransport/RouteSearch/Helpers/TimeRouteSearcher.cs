using DCNC.Bussiness.PublicTransport.RouteSearch;
using DCNC.Service.Database.Interfaces;
using System;
using System.Collections.Generic;

namespace DCNC.Service.PublicTransport.RouteSearch.Helpers
{
    public class TimeRouteSearcher
    {
        readonly IDocumentStoreRepository _documentStoreRepository;

        public TimeRouteSearcher(IDocumentStoreRepository documentStoreRepository)
        {
            _documentStoreRepository = documentStoreRepository;
        }

        public List<Route> GetTimeForRoutes(List<Route> routes, bool departure, DateTime desiredTime)
        {
            var routesWithTime = new List<Route>();

            foreach (var route in routes)
            {
                var routeToAdd = new Route { ChangeList = new List<Change>() };

                foreach (var change in route.ChangeList)
                {
                    var timeTableData = _documentStoreRepository.GetTimeTableDataByRouteIdAndDayOfWeek(change.RouteId, desiredTime.DayOfWeek);

                    if (departure)
                    {

                    }
                }
            }

            return routesWithTime;
        }
    }
}