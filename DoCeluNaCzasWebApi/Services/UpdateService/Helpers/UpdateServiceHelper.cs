using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Bussiness.PublicTransport.JsonData.General;
using DCNC.DataAccess.PublicTransport.Helpers;
using DCNC.Service.Database;
using DCNC.Service.PublicTransport.Caching;
using DCNC.Service.PublicTransport.Caching.Helpers;
using DCNC.Service.PublicTransport.JsonData.Abstracts.Interfaces;
using DCNC.Service.PublicTransport.JsonData.General;
using DCNC.Service.PublicTransport.Time;
using DoCeluNaCzasWebApi.Services.Delays;
using DoCeluNaCzasWebApi.Services.PublicTransport;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DoCeluNaCzasWebApi.Services.UpdateService.Helpers
{
    public class UpdateServiceHelper
    {
        private readonly TimeService _timeService;
        private readonly CacheService _cacheService;

        private readonly Joiner _joiner;
        private readonly Grouper _grouper;
        private readonly IJsonDataService _tripService;
        private readonly IJsonDataService _busStopService;
        private readonly IJsonDataService _busLineService;
        private readonly IJsonDataService _expeditionService;
        private readonly IJsonDataService _stopInTripService;
        private readonly BusStopModelService _busStopModelService;
        
        public UpdateServiceHelper(CacheService cacheService, TimeService timeService, IDocumentStoreRepository dsr)
        {
            _joiner = new Joiner();
            _grouper = new Grouper();
            _timeService = timeService;
            _cacheService = cacheService;
            _tripService = new TripService(dsr);
            _busStopService = new BusStopService(dsr);
            _busLineService = new BusLineService(dsr);
            _expeditionService = new ExpeditionService(dsr);
            _stopInTripService = new StopInTripService(dsr);
            _busStopModelService = new BusStopModelService();
        }

        public async Task<(JObject tripsAsJObject, JObject busStopsAsJObject, JObject busLinesAsJObject, JObject expeditionsAsJObject, JObject stopsInTripsAsJObject)> GetDataAsync()
        {
            var tripsAsJObject = await _tripService.GetDataAsJObjectAsync(Urls.Trips, JsonType.Trip);
            var busStopsAsJObject = await _busStopService.GetDataAsJObjectAsync(Urls.Stops, JsonType.BusStop);
            var busLinesAsJObject = await _busLineService.GetDataAsJObjectAsync(Urls.Lines, JsonType.BusLine);
            var expeditionsAsJObject = await _expeditionService.GetDataAsJObjectAsync(Urls.Expedition, JsonType.Expedition);
            var stopsInTripsAsJObject = await _stopInTripService.GetDataAsJObjectAsync(Urls.StopsInTrips, JsonType.StopInTrip);

            return (tripsAsJObject, busStopsAsJObject, busLinesAsJObject, expeditionsAsJObject, stopsInTripsAsJObject);
        }

        // ReSharper disable PossibleNullReferenceException
        public void SetAndCache(JObject tripsAsJObject, JObject busStopsAsJObject, JObject busLinesAsJObject, JObject expeditionsAsJObject, JObject stopsInTripsAsJObject)
        {
            var tripDataList = _tripService.GetData<TripData>(tripsAsJObject);
            var busStopDataList = _busStopService.GetData<BusStopData>(busStopsAsJObject);
            var busLineDataList = _busLineService.GetData<BusLineData>(busLinesAsJObject);
            var stopInTripDataList = _stopInTripService.GetData<StopInTripData>(stopsInTripsAsJObject);
            var expeditionData = _expeditionService.GetData<ExpeditionData>(expeditionsAsJObject).FirstOrDefault();

            var busStopDataModel = _busStopModelService.JoinBusStopData(busStopDataList);
            var joinedTripsModelList = _joiner.GetJoinedTripsModelList(tripDataList, busStopDataList, busLineDataList, stopInTripDataList, expeditionData);
            var groupedJoinedTrips = _grouper.Group(joinedTripsModelList);

            DelayService.BusLineData = busLineDataList.FirstOrDefault(x => x.Day.Date == DateTime.Today);
            DelayService.TripData = tripDataList.FirstOrDefault(x => x.Day.Date == DateTime.Today);

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