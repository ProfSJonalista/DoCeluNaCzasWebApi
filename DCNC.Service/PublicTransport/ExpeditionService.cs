using DCNC.Bussiness.PublicTransport;
using DCNC.DataAccess.PublicTransport;
using DCNC.Service.PublicTransport.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;

namespace DCNC.Service.PublicTransport
{
    public class ExpeditionService
    {
        static readonly ObjectCache _cache = MemoryCache.Default;
        PublicTransportRepository _publicTransportRepository;

        public ExpeditionService()
        {
            _publicTransportRepository = new PublicTransportRepository();
        }

        public async Task<ExpeditionData> GetExpeditionData()
        {
            var json = await _publicTransportRepository.GetExpeditionData();
            JObject expeditions = (JObject)JsonConvert.DeserializeObject(json);

            if (!expeditions.HasValues)
            {
                return _cache[CacheKeys.EXPEDITION_DATA_LIST_KEY] as ExpeditionData;
            }

            return CacheExpeditionDataAndGetResult(expeditions);
        }

        private ExpeditionData CacheExpeditionDataAndGetResult(JObject expeditions)
        {
            var expeditionsToCache = ExpeditionConverter(expeditions);
            _cache.Set(CacheKeys.EXPEDITION_DATA_LIST_KEY, expeditionsToCache, new CacheItemPolicy());
            return expeditionsToCache;
        }

        private ExpeditionData ExpeditionConverter(JToken expedition)
        {
            ExpeditionData expeditionData = new ExpeditionData()
            {
                LastUpdate = expedition.Value<DateTime>("lastUpdate"),
                Expeditions = new List<Expedition>()
            };

            var expeditions = expedition.Value<JArray>("expeditionData");

            foreach (var exp in expeditions.Children<JObject>())
            {
                Expedition expeditionToAdd = new Expedition()
                {
                    StartDate = exp.Value<DateTime>("startDate"),
                    EndDate = exp.Value<DateTime>("endDate"),
                    RouteId = exp.Value<int>("routeId"),
                    TripId = exp.Value<int>("tripId"),
                    TechnicalTrip = exp.Value<bool>("technicalTrip"),
                    MainRoute = exp.Value<bool>("mainRoute")
                };

                expeditionData.Expeditions.Add(expeditionToAdd);
            }

            return expeditionData;
        }
    }
}
