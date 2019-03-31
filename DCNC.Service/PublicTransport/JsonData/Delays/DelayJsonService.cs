using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DCNC.Bussiness.PublicTransport.Delays;
using DCNC.Service.Database;
using DCNC.Service.PublicTransport.JsonData.Abstracts;
using Newtonsoft.Json.Linq;

namespace DCNC.Service.PublicTransport.JsonData.Delays
{
    public class DelayJsonService : DataAbstractService
    {
        public DelayJsonService()
        {
            
        }

        public DelayJsonService(DocumentStoreRepository documentStoreRepository) : base(documentStoreRepository) { }

        public override object Converter(JToken delaysJToken)
        {
            if (!delaysJToken.HasValues) return new DelayData();

            var delayData = new DelayData()
            {
                LastUpdate = delaysJToken.Value<DateTime>("lastUpdate"),
                Delays = new List<Delay>()
            };

            var delayList = delaysJToken.Value<JArray>("delay");

            foreach (var item in delayList.Children<JToken>())
            {
                var delayToAdd = new Delay()
                {
                    Id = item.Value<string>("id"),
                    DelayInSeconds = item.Value<int>("delayInSeconds"),
                    EstimatedTime = item.Value<DateTime>("estimatedTime"),
                    Headsign = item.Value<string>("headsign"),
                    RouteId = item.Value<int>("routeId"),
                    TripId = item.Value<int>("tripId"),
                    TheoreticalTime = item.Value<DateTime>("theoreticalTime"),
                    TimeStamp = item.Value<DateTime>("timestamp")
                };

                delayData.Delays.Add(delayToAdd);
            }

            return delayData;
        }
    }
}