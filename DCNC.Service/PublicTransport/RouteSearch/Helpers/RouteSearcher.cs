using DCNC.Bussiness.PublicTransport.General;
using DCNC.Bussiness.PublicTransport.RouteSearch;
using System.Collections.Generic;

namespace DCNC.Service.PublicTransport.RouteSearch.Helpers
{
    public class RouteSearcher
    {
        public List<Route> GetDirectLines(List<GroupedJoinedModel> groupedJoinedModels, int startStopId, int destStopId, int changeNo)
        {
            var routesToReturn = new List<Route>();

            foreach (var groupedJoinedModel in groupedJoinedModels)
            {
                foreach (var group in groupedJoinedModel.JoinedTripModels)
                {
                    foreach (var joinedTrip in group.JoinedTrips)
                    {
                        var routeToAdd = new Route { ChangeList = new List<Change>() };
                        var startStopIndex = joinedTrip.Stops.FindIndex(x => x.StopId == startStopId);
                        var destStopIndex = joinedTrip.Stops.FindIndex(x => x.StopId == destStopId);

                        if (startStopIndex < 0 || destStopIndex < 0 || startStopIndex >= destStopIndex) continue;

                        var changeToAdd = RouteMapper.MapChange(joinedTrip, startStopIndex, destStopIndex, changeNo);
                        routeToAdd.ChangeList.Add(changeToAdd);
                        routesToReturn.Add(routeToAdd);
                    }
                }
            }

            return routesToReturn;
        }

        public List<Route> GetLinesWithOneChange(List<GroupedJoinedModel> groupedJoinedModels, int startStopId, int destStopId)
        {
            var routesToReturn = new List<Route>();

            foreach (var groupedJoinedModel in groupedJoinedModels)
            {
                foreach (var group in groupedJoinedModel.JoinedTripModels)
                {
                    foreach (var joinedTrip in group.JoinedTrips)
                    {
                        var startStopIndex = joinedTrip.Stops.FindIndex(x => x.StopId == startStopId);

                        if(startStopIndex < 0) continue;


                    }
                }
            }

            return routesToReturn;
        }
    }
}