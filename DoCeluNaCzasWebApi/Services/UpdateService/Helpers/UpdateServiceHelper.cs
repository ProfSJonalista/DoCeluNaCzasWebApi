using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Bussiness.PublicTransport.JsonData.General;
using DCNC.DataAccess.PublicTransport.Helpers;
using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using DCNC.Service.PublicTransport.JsonData.Abstracts.Interfaces;
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

        private readonly Joiner _joiner;
        private readonly Grouper _grouper;
        private readonly IJsonDataService _tripService;
        private readonly IJsonDataService _busStopService;
        private readonly IJsonDataService _busLineService;
        private readonly IJsonDataService _expeditionService;
        private readonly IJsonDataService _stopInTripService;
        private readonly BusStopModelService _busStopModelService;
        
        public UpdateServiceHelper(Joiner joiner, Grouper grouper, TimeService timeService, 
                                   IJsonDataService tripService, IJsonDataService busStopService, IJsonDataService busLineService, 
                                   IJsonDataService expeditionService, IJsonDataService stopInTripService, BusStopModelService busStopModelService)
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
            DelayService.SetChooseBusStopModelCollection(busStopDataModel, groupedJoinedTrips);

            CacheService.CacheData(busStopDataModel, CacheKeys.BUS_STOP_DATA_MODEL);
            CacheService.CacheData(groupedJoinedTrips, CacheKeys.GROUPED_JOINED_TRIPS);
            
            _timeService.CacheLastUpdates(tripDataList.FirstOrDefault().LastUpdate,
                                        busStopDataList.FirstOrDefault().LastUpdate,
                                        busStopDataList.FirstOrDefault().LastUpdate,
                                      stopInTripDataList.FirstOrDefault().LastUpdate,
                                                    expeditionData.LastUpdate);
        }
    }
}