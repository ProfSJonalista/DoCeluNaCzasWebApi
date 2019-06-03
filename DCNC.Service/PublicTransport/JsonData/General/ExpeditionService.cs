using DCNC.Bussiness.PublicTransport.JsonData.General;
using DCNC.DataAccess.PublicTransport.Interfaces;
using DCNC.Service.Database.Interfaces;
using DCNC.Service.PublicTransport.JsonData.Abstracts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace DCNC.Service.PublicTransport.JsonData.General
{
    public class ExpeditionService : DataAbstractService
    {
        public ExpeditionService(IDocumentStoreRepository documentStoreRepository, IPublicTransportRepository publicTransportRepository) : base(documentStoreRepository, publicTransportRepository) { }

        public override List<T> GetData<T>(JObject dataAsJObject)
        {
            return new List<T> { (T)(object)Converter(dataAsJObject) };
        }

        protected override object Converter(JToken expedition)
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
