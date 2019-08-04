using DCNC.Bussiness.PublicTransport.General;
using DCNC.Bussiness.PublicTransport.JoiningTrips;
using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using DCNC.Service.PublicTransport.Time;
using DoCeluNaCzasWebApi.Services.UpdateService.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using DCNC.Service.Database.Interfaces;
using Newtonsoft.Json.Linq;

namespace DoCeluNaCzasWebApi.Services.UpdateService
{
    public static class UpdateDataService
    {
        static Timer _timer;
        static TimeService _timeService;
        static UpdateServiceHelper _updateServiceHelper;
        public static IDocumentStoreRepository DocumentStoreRepository;

        public static async Task Init(TimeService timeService, UpdateServiceHelper updateServiceHelper)
        {
            _timeService = timeService;
            _updateServiceHelper = updateServiceHelper;

            var (tripsAsJObject, busStopsAsJObject, busLinesAsJObject, expeditionsAsJObject, stopsInTripsAsJObject)
                = await _updateServiceHelper.GetDataAsync();

            _updateServiceHelper.SetAndCache(tripsAsJObject, busStopsAsJObject, busLinesAsJObject,
                        expeditionsAsJObject, stopsInTripsAsJObject);

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

        static async void UpdateDataEvent(object source, ElapsedEventArgs e)
        {
            var (tripsAsJObject, busStopsAsJObject, busLinesAsJObject, expeditionsAsJObject, stopsInTripsAsJObject) = await _updateServiceHelper.GetDataAsync();
            var updateNeeded = _timeService.CheckForUpdates(tripsAsJObject, busStopsAsJObject, busLinesAsJObject, expeditionsAsJObject, stopsInTripsAsJObject);

            if (!updateNeeded) return;

            _updateServiceHelper.SetAndCache(tripsAsJObject, busStopsAsJObject, busLinesAsJObject, expeditionsAsJObject, stopsInTripsAsJObject);
        }

        public static List<GroupedJoinedModel> GetJoinedTrips()
        {
            return DocumentStoreRepository.GetGroupedJoinedModels();
        }

        public static BusStopDataModel GetBusStops()
        {
            return DocumentStoreRepository.GetBusStopDataModel();
        }
    }
}