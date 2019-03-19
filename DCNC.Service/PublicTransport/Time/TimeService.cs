using System;
using System.Collections.Generic;
using System.Linq;
using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using DCNC.Service.PublicTransport.Caching;
using DCNC.Service.PublicTransport.Caching.Helpers;
using Newtonsoft.Json.Linq;

namespace DCNC.Service.PublicTransport.Time
{
    public class TimeService
    {
        private readonly CacheService _cacheService;

        public TimeService(CacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public void CacheLastUpdates(DateTime tripDataLu, DateTime busStopDataLu, DateTime busLineDataLu, DateTime stopInTripDataLu, DateTime expeditionDataLu)
        {
            _cacheService.CacheData(tripDataLu, CacheKeys.TRIP_DATA_LAST_UPDATE);
            _cacheService.CacheData(busStopDataLu, CacheKeys.BUS_STOP_DATA_LAST_UPDATE);
            _cacheService.CacheData(busLineDataLu, CacheKeys.BUS_LINE_DATA_LAST_UPDATE);
            _cacheService.CacheData(expeditionDataLu, CacheKeys.EXPEDITION_LAST_UPDATE);
            _cacheService.CacheData(stopInTripDataLu, CacheKeys.STOP_IN_TRIP_DATA_LAST_UPDATE);
        }

        public bool CheckForUpdates(JObject tripsAsJObject, JObject busStopsAsJObject, JObject busLinesAsJObject, JObject expeditionsAsJObject, JObject stopsInTripsAsJObject)
        {
            var tempTripsLu = GetLastUpdate(tripsAsJObject, CacheKeys.TRIP_DATA_LAST_UPDATE);
            var tempBusStopLu = GetLastUpdate(busStopsAsJObject, CacheKeys.BUS_STOP_DATA_LAST_UPDATE);
            var tempBusLineLu = GetLastUpdate(busStopsAsJObject, CacheKeys.BUS_STOP_DATA_LAST_UPDATE);
            var tempStopInTripLu = GetLastUpdate(busStopsAsJObject, CacheKeys.BUS_STOP_DATA_LAST_UPDATE);
            var tempExpeditionLu = expeditionsAsJObject.HasValues ? expeditionsAsJObject.Value<DateTime>("lastUpdate") : _cacheService.GetData(CacheKeys.EXPEDITION_LAST_UPDATE);

            var actTripsLu = _cacheService.GetData(CacheKeys.TRIP_DATA_LAST_UPDATE);
            var actBusStopLu = _cacheService.GetData(CacheKeys.BUS_STOP_DATA_LAST_UPDATE);
            var actBusLineLu = _cacheService.GetData(CacheKeys.BUS_LINE_DATA_LAST_UPDATE);
            var actExpeditionLu = _cacheService.GetData(CacheKeys.EXPEDITION_LAST_UPDATE);
            var actStopInTripLu = _cacheService.GetData(CacheKeys.STOP_IN_TRIP_DATA_LAST_UPDATE);

            return tempTripsLu.Date > actTripsLu.Date
                   || tempBusStopLu.Date > actBusStopLu.Date
                   || tempBusLineLu.Date > actBusLineLu.Date
                   || tempExpeditionLu.Date > actExpeditionLu.Date
                   || tempStopInTripLu.Date > actStopInTripLu.Date;
        }

        private DateTime GetLastUpdate(JObject jObject, string key)
        {
            return jObject.HasValues
                   ? jObject[DateTime.Now.ToString("yyyy-MM-dd")].Value<DateTime>("lastUpdate")
                   : _cacheService.GetData(key);
        }

        public List<StopTime> FilterStopTimes(List<StopTime> convertedStopTimes)
        {
            convertedStopTimes.ForEach(stopTime =>
            {
                stopTime.Urls = stopTime.Urls
                                        .Select(str => new { str, dt = DateTime.Parse(str.Substring(38, 10)) })
                                        .Where(x => x.dt.Date >= DateTime.Now.Date)
                                        .OrderBy(x => x.dt)
                                        .Select(x => x.str)
                                        .Take(7)
                                        .ToList();
            });

            return convertedStopTimes;
        }
    }
}