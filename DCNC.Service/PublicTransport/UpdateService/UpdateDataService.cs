using DCNC.Bussiness.PublicTransport;
using DCNC.Bussiness.PublicTransport.GeneralData;
using DCNC.Bussiness.PublicTransport.JoinedTrips;
using DCNC.Service.PublicTransport.Resources;
using DCNC.Service.PublicTransport.TimeTable;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace DCNC.Service.PublicTransport.UpdateService
{
    public class UpdateDataService
    {
        static Data _data;
        static Timer _timer;
        static TripData _trips;
        static BusLineData _busLines;
        static BusStopData _busStops;
        static StopInTripData _stopsInTrips;
        static ExpeditionData _expeditionData;
        static readonly ObjectCache _cache = MemoryCache.Default;

        
        TripService _tripService;
        BusLineService _busLineService;
        BusStopService _busStopService;
        JoinTripService _joinTripService;
        ExpeditionService _expeditionService;
        StopInTripService _stopInTripService;

        public UpdateDataService()
        {
            _tripService = new TripService();
            _busLineService = new BusLineService();
            _busStopService = new BusStopService();
            _joinTripService = new JoinTripService();
            _expeditionService = new ExpeditionService();
            _stopInTripService = new StopInTripService();

        }

        public async Task Init()
        {
            await DownloadData();
            AllocateData();

            _data = _cache[CacheKeys.GENERAL_DATA_KEY] as Data;

            UpdateJoinedTrips();
        }

        //co określony okres czasu sprawdza czy nie zostały zaktualizowane dane na zewnętrznym API - jeśli tak, aktualizuje je
        public void SetTimer()
        {
            var timeInMilliseconds = 3600000; //1 godzina
            _timer = new Timer(timeInMilliseconds);
            _timer.Elapsed += UpdateDataEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void UpdateDataEvent(Object source, ElapsedEventArgs e)
        {
            if (_data.BusLineData != null && _data.BusStopData != null && _data.ExpeditionData != null
                            && _data.TripData != null && _data.StopInTripData != null)
            {
                if (CheckForUpdates())
                {
                    AllocateData();
                    UpdateJoinedTrips();
                }
            }
        }

        private void UpdateJoinedTrips()
        {
            _data.TripsWithBusStops = _tripService.TripsWithBusStopsMapper(_data.BusLineData, _data.TripData, 
                                                                          _data.StopInTripData, _data.ExpeditionData, _data.BusStopData);
            _data.JoinedTrips = _joinTripService.JoinTrips(_data.BusLineData, _data.TripData, _data.StopInTripData, 
                                                          _data.ExpeditionData, _data.BusStopData, _data.TripsWithBusStops);

            UpdateCachedData(_data, CacheKeys.GENERAL_DATA_KEY);
        }

        public async Task DownloadData()
        {
            _trips = await _tripService.GetData();
            _busLines = await _busLineService.GetBusLineData();
            _busStops = await _busStopService.GetData();
            _stopsInTrips = await _stopInTripService.GetStopInTripData();
            _expeditionData = await _expeditionService.GetExpeditionData();
        }

        public void AllocateData()
        {
            Data dataToCache = new Data()
            {
                TripData = _trips,
                BusLineData = _busLines,
                BusStopData = _busStops,
                StopInTripData = _stopsInTrips,
                ExpeditionData = _expeditionData
            };

            UpdateCachedData(dataToCache, CacheKeys.GENERAL_DATA_KEY);
        }

        private void UpdateCachedData<T>(T data, string cacheKey)
        {
            _cache.Set(cacheKey, data, new CacheItemPolicy());
        }

        public bool CheckForUpdates()
        {
            if (_data.TripData.Day < _trips.Day)
                return true;
            if (_data.BusLineData.Day < _busLines.Day)
                return true;
            if (_data.BusStopData.Day < _busStops.Day)
                return true;
            if (_data.StopInTripData.Day < _stopsInTrips.Day)
                return true;
            if (_data.ExpeditionData.LastUpdate < _expeditionData.LastUpdate)
                return true;

            return false;
        }
        
        public static BusLineData GetBusLines(bool hasData)
        {
            if (hasData)
                 return new BusLineData();

            return _data.BusLineData;
        }

        public static BusStopData GetBusStops(bool hasData)
        {
            if (hasData)
                return new BusStopData();

            return _data.BusStopData;
        }

        public static List<JoinedTripsViewModel> GetJoinedTrips(bool hasData)
        {
            if (hasData)
                return new List<JoinedTripsViewModel>();

            return _data.JoinedTrips;
        }
    }
}
