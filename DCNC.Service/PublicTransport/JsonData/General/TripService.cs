using DCNC.Bussiness.PublicTransport.JsonData.General;
using DCNC.Service.PublicTransport.JsonData.Abstracts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using DCNC.DataAccess.PublicTransport;
using DCNC.DataAccess.PublicTransport.Interfaces;
using DCNC.Service.Database;
using DCNC.Service.Database.Interfaces;

namespace DCNC.Service.PublicTransport.JsonData.General
{
    public class TripService : DataAbstractService
    {
        public TripService(IDocumentStoreRepository documentStoreRepository, IPublicTransportRepository publicTransportRepository) : base(documentStoreRepository, publicTransportRepository) { }

        protected override object Converter(JToken trips)
        {
            var tripData = new TripData()
            {
                Day = DateTime.Parse(trips.Path),
                Trips = new List<Trip>()
            };

            foreach (var item in trips.Children<JObject>())
            {
                tripData.LastUpdate = item.Value<DateTime>("lastUpdate");

                var tripList = item.Value<JArray>("trips");

                foreach (var trip in tripList.Children<JObject>())
                {
                    var tripToAdd = new Trip()
                    {
                        Id = trip.Value<string>("id"),
                        TripId = trip.Value<int>("tripId"),
                        RouteId = trip.Value<int>("routeId"),
                        TripHeadsign = trip.Value<string>("tripHeadsign"),
                        TripShortName = trip.Value<string>("tripShortName"),
                        DirectionId = trip.Value<int>("directionId"),
                        ActivationDate = trip.Value<DateTime>("activationDate")
                    };

                    tripData.Trips.Add(tripToAdd);
                }
            }

            return tripData;
        }
    }
}