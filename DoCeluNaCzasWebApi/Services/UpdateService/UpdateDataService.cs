﻿using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using DCNC.Service.PublicTransport.Time;
using DoCeluNaCzasWebApi.Models.PublicTransport.General;
using DoCeluNaCzasWebApi.Services.UpdateService.Helpers;
using System.Collections.Generic;
using System.Timers;

namespace DoCeluNaCzasWebApi.Services.UpdateService
{
    public static class UpdateDataService
    {
        private static Timer _timer;
        private static TimeService _timeService;
        private static UpdateServiceHelper _updateServiceHelper;

        public static async void Init(TimeService timeService, UpdateServiceHelper updateServiceHelper)
        {
            _timeService = timeService;
            _updateServiceHelper = updateServiceHelper;

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
            return CacheService.GetData<List<GroupedJoinedModel>>(CacheKeys.GROUPED_JOINED_TRIPS);
        }

        public static BusStopDataModel GetBusStops()
        {
            return CacheService.GetData<BusStopDataModel>(CacheKeys.BUS_STOP_DATA_MODEL);
        }
    }
}