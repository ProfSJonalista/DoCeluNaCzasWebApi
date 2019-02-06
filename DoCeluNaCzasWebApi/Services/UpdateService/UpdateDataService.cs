using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Timers;
using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Service.PublicTransport;
using DCNC.Service.PublicTransport.JsonData;
using DCNC.Service.PublicTransport.Resources;
using DCNC.Service.PublicTransport.TimeTable;
using DoCeluNaCzasWebApi.Models.PublicTransport;

namespace DoCeluNaCzasWebApi.Services.UpdateService
{
    public static class UpdateDataService
    {
        static GeneralData _generalData;
        static Timer _timer;
        static TripData _trips;
        static BusLineData _busLines;
        static BusStopData _busStops;
        static StopInTripData _stopsInTrips;
        static ExpeditionData _expeditionData;
        static readonly ObjectCache _cache = MemoryCache.Default;

        static TripService _tripService;
        static BusLineService _busLineService;
        static BusStopService _busStopService;
        static JoinTripService _joinTripService;
        static ExpeditionService _expeditionService;
        static StopInTripService _stopInTripService;

        public static async Task Init()
        {
            _tripService = new TripService();
            _busLineService = new BusLineService();
            _busStopService = new BusStopService();
            _joinTripService = new JoinTripService();
            _expeditionService = new ExpeditionService();
            _stopInTripService = new StopInTripService();

            await DownloadData();
            AllocateData();

            _generalData = _cache[CacheKeys.GENERAL_DATA_KEY] as GeneralData;

            UpdateJoinedTrips();
        }

        //co określony okres czasu sprawdza czy nie zostały zaktualizowane dane na zewnętrznym API - jeśli tak, aktualizuje je
        public static void SetTimer()
        {
            const int timeInMilliseconds = 3600000; //1 godzina
            _timer = new Timer(timeInMilliseconds);
            _timer.Elapsed += UpdateDataEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private static void UpdateDataEvent(object source, ElapsedEventArgs e)
        {
            if (_generalData.BusLineData == null || _generalData.BusStopData == null || _generalData.ExpeditionData == null ||
                _generalData.TripData == null || _generalData.StopInTripData == null) return;
            if (!CheckForUpdates()) return;
            AllocateData();
            UpdateJoinedTrips();
        }

        private static void UpdateJoinedTrips()
        {
            _generalData.TripsWithBusStops = _tripService.TripsWithBusStopsMapper(_generalData.BusLineData, _generalData.TripData, 
                                                                          _generalData.StopInTripData, _generalData.ExpeditionData, _generalData.BusStopData);
            //_generalData.JoinedTrips = _joinTripService.JoinTrips(_generalData.BusLineData, _generalData.TripData, _generalData.StopInTripData, 
            //                                              _generalData.ExpeditionData, _generalData.BusStopData, _generalData.TripsWithBusStops);

            UpdateCachedData(_generalData, CacheKeys.GENERAL_DATA_KEY);
        }

        public static async Task DownloadData()
        {
            //_trips = await _tripService.GetData();
            //_busLines = await _busLineService.GetBusLineData();
            //_busStops = await _busStopService.GetData();
            //_stopsInTrips = await _stopInTripService.GetStopInTripData();
            //_expeditionData = await _expeditionService.GetExpeditionData();
        }

        public static void AllocateData()
        {
            var dataToCache = new GeneralData()
            {
                TripData = _trips,
                BusLineData = _busLines,
                BusStopData = _busStops,
                StopInTripData = _stopsInTrips,
                ExpeditionData = _expeditionData
            };

            UpdateCachedData(dataToCache, CacheKeys.GENERAL_DATA_KEY);
        }

        private static void UpdateCachedData<T>(T data, string cacheKey)
        {
            _cache.Set(cacheKey, data, new CacheItemPolicy());
        }

        public static bool CheckForUpdates()
        {
            if (_generalData.TripData.Day < _trips.Day)
                return true;
            if (_generalData.BusLineData.Day < _busLines.Day)
                return true;
            if (_generalData.BusStopData.Day < _busStops.Day)
                return true;
            if (_generalData.StopInTripData.Day < _stopsInTrips.Day)
                return true;
            if (_generalData.ExpeditionData.LastUpdate < _expeditionData.LastUpdate)
                return true;

            return false;
        }
        
        public static BusLineData GetBusLines()
        {
            return _generalData.BusLineData;
        }

        public static BusStopData GetBusStops()
        {
            return _generalData.BusStopData;
        }

        public static List<JoinedTripsModel> GetJoinedTrips()
        {
            //return _generalData.JoinedTrips;
            return new List<JoinedTripsModel>();
        }
    }
}
