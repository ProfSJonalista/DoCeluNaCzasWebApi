using DCNC.Bussiness.PublicTransport;
using DCNC.DataAccess.PublicTransport;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DCNC.Service.PublicTransport
{
    public class ExpeditionService
    {
        public async static Task<ExpeditionData> GetExpeditionData()
        {
            var json = await PublicTransportRepository.GetExpeditionData();
            JObject expeditions = (JObject)JsonConvert.DeserializeObject(json);
            return ExpeditionConverter(expeditions);
        }

        private static ExpeditionData ExpeditionConverter(JToken expedition)
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
