using DCNC.Bussiness.PublicTransport;
using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Service.PublicTransport.JsonData.Abstracts;
using DCNC.Service.PublicTransport.TimeTable.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DCNC.Bussiness.PublicTransport.JsonData.Shared;

namespace DCNC.Service.PublicTransport.JsonData
{
    public class TripService : DataAbstractService
    {
        public override Common Converter(JToken trips)
        {
            var tripData = new TripData()
            {
                Day = DateTime.Parse(trips.Path),
                Trips = new List<Trip>()
            };

            foreach (JObject item in trips.Children<JObject>())
            {
                tripData.LastUpdate = item.Value<DateTime>("lastUpdate");

                var tripList = item.Value<JArray>("trips");

                foreach (JObject trip in tripList.Children<JObject>())
                {
                    Trip tripToAdd = new Trip()
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