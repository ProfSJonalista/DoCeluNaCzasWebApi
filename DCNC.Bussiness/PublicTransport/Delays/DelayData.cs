using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DCNC.Bussiness.PublicTransport.Delays
{
    public class DelayData
    {
        [JsonProperty("lastUpdate")]
        public DateTime LastUpdate { get; set; }
        [JsonProperty("delay")]
        public List<Delay> Delays { get; set; }
    }

    public class Delay
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("delayInSeconds")]
        public int DelayInSeconds { get; set; }

        [JsonProperty("estimatedTime")]
        public DateTime EstimatedTime { get; set; }

        [JsonProperty("headsign")]
        public string Headsign { get; set; }

        [JsonProperty("routeId")]
        public int RouteId { get; set; }

        [JsonProperty("tripId")]
        public int TripId { get; set; }

        [JsonProperty("theoreticalTime")]
        public DateTime TheoreticalTime { get; set; }

        [JsonProperty("timestamp")]
        public DateTime TimeStamp { get; set; }
    }
}