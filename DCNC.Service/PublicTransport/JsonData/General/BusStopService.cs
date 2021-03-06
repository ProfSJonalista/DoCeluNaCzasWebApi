﻿using DCNC.Bussiness.PublicTransport.JsonData.General;
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
    public class BusStopService : DataAbstractService
    {
        public BusStopService(IDocumentStoreRepository documentStoreRepository, IPublicTransportRepository publicTransportRepository) : base(documentStoreRepository, publicTransportRepository) { }

        protected override object Converter(JToken busStop)
        {
            var busStopData = new BusStopData()
            {
                Day = DateTime.Parse(busStop.Path),
                Stops = new List<Stop>()
            };

            foreach (var item in busStop.Children())
            {
                busStopData.LastUpdate = item.Value<DateTime>("lastUpdate");

                var stopList = item.Value<JArray>("stops");

                foreach (var stop in stopList.Children<JObject>())
                {
                    var stopToAdd = new Stop()
                    {
                        StopId = stop.Value<int>("stopId"),
                        StopCode = stop.Value<string>("stopCode"),
                        StopName = stop.Value<string>("stopName"),
                        StopShortName = stop.Value<string>("stopShortName"),
                        StopDesc = stop.Value<string>("stopDesc"),
                        SubName = stop.Value<string>("subName"),
                        Date = stop.Value<DateTime>("date"),
                        StopLat = stop.Value<double>("stopLat"),
                        StopLon = stop.Value<double>("stopLon"),
                        ZoneId = stop.Value<int?>("zoneId") ?? 0,
                        ZoneName = stop.Value<string>("zoneName"),
                        VirtualBusStop = stop.Value<bool?>("virtual") ?? false,
                        NonPassenger = stop.Value<bool?>("nonpassenger") ?? false,
                        Depot = stop.Value<bool?>("depot") ?? false,
                        TicketZoneBorder = stop.Value<bool?>("ticketZoneBorder") ?? false,
                        OnDemand = stop.Value<bool?>("onDemand") ?? false,
                        ActivationDate = stop.Value<DateTime>("activationDate")
                    };

                    busStopData.Stops.Add(stopToAdd);
                }
            }

            return busStopData;
        }
    }
}