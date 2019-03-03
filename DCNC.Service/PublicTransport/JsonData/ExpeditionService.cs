using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Bussiness.PublicTransport.JsonData.Shared;
using DCNC.Service.PublicTransport.JsonData.Abstracts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace DCNC.Service.PublicTransport.JsonData
{
    public class ExpeditionService : DataAbstractService
    {
        public override List<T> GetList<T>(JObject dataAsJObject)
        {
            return new List<T> { (T)(object)Converter(dataAsJObject) };
        }

        public override Common Converter(JToken expedition)
        {
            var expeditionData = new ExpeditionData()
            {
                Day = DateTime.Today,
                LastUpdate = expedition.Value<DateTime>("lastUpdate"),
                Expeditions = new List<Expedition>()
            };

            var expeditions = expedition.Value<JArray>("expeditionData");

            foreach (var exp in expeditions.Children<JObject>())
            {
                var expeditionToAdd = new Expedition()
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
