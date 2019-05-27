using DCNC.Bussiness.PublicTransport.General;
using DCNC.Bussiness.PublicTransport.JsonData.General;
using DCNC.Service.PublicTransport.JoiningTrips;
using System.Collections.Generic;
using System.Linq;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.Joining
{
    public class Joiner
    {
        readonly CombineTripService _combineTripService;
        readonly JoinTripMappingService _joinTripMappingService;
        readonly TripsWithBusStopsService _tripsWithBusStopsService;

        public Joiner(CombineTripService combineTripService, JoinTripMappingService joinTripMappingService, TripsWithBusStopsService tripsWithBusStopsService)
        {
            _combineTripService = combineTripService;
            _joinTripMappingService = joinTripMappingService;
            _tripsWithBusStopsService = tripsWithBusStopsService;
        }

        public List<JoinedTripsModel> GetJoinedTripsModelList(
            List<TripData> tripDataList,
            List<BusStopData> busStopDataList,
            List<BusLineData> busLineDataList,
            List<StopInTripData> stopInTripDataList,
            ExpeditionData expeditionObject)
        {
            var tripsWithBusStops = _tripsWithBusStopsService.GetTripsWithBusStops(tripDataList,
                busStopDataList, busLineDataList, stopInTripDataList, expeditionObject);
            var organizedTrips = _tripsWithBusStopsService.OrganizeTrips(tripsWithBusStops, busLineDataList);
            var joinedDistinctBusLineList = JoinBusLines(busLineDataList);
            var joinedTripList = _combineTripService.JoinTrips(organizedTrips, joinedDistinctBusLineList);
            var mappedJoinedTripModels = _joinTripMappingService.Map(joinedTripList);

            return mappedJoinedTripModels;
        }

        static List<Route> JoinBusLines(List<BusLineData> busLineDataList)
        {
            return busLineDataList.SelectMany(x => x.Routes)
                .Distinct()
                .GroupBy(x => x.RouteShortName)
                .Select(x => x.FirstOrDefault())
                .ToList();
        }
    }
}
