using DCNC.Bussiness.Public_Transport;
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
    public class BusStopService
    {
        public async static Task<string> GetStopsForCurrentDay()
        {
            var json = await PublicTransportRepository.GetBusStops();
            JObject stops = (JObject)JsonConvert.DeserializeObject(json);
            var data = BusStopConverter(stops.First);

            var jsonToSend = JsonConvert.SerializeObject(data);
            return jsonToSend;
        }

        private static BusStopData BusStopConverter(JToken busStop)
        {
            BusStopData busStopData = new BusStopData();
            busStopData.Stops = new List<Stop>();
            busStopData.Day = busStop.Path;

            foreach (var item in busStop.Children())
            {
                busStopData.LastUpdate = item.Value<DateTime>("lastUpdate");

                var stopList = item.Value<JArray>("stops");

                foreach (JObject stop in stopList.Children<JObject>())
                {
                    Stop stopToAdd = new Stop()
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
                        ZoneId = stop.Value<int?>("zoneId").HasValue ? stop.Value<int?>("zoneId").Value : 0,
                        ZoneName = stop.Value<string>("zoneName"),
                        VirtualBusStop = stop.Value<bool?>("virtual").HasValue ? stop.Value<bool?>("virtual").Value : false,
                        NonPassenger = stop.Value<bool?>("nonpassenger").HasValue ? stop.Value<bool?>("nonpassenger").Value : false,
                        Depot = stop.Value<bool?>("depot").HasValue ? stop.Value<bool?>("depot").Value : false,
                        TicketZoneBorder = stop.Value<bool?>("ticketZoneBorder").HasValue ? stop.Value<bool?>("ticketZoneBorder").Value : false,
                        OnDemand = stop.Value<bool?>("onDemand").HasValue ? stop.Value<bool?>("onDemand").Value : false,
                        ActivationDate = stop.Value<string>("activationDate")
                    };

                    busStopData.Stops.Add(stopToAdd);
                }
            }

            return busStopData;
        }
    }
}