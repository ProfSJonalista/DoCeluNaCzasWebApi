using System.Collections.Generic;
using System.Linq;
using DCNC.Bussiness.PublicTransport.JoiningTrips;
using DoCeluNaCzasWebApi.Services.PublicTransport.Joining.Helpers;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.Joining
{
    public class Grouper
    {
        public List<GroupedJoinedModel> GroupTrips(List<JoinedTripsModel> joinedTripsModelList)
        {
            var semiNumericComparer = new SemiNumericComparer();

            var buses = joinedTripsModelList.Where(x => x.JoinedTrips.Any(y => y.AgencyId == 1
                                                                               || y.AgencyId == 6
                                                                               || y.AgencyId == 7
                                                                               || y.AgencyId == 8
                                                                               || y.AgencyId == 9
                                                                               || y.AgencyId == 10
                                                                               || y.AgencyId == 11
                                                                               || y.AgencyId == 17
                                                                               || y.AgencyId == 18))
                .OrderBy(z => z.BusLineName, semiNumericComparer)
                .ToList();

            var trams = joinedTripsModelList.Where(x => x.JoinedTrips.Any(y => y.AgencyId == 2))
                .OrderBy(z => z.BusLineName, semiNumericComparer)
                .ToList();
            var trolleys = joinedTripsModelList.Where(x => x.JoinedTrips.Any(y => y.AgencyId == 5))
                .OrderBy(z => z.BusLineName, semiNumericComparer)
                .ToList();

            return new List<GroupedJoinedModel>()
            {
                new GroupedJoinedModel(){ Group = Group.Buses, JoinedTripModels = buses },
                new GroupedJoinedModel(){ Group = Group.Trams, JoinedTripModels = trams },
                new GroupedJoinedModel(){ Group = Group.Trolleys, JoinedTripModels = trolleys}
            };
        }
    }
}