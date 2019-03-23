﻿using DCNC.Bussiness.PublicTransport.JsonData.General;
using DCNC.Service.PublicTransport.JsonData.Abstracts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace DCNC.Service.PublicTransport.JsonData.General
{
    public class StopInTripService : DataAbstractService
    {
        public override object Converter(JToken stops)
        {
            var stopInTripData = new StopInTripData()
            {
                Day = DateTime.Parse(stops.Path),
                StopsInTrip = new List<StopInTrip>()
            };

            foreach (var item in stops.Children())
            {
                stopInTripData.LastUpdate = item.Value<DateTime>("lastUpdate");

                var stopList = item.Value<JArray>("stopsInTrip");

                foreach (var stop in stopList.Children<JObject>())
                {
                    var stopInTripToAdd = new StopInTrip()
                    {
                        RouteId = stop.Value<int>("routeId"),
                        TripId = stop.Value<int>("tripId"),
                        StopId = stop.Value<int>("stopId"),
                        StopSequence = stop.Value<int>("stopSequence"),
                        AgencyId = stop.Value<int>("agencyId"),
                        TopologyVersionId = stop.Value<int>("topologyVersionId"),
                        TripActivationDate = stop.Value<DateTime>("tripActivationDate"),
                        StopActivationDate = stop.Value<DateTime>("stopActivationDate")
                    };

                    stopInTripData.StopsInTrip.Add(stopInTripToAdd);
                }
            }

            return stopInTripData;
        }
    }
}