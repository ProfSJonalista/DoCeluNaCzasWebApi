using DoCeluNaCzasWebApi.Models.PublicTransport.General;
using System.Collections.Generic;
using System.Linq;

namespace DoCeluNaCzasWebApi.Services.PublicTransport
{
    public class Grouper
    {
        public List<GroupedJoinedModel> Group(List<JoinedTripsModel> joinedTripsModelList)
        {
            var buses = joinedTripsModelList.Where(x => x.JoinedTrips.Any(y => y.AgencyId == 1
                                                                               || y.AgencyId == 6
                                                                               || y.AgencyId == 7
                                                                               || y.AgencyId == 8
                                                                               || y.AgencyId == 9
                                                                               || y.AgencyId == 10
                                                                               || y.AgencyId == 11
                                                                               || y.AgencyId == 17
                                                                               || y.AgencyId == 18))
                .OrderBy(z => z.BusLineName).ToList();
            var trams = joinedTripsModelList.Where(x => x.JoinedTrips.Any(y => y.AgencyId == 2)).OrderBy(z => z.BusLineName).ToList();
            var trolleys = joinedTripsModelList.Where(x => x.JoinedTrips.Any(y => y.AgencyId == 5)).OrderBy(z => z.BusLineName).ToList();

            return new List<GroupedJoinedModel>()
            {
                new GroupedJoinedModel(){ Group = Models.PublicTransport.General.Group.Buses, JoinedTripModels = buses },
                new GroupedJoinedModel(){ Group = Models.PublicTransport.General.Group.Trams, JoinedTripModels = trams },
                new GroupedJoinedModel(){ Group = Models.PublicTransport.General.Group.Trolleys, JoinedTripModels = trolleys}
            };
        }
    }
}