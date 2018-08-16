﻿using DCNC.Bussiness.Public_Transport;
using DCNC.DataAccess.PublicTransport;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DCNC.Service.Public_Transport
{
    public class TripService
    {
        public async static Task<TripData> GetTripData()
        {
            var json = await PublicTransportRepository.GetTrips();
            JObject trips = (JObject)JsonConvert.DeserializeObject(json);
            return TripConverter(trips.First);
        }

        private static TripData TripConverter(JToken trips)
        {
            var tripData = new TripData()
            {
                Day = trips.Path,
                Trips = new List<Trip>()
            };

            foreach(JObject item in trips.Children<JObject>())
            {
                tripData.LastUpdate = item.Value<string>("lastUpdate");

                var tripList = item.Value<JArray>("trips");

                foreach(JObject trip in tripList.Children<JObject>())
                {
                    Trip tripToAdd = new Trip()
                    {
                        Id = trip.Value<string>("id"),
                        TripId = trip.Value<int>("tripId"),
                        RouteId = trip.Value<int>("routeId"),
                        TripHeadsign = trip.Value<string>("tripHeadsign"),
                        TripShortName = trip.Value<string>("tripShortName"),
                        DirectionId = trip.Value<int>("directionId"),
                        ActivationDate = trip.Value<string>("activationDate")
                    };

                    tripData.Trips.Add(tripToAdd);
                }
            }

            return tripData;
        }
    }
}