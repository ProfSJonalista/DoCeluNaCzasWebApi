using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Bussiness.PublicTransport.JsonData.General;
using DCNC.DataAccess.PublicTransport.Helpers;
using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using DCNC.Service.PublicTransport.JsonData.Abstracts.Interfaces;
using DCNC.Service.PublicTransport.Time;
using DoCeluNaCzasWebApi.Services.Delays;
using DoCeluNaCzasWebApi.Services.PublicTransport.Joining;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DCNC.Bussiness.PublicTransport.JoiningTrips;
using DCNC.Service.Database.Interfaces;

// ReSharper disable PossibleNullReferenceException
namespace DoCeluNaCzasWebApi.Services.UpdateService.Helpers
{
    public class UpdateServiceHelper
    {
        private readonly TimeService _timeService;

        readonly Joiner _joiner;
        readonly Grouper _grouper;
        readonly IJsonDataService _tripService;
        readonly IJsonDataService _busStopService;
        readonly IJsonDataService _busLineService;
        readonly IJsonDataService _expeditionService;
        readonly IJsonDataService _stopInTripService;
        readonly BusStopModelService _busStopModelService;
        readonly IDocumentStoreRepository _documentStoreRepository;

        public UpdateServiceHelper(Joiner joiner, Grouper grouper, TimeService timeService, IJsonDataService tripService, 
                                   IJsonDataService busStopService, IJsonDataService busLineService, IJsonDataService expeditionService, 
                                   IJsonDataService stopInTripService, BusStopModelService busStopModelService, IDocumentStoreRepository documentStoreRepository)
        {
            _joiner = joiner;
            _grouper = grouper;
            _timeService = timeService;
            _tripService = tripService;
            _busStopService = busStopService;
            _busLineService = busLineService;
            _expeditionService = expeditionService;
            _stopInTripService = stopInTripService;
            _busStopModelService = busStopModelService;
            _documentStoreRepository = documentStoreRepository;
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

        public void SetAndCache(JObject tripsAsJObject, JObject busStopsAsJObject, JObject busLinesAsJObject, JObject expeditionsAsJObject, JObject stopsInTripsAsJObject)
        {
            var tripDataList = _tripService.GetData<TripData>(tripsAsJObject);
            var busStopDataList = _busStopService.GetData<BusStopData>(busStopsAsJObject);
            var busLineDataList = _busLineService.GetData<BusLineData>(busLinesAsJObject);
            var stopInTripDataList = _stopInTripService.GetData<StopInTripData>(stopsInTripsAsJObject);
            var expeditionData = _expeditionService.GetData<ExpeditionData>(expeditionsAsJObject).FirstOrDefault();

            var busStopDataModel = _busStopModelService.JoinBusStopData(busStopDataList);
            var tripsWithBusStops = _joiner.GetTripsWithBusStopList(tripDataList, busStopDataList, busLineDataList, stopInTripDataList, expeditionData);

            DeleteAndStoreTripsInDb(tripsWithBusStops);

            var joinedTripsModelList = _joiner.GetJoinedTripsModelList(tripsWithBusStops, busLineDataList);
            var groupedJoinedTrips = _grouper.GroupTrips(joinedTripsModelList);

            DelayService.BusLineData = busLineDataList.FirstOrDefault(x => x.Day.Date <= DateTime.Today);
            DelayService.TripData = tripDataList.FirstOrDefault(x => x.Day.Date <= DateTime.Today);
            DelayService.SetChooseBusStopModelCollection(busStopDataModel, groupedJoinedTrips);

            CacheService.CacheData(busStopDataModel, CacheKeys.BUS_STOP_DATA_MODEL);
            CacheService.CacheData(groupedJoinedTrips, CacheKeys.GROUPED_JOINED_MODEL_LIST);

            _timeService.CacheLastUpdates(tripDataList.FirstOrDefault().LastUpdate, 
                                          busStopDataList.FirstOrDefault().LastUpdate,
                                          busStopDataList.FirstOrDefault().LastUpdate,
                                          stopInTripDataList.FirstOrDefault().LastUpdate,
                                          expeditionData.LastUpdate);
        }

        void DeleteAndStoreTripsInDb(List<TripsWithBusStops> tripsWithBusStops)
        {
            _documentStoreRepository.DeleteTripsWithBusStops();
            _documentStoreRepository.Save(tripsWithBusStops);
        }
    }
}