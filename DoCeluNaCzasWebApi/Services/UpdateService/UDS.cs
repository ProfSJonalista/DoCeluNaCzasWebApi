using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.DataAccess.Helpers;
using DCNC.Service.PublicTransport.JsonData;
using DCNC.Service.PublicTransport.UpdateData;
using DoCeluNaCzasWebApi.Models.PublicTransport;
using DoCeluNaCzasWebApi.Services.PublicTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using DCNC.Service.PublicTransport.Caching;
using DCNC.Service.PublicTransport.Caching.Helpers;
using Newtonsoft.Json.Linq;

namespace DoCeluNaCzasWebApi.Services.UpdateService
{
    public static class UDS
    {
        private static TimeService _timeService;
        private static TripService _tripService;
        private static CacheService _cacheService;
        private static BusStopService _busStopService;
        private static BusLineService _busLineService;
        private static JoinTripService _joinTripService;
        private static ExpeditionService _expeditionService;
        private static StopInTripService _stopInTripService;

        private static BusStopModelService _busStopModelService;

        private static Timer _timer;

        private static BusStopDataModel _busStopDataModel;
        private static List<JoinedTripsModel> _joinedTripsModelList;

        public static async Task Init()
        {
            InitializeServices();

            var (tripsAsJObject, 
                busStopsAsJObject, 
                busLinesAsJObject, 
                expeditionsAsJObject, 
                stopsInTripsAsJObject) = await GetDataAsync();

            var tripDataList = _tripService.GetMappedListAndCacheData<TripData>(tripsAsJObject, CacheKeys.TRIP_DATA_LIST_KEY);
            var busStopDataList = _busStopService.GetMappedListAndCacheData<BusStopData>(busStopsAsJObject, CacheKeys.BUS_STOP_DATA_LIST_KEY);
            var busLineDataList = _busLineService.GetMappedListAndCacheData<BusLineData>(busLinesAsJObject, CacheKeys.BUS_LINE_DATA_LIST_KEY);
            var stopInTripDataList = _stopInTripService.GetMappedListAndCacheData<StopInTripData>(stopsInTripsAsJObject, CacheKeys.STOP_IN_TRIP_DATA_LIST_KEY);
            var expeditionData = _expeditionService.GetMappedListAndCacheData<ExpeditionData>(expeditionsAsJObject, CacheKeys.EXPEDITION_DATA_KEY).FirstOrDefault();

            _busStopDataModel = _busStopModelService.JoinBusStopData(busStopDataList);

            _joinedTripsModelList = _joinTripService.GetJoinedTripsModelList(tripDataList, busStopDataList,
                busLineDataList, stopInTripDataList, expeditionData);

            SetTimer();
            /*
             TODO ściągnąć dane (w formie list),
             przemapować dane na Modele (łącząc dane z list w jeden obiekt), 
             ustawić lastUpdates
            */
        }

        private static async Task<(
                JObject tripsAsJObject, 
                JObject busStopsAsJObject, 
                JObject busLinesAsJObject, 
                JObject expeditionsAsJObject, 
                JObject stopsInTripsAsJObject
                )> GetDataAsync()
        {
            var tripsAsJObject = await _tripService.GetDataAsJObjectAsync(Urls.TRIPS);
            var busStopsAsJObject = await _busStopService.GetDataAsJObjectAsync(Urls.BUS_STOPS);
            var busLinesAsJObject = await _busLineService.GetDataAsJObjectAsync(Urls.BUS_LINES);
            var expeditionsAsJObject = await _expeditionService.GetDataAsJObjectAsync(Urls.EXPEDITION);
            var stopsInTripsAsJObject = await _stopInTripService.GetDataAsJObjectAsync(Urls.STOPS_IN_TRIPS);

            return (tripsAsJObject, busStopsAsJObject, busLinesAsJObject, expeditionsAsJObject, stopsInTripsAsJObject);
        }

        private static void InitializeServices()
        {
            _timeService = new TimeService();
            _tripService = new TripService();
            _cacheService = new CacheService();
            _busStopService = new BusStopService();
            _busLineService = new BusLineService();
            _joinTripService = new JoinTripService();
            _expeditionService = new ExpeditionService();
            _stopInTripService = new StopInTripService();
            _busStopModelService = new BusStopModelService();
        }

        public static void SetTimer()
        {
            const int timeInMilliseconds = 3600000; //1 hour
            _timer = new Timer(timeInMilliseconds);
            _timer.Elapsed += UpdateDataEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private static void UpdateDataEvent(object source, ElapsedEventArgs e)
        {
            /*
             TODO ściągnąć dane (w formie list)
             porównać daty lastUpdate
             JEŚLI daty się różnią - 
             przemapować dane na Modele (łącząc dane z list w jeden obiekt), 
             ustawić lastUpdates
             JEŚLI SIĘ NIE RÓŻNIĄ - nie robić nic
            */

        }

        public static List<JoinedTripsModel> GetJoinedTrips()
        {
            return _joinedTripsModelList;
        }

        public static BusStopDataModel GetBusStops()
        {
            return _busStopDataModel;
        }
    }
}