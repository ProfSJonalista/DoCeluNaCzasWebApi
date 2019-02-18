using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DCNC.Bussiness.PublicTransport.JoiningTrips;
using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Service.PublicTransport.JoiningTrips;
using DCNC.Service.PublicTransport.TimeTable;
using DoCeluNaCzasWebApi.Models.PublicTransport;

namespace DoCeluNaCzasWebApi.Services.PublicTransport
{
    public class JoinTripService
    {
        private readonly TripsWithBusStopsService _tripsWithBusStopsService;

        public JoinTripService()
        {
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

            throw new NotImplementedException();
        }
    }
}
