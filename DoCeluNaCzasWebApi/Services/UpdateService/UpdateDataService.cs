﻿using DCNC.Service.PublicTransport.Caching;
using DCNC.Service.PublicTransport.Caching.Helpers;
using DCNC.Service.PublicTransport.Time;
using DoCeluNaCzasWebApi.Models.PublicTransport.General;
using DoCeluNaCzasWebApi.Services.UpdateService.Helpers;
using System.Collections.Generic;
using System.Timers;
using DCNC.Service.Database;

namespace DoCeluNaCzasWebApi.Services.UpdateService
{
    public static class UpdateDataService
    {
        private static Timer _timer;
        private static TimeService _timeService;
        private static CacheService _cacheService;
        private static UpdateServiceHelper _updateServiceHelper;

        public static async void Init(DocumentStoreRepository dsr)
        {
            _cacheService = new CacheService();
            _timeService = new TimeService(_cacheService);
            _updateServiceHelper = new UpdateServiceHelper(_cacheService, _timeService, dsr);

            var (tripsAsJObject, busStopsAsJObject, busLinesAsJObject, expeditionsAsJObject, stopsInTripsAsJObject) = await _updateServiceHelper.GetDataAsync();
            _updateServiceHelper.SetAndCache(tripsAsJObject, busStopsAsJObject, busLinesAsJObject, expeditionsAsJObject, stopsInTripsAsJObject);

            SetTimer();
        }

        public static void SetTimer()
        {
            const int timeInMilliseconds = 3600000; //1 hour
            _timer = new Timer(timeInMilliseconds);
            _timer.Elapsed += UpdateDataEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private static async void UpdateDataEvent(object source, ElapsedEventArgs e)
        {
            var (tripsAsJObject, busStopsAsJObject, busLinesAsJObject, expeditionsAsJObject, stopsInTripsAsJObject) = await _updateServiceHelper.GetDataAsync();
            var updateNeeded = _timeService.CheckForUpdates(tripsAsJObject, busStopsAsJObject, busLinesAsJObject, expeditionsAsJObject, stopsInTripsAsJObject);

            if (!updateNeeded) return;

            _updateServiceHelper.SetAndCache(tripsAsJObject, busStopsAsJObject, busLinesAsJObject, expeditionsAsJObject, stopsInTripsAsJObject);
        }

        public static List<GroupedJoinedModel> GetJoinedTrips()
        {
            return _cacheService.GetData<List<GroupedJoinedModel>>(CacheKeys.JOINED_TRIP_MODEL_LIST);
        }

        public static BusStopDataModel GetBusStops()
        {
            return _cacheService.GetData<BusStopDataModel>(CacheKeys.JOINED_BUS_STOPS);
        }
    }
}