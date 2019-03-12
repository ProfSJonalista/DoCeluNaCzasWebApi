﻿using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Service.PublicTransport.JoiningTrips;
using DCNC.Service.PublicTransport.JsonData;
using DoCeluNaCzasWebApi.Models.PublicTransport;
using System.Collections.Generic;

namespace DoCeluNaCzasWebApi.Services.PublicTransport
{
    public class Joiner
    {
        private readonly BusLineService _busLineService;
        private readonly CombineTripService _combineTripService;
        private readonly JoinTripMappingService _joinTripMappingService;
        private readonly TripsWithBusStopsService _tripsWithBusStopsService;

        public Joiner()
        {
            _busLineService = new BusLineService();
            _combineTripService = new CombineTripService();
            _joinTripMappingService = new JoinTripMappingService();
            _tripsWithBusStopsService = new TripsWithBusStopsService();
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
            var joinedDistinctBusLineList = _busLineService.JoinBusLines(busLineDataList);
            var joinedTripList = _combineTripService.JoinTrips(organizedTrips, joinedDistinctBusLineList);
            var mappedJoinedTripModels = _joinTripMappingService.Map(joinedTripList);

            return mappedJoinedTripModels;
        }
    }
}