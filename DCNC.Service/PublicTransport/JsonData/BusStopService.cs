using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Bussiness.PublicTransport.JsonData.Shared;
using DCNC.Service.PublicTransport.JsonData.Abstracts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace DCNC.Service.PublicTransport.JsonData
{
    public class BusStopService : DataAbstractService
    {
        public override object Converter(JToken busStop)
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