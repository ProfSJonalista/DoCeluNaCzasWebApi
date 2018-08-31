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
    public class StopInTripService
    {
        public async static Task<StopInTripData> GetStopInTripData()
        {
            var json = await PublicTransportRepository.GetStopsInTrips();
            JObject stops = (JObject)JsonConvert.DeserializeObject(json);
            return StopInTripConverter(stops.First);
        }

        private static StopInTripData StopInTripConverter(JToken stops)
        {
            StopInTripData stopInTripData = new StopInTripData()
            {
                Day = stops.Path,
                StopsInTrip = new List<StopInTrip>()
            };

            foreach(var item in stops.Children())
            {
                stopInTripData.LastUpdate = item.Value<string>("lastUpdate");

                var stopList = item.Value<JArray>("stopsInTrip");

                foreach(JObject stop in stopList)
                {
                    StopInTrip stopInTripToAdd = new StopInTrip()
                    {
                        RouteId = stop.Value<int>("routeId"),
                        TripId = stop.Value<int>("tripId"),
                        StopId = stop.Value<int>("stopId"),
                        StopSequence = stop.Value<int>("stopSequence"),
                        AgencyId = stop.Value<int>("agencyId"),
                        TopologyVersionId = stop.Value<int>("topologyVersionId"),
                        TripActivationDate = stop.Value<string>("tripActivationDate"),
                        StopActivationDate = stop.Value<string>("stopActivationDate")
                    };

                    stopInTripData.StopsInTrip.Add(stopInTripToAdd);
                }
            }

            return stopInTripData;
        }
    }
}