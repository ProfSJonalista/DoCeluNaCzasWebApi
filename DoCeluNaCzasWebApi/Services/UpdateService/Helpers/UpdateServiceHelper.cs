﻿using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Service.PublicTransport.Caching;
using DCNC.Service.PublicTransport.Caching.Helpers;
using DCNC.Service.PublicTransport.JsonData;
using DCNC.Service.PublicTransport.UpdateData;
using DoCeluNaCzasWebApi.Services.PublicTransport;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using DCNC.DataAccess.PublicTransport.Helpers;

namespace DoCeluNaCzasWebApi.Services.UpdateService.Helpers
{
    public class UpdateServiceHelper
    {
        private readonly TimeService _timeService;
        private readonly CacheService _cacheService;

        private readonly Joiner _joiner;
        private readonly Grouper _grouper;
        private readonly TripService _tripService;
        private readonly BusStopService _busStopService;
        private readonly BusLineService _busLineService;
        private readonly ExpeditionService _expeditionService;
        private readonly StopInTripService _stopInTripService;
        private readonly BusStopModelService _busStopModelService;

        public UpdateServiceHelper(CacheService cacheService, TimeService timeService)
        {
            _joiner = new Joiner();
            _grouper = new Grouper();
            _timeService = timeService;
            _cacheService = cacheService;
            _tripService = new TripService();
            _busStopService = new BusStopService();
            _busLineService = new BusLineService();
            _expeditionService = new ExpeditionService();
            _stopInTripService = new StopInTripService();
            _busStopModelService = new BusStopModelService();
        }

        public async Task<(JObject tripsAsJObject, JObject busStopsAsJObject, JObject busLinesAsJObject,
            JObject expeditionsAsJObject, JObject stopsInTripsAsJObject)> GetDataAsync()
        {
            var tripsAsJObject = await _tripService.GetDataAsJObjectAsync(Urls.TRIPS);
            var busStopsAsJObject = await _busStopService.GetDataAsJObjectAsync(Urls.BUS_STOPS);
            var busLinesAsJObject = await _busLineService.GetDataAsJObjectAsync(Urls.BUS_LINES);
            var expeditionsAsJObject = await _expeditionService.GetDataAsJObjectAsync(Urls.EXPEDITION);
            var stopsInTripsAsJObject = await _stopInTripService.GetDataAsJObjectAsync(Urls.STOPS_IN_TRIPS);

            return (tripsAsJObject, busStopsAsJObject, busLinesAsJObject, expeditionsAsJObject, stopsInTripsAsJObject);
        }

        public void SetAndCache(JObject tripsAsJObject, JObject busStopsAsJObject, JObject busLinesAsJObject, JObject expeditionsAsJObject, JObject stopsInTripsAsJObject)
        {
            var tripDataList = _tripService.GetList<TripData>(tripsAsJObject);
            var busStopDataList = _busStopService.GetList<BusStopData>(busStopsAsJObject);
            var busLineDataList = _busLineService.GetList<BusLineData>(busLinesAsJObject);
            var stopInTripDataList = _stopInTripService.GetList<StopInTripData>(stopsInTripsAsJObject);
            var expeditionData = _expeditionService.GetList<ExpeditionData>(expeditionsAsJObject).FirstOrDefault();

            var busStopDataModel = _busStopModelService.JoinBusStopData(busStopDataList);
            var joinedTripsModelList = _joiner.GetJoinedTripsModelList(tripDataList, busStopDataList, busLineDataList, stopInTripDataList, expeditionData);
            var groupedJoinedTrips = _grouper.Group(joinedTripsModelList);
            
            _cacheService.CacheData(busStopDataModel, CacheKeys.JOINED_BUS_STOPS);

            _cacheService.CacheData(groupedJoinedTrips, CacheKeys.JOINED_TRIP_MODEL_LIST);

            _timeService.CacheLastUpdates(tripDataList.FirstOrDefault().LastUpdate,
                                        busStopDataList.FirstOrDefault().LastUpdate,
                                        busStopDataList.FirstOrDefault().LastUpdate,
                                      stopInTripDataList.FirstOrDefault().LastUpdate,
                                                    expeditionData.LastUpdate);
        }
    }
}