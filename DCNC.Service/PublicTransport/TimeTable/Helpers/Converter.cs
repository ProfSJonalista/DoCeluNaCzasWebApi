using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DCNC.Bussiness.PublicTransport.TimeTable;
using Newtonsoft.Json.Linq;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers
{
    public class Converter
    {
        public TimeTableData Deserialize(JObject jsonAsJObject)
        {
            var timeTableData = new TimeTableData()
            {
                LastUpdate = jsonAsJObject.Value<DateTime>("lastUpdate"),
                StopTimes = new List<StopTime>()
            };

            var stopTimeList = jsonAsJObject.Value<JArray>("stopTimes");

            foreach (var stopTime in stopTimeList.Children<JObject>())
            {
                var stopTimeToAdd = new StopTime()
                {
                    RouteId = stopTime.Value<int>("routeId"),
                    TripId = stopTime.Value<int>("tripId"),
                    AgencyId = stopTime.Value<int>("agencyId"),
                    TopologyVersionId = stopTime.Value<int>("topologyVersionId"),
                    ArrivalTime = stopTime.Value<DateTime>("arrivalTime"),
                    DepartureTime = stopTime.Value<DateTime>("departureTime"),
                    StopId = stopTime.Value<int>("stopId"),
                    StopSequence = stopTime.Value<int>("stopSequence"),
                    Date = stopTime.Value<DateTime>("date"),
                    BusServiceName = stopTime.Value<string>("busServiceName"),
                    Order = stopTime.Value<int>("order")
                };

                timeTableData.StopTimes.Add(stopTimeToAdd);
            }

            var firstStopTime = timeTableData.StopTimes.FirstOrDefault();

            timeTableData.RouteId = firstStopTime.RouteId;
            timeTableData.TripId = firstStopTime.TripId;
            timeTableData.Date = firstStopTime.Date;

            return timeTableData;
        }
    }
}