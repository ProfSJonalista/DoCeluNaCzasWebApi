using DCNC.Bussiness.PublicTransport;
using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.DataAccess.Helpers;
using DCNC.Service.PublicTransport.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Linq;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers
{
    public class TimeTableServiceHelper
    {
        static readonly ObjectCache _cache = MemoryCache.Default;

        public static List<string> CreateUrls()
        {
            var busLineDataList = _cache[CacheKeys.BUS_LINE_DATA_LIST_KEY] as List<BusLineData>;
            var busLineDataListForNextWeek = busLineDataList.OrderBy(x => x.Day).Take(7).ToList();

            var urlListToReturn = new List<string>();

            busLineDataListForNextWeek.ForEach(
                x => x.Routes.ForEach(
                    y => urlListToReturn.Add(
                        CreateUrlForGivenDay(
                            x.Day.ToString("yyyy-MM-dd"), y.RouteId))));

            return urlListToReturn;
        }

        private static string CreateUrlForGivenDay(string date, int routeId)
        {
            return string.Format(Constants.TIME_TABLE_DATA_URL, date, routeId);
        }

        internal static TimeTableData MapTimeTableData(string json)
        {
            JObject timeTableAsJObject = (JObject)JsonConvert.DeserializeObject(json);
            var stopTimes = timeTableAsJObject.Value<JArray>("stopTimes");

            return new TimeTableData()
            {
                LastUpdate = timeTableAsJObject.Value<DateTime>("lastUpdate"),
                StopTimes = StopTimeListMapper(stopTimes)
            };
        }

        private static List<StopTime> StopTimeListMapper(JArray stopTimes)
        {
            List<StopTime> stopTimesToReturn = new List<StopTime>();

            foreach(var item in stopTimes.Children())
            {
                stopTimesToReturn.Add(StopTimeMapper(item));
            }

            return stopTimesToReturn;
        }

        private static StopTime StopTimeMapper(JToken item)
        {
            return new StopTime()
            {
                RouteId = item.Value<int>("routeId"),
                TripId = item.Value<int>("tripId"),
                AgencyId = item.Value<int>("agencyId"),
                TopologyVersionId = item.Value<int>("topologyVersionId"),
                ArrivalTime = item.Value<DateTime>("arrivalTime"),
                DepartureTime = item.Value<DateTime>("departureTime"),
                StopId = item.Value<int>("stopId"),
                StopSequence = item.Value<int>("stopSequence"),
                Date = item.Value<DateTime>("date"),
                VariantId = item.Value<int>("variantId"),
                NoteSymbol = item.Value<string>("noteSymbol"),
                NoteDescription = item.Value<string>("noteDescription"),
                BusServiceName = item.Value<string>("busServiceName"),
                Order = item.Value<int>("order"),
                NonPassenger = item.Value<bool>("nonpassenger"),
                TicketZoneBorder = item.Value<bool>("ticketZoneBorder"),
                OnDemand = item.Value<bool>("onDemand"),
                IsVirtual = item.Value<bool>("virtual"),
                IsSlupek = item.Value<int>("islupek"),
                WheelchairAccessible = item.Value<bool>("wheelchairAccessible"),
                StopShortName = item.Value<int>("stopShortName")
            };
        }
    }
}