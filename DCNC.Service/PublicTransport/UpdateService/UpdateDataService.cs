using DCNC.Bussiness.PublicTransport;
using DCNC.Bussiness.PublicTransport.GeneralData;
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
    public static class UpdateDataService
    {
        static Data _data;
        static Timer _timer;
        static TripData _trips;
        static BusLineData _busLines;
        static BusStopData _busStops;
        static StopInTripData _stopsInTrips;
        static ExpeditionData _expeditionData;
        static readonly ObjectCache _cache = MemoryCache.Default;

        public async static Task Init()
        {
            await DownloadData();
            AllocateData();

            _data = _cache[CacheKeys.GENERAL_DATA_KEY] as Data;

            UpdateJoinedTrips();
        }

        //co określony okres czasu sprawdza czy nie zostały zaktualizowane dane na zewnętrznym API - jeśli tak, aktualizuje je
        public static void SetTimer()
        {
            var timeInMilliseconds = 3600000; //1 godzina
            _timer = new Timer(timeInMilliseconds);
            _timer.Elapsed += UpdateDataEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private static void UpdateDataEvent(Object source, ElapsedEventArgs e)
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

        private static void UpdateJoinedTrips()
        {
            _data.TripsWithBusStops = TripService.TripsWithBusStopsMapper(_data.BusLineData, _data.TripData, _data.StopInTripData, _data.ExpeditionData, _data.BusStopData);
            _data.JoinedTrips = JoinTripService.JoinTrips(_data.BusLineData, _data.TripData, _data.StopInTripData, _data.ExpeditionData, _data.BusStopData, _data.TripsWithBusStops);
            
            _data.JoinedTripsAsJson = JsonConvert.SerializeObject(_data.JoinedTrips);
            _data.BusLinesAsJson = JsonConvert.SerializeObject(_data.BusLineData);
            _data.BusStopsAsJson = JsonConvert.SerializeObject(_data.BusStopData);

            UpdateCachedData(_data, CacheKeys.GENERAL_DATA_KEY);
        }

        public static async Task DownloadData()
        {
            _trips = await TripService.GetTripData();
            _busLines = await BusLineService.GetBusLineData();
            _busStops = await BusStopService.GetBusStopData();
            _stopsInTrips = await StopInTripService.GetStopInTripData();
            _expeditionData = await ExpeditionService.GetExpeditionData();
        }

        public static void AllocateData()
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

        private static void UpdateCachedData<T>(T data, string cacheKey)
        {
            _cache.Set(cacheKey, data, new CacheItemPolicy());
        }

        public static bool CheckForUpdates()
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
        
        public static string GetBusLines(bool hasData)
        {
            if (hasData)
                 return "";

            return _data.BusLinesAsJson;
        }

        public static string GetBusStops(bool hasData)
        {
            if (hasData)
                return "";

            return _data.BusStopsAsJson;
        }

        public static string GetJoinedTrips(bool hasData)
        {
            if (hasData)
                return "";

            return _data.JoinedTripsAsJson;
        }
    }
}
