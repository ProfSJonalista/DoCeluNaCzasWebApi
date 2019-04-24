using Newtonsoft.Json.Linq;
using System;
using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;

namespace DCNC.Service.PublicTransport.Time
{
    public class TimeService
    {
        public void CacheLastUpdates(DateTime tripDataLu, DateTime busStopDataLu, DateTime busLineDataLu, DateTime stopInTripDataLu, DateTime expeditionDataLu)
        {
            CacheService.CacheData(tripDataLu, CacheKeys.TRIP_DATA_LAST_UPDATE);
            CacheService.CacheData(busStopDataLu, CacheKeys.BUS_STOP_DATA_LAST_UPDATE);
            CacheService.CacheData(busLineDataLu, CacheKeys.BUS_LINE_DATA_LAST_UPDATE);
            CacheService.CacheData(expeditionDataLu, CacheKeys.EXPEDITION_LAST_UPDATE);
            CacheService.CacheData(stopInTripDataLu, CacheKeys.STOP_IN_TRIP_DATA_LAST_UPDATE);
        }

        public bool CheckForUpdates(JObject tripsAsJObject, JObject busStopsAsJObject, JObject busLinesAsJObject, JObject expeditionsAsJObject, JObject stopsInTripsAsJObject)
        {
            var tempTripsLu = GetLastUpdate(tripsAsJObject, CacheKeys.TRIP_DATA_LAST_UPDATE);
            var tempBusStopLu = GetLastUpdate(busStopsAsJObject, CacheKeys.BUS_STOP_DATA_LAST_UPDATE);
            var tempBusLineLu = GetLastUpdate(busStopsAsJObject, CacheKeys.BUS_STOP_DATA_LAST_UPDATE);
            var tempStopInTripLu = GetLastUpdate(busStopsAsJObject, CacheKeys.BUS_STOP_DATA_LAST_UPDATE);
            var tempExpeditionLu = expeditionsAsJObject.HasValues ? expeditionsAsJObject.Value<DateTime>("lastUpdate") : CacheService.GetData(CacheKeys.EXPEDITION_LAST_UPDATE);

            var actTripsLu = CacheService.GetData(CacheKeys.TRIP_DATA_LAST_UPDATE);
            var actBusStopLu = CacheService.GetData(CacheKeys.BUS_STOP_DATA_LAST_UPDATE);
            var actBusLineLu = CacheService.GetData(CacheKeys.BUS_LINE_DATA_LAST_UPDATE);
            var actExpeditionLu = CacheService.GetData(CacheKeys.EXPEDITION_LAST_UPDATE);
            var actStopInTripLu = CacheService.GetData(CacheKeys.STOP_IN_TRIP_DATA_LAST_UPDATE);

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
                   : CacheService.GetData(key);
        }
    }
}