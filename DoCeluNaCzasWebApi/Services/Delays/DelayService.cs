using DCNC.Bussiness.PublicTransport.JsonData.General;
using DCNC.DataAccess.PublicTransport.Helpers;
using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using DCNC.Service.PublicTransport.JsonData.Delays;
using DoCeluNaCzasWebApi.Models.PublicTransport.Delay;
using DoCeluNaCzasWebApi.Models.PublicTransport.General;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoCeluNaCzasWebApi.Services.PublicTransport.Joining.Helpers;

// ReSharper disable PossibleNullReferenceException

namespace DoCeluNaCzasWebApi.Services.Delays
{
    public class DelayService
    {
        public static TripData TripData { get; set; }
        public static BusLineData BusLineData { get; set; }
        private readonly DelayJsonService _delayJsonService;

        public DelayService(DelayJsonService delayJsonService)
        {
            _delayJsonService = delayJsonService;
        }

        public async Task<ObservableCollection<DelayModel>> GetDelays(int stopId)
        {
            var url = string.Format(Urls.Delays, stopId);
            var data = await _delayJsonService.GetData(url);

            var convertedData = data.Delays.Select(item =>
            {
                var routeShortName = BusLineData.Routes.FirstOrDefault(x => x.RouteId == item.RouteId).RouteShortName;
                var tripHeadsign = TripData.Trips
                    .FirstOrDefault(x => x.RouteId == item.RouteId && x.TripId == item.TripId).TripHeadsign;
                var headsign = JoinTripHelper.GetDestinationStopName(tripHeadsign);
                var delayMessage = GetDelayMessage(item.DelayInSeconds);

                return new DelayModel()
                {
                    RouteId = item.RouteId,
                    TripId = item.TripId,
                    BusLineName = routeShortName,
                    Headsign = headsign,
                    DelayMessage = delayMessage,
                    TheoreticalTime = item.TheoreticalTime,
                    EstimatedTime = item.EstimatedTime,
                    Timestamp = item.TimeStamp
                };
            }).ToList();

            return new ObservableCollection<DelayModel>(convertedData);
        }

        private string GetDelayMessage(int itemDelayInSeconds)
        {
            if (itemDelayInSeconds >= 0)
            {
                var minutes = itemDelayInSeconds / 60;

                if (minutes > 0) return minutes + "min";

                var seconds = itemDelayInSeconds % 60;
                return seconds > 10 ? "> 1 min" : "Teraz!";
            }
            else
            {
                var minutes = itemDelayInSeconds / 60;

                if (minutes < 0) return minutes + "min";

                var seconds = itemDelayInSeconds % 60;
                return seconds < 10 ? "> 1 min" : "Teraz!";
            }
        }


        public static void SetChooseBusStopModelCollection(BusStopDataModel busStopDataModel, List<GroupedJoinedModel> groupedJoinedTrips)
        {
            var chooseBusStopCollection = busStopDataModel.Stops.Select(stop =>
            {
                var busLineNamesStringBuilder = new StringBuilder();
                var destinationsStringBuilder = new StringBuilder();

                foreach (var groupedJoinedModel in groupedJoinedTrips)
                {
                    foreach (var joinedTripModel in groupedJoinedModel.JoinedTripModels)
                    {
                        var joinedTripModelList = joinedTripModel.JoinedTrips.Where(x => x.Stops.Any(y => y.StopId == stop.StopId)).ToList();

                        foreach (var item in joinedTripModelList)
                        {
                            if (!destinationsStringBuilder.ToString().Contains(item.BusLineName))
                                busLineNamesStringBuilder.Append(item.BusLineName + ", ");

                            if (!destinationsStringBuilder.ToString().Contains(item.DestinationStopName))
                                destinationsStringBuilder.Append(item.DestinationStopName + ", ");
                        }
                    }
                }

                var busLines = busLineNamesStringBuilder.ToString();
                if (busLines.Length > 0)
                    busLines = busLines.Substring(0, busLines.Length - 2);

                var destinations = destinationsStringBuilder.ToString();
                if (destinations.Length > 0)
                    destinations = destinations.Substring(0, destinations.Length - 2); //removes extra ", " at the end of the string

                return new ChooseBusStopModel
                {
                    StopId = stop.StopId,
                    StopDesc = stop.StopDesc,
                    BusLineNames = busLines,
                    DestinationHeadsigns = destinations
                };
            }).ToList();

            CacheService.CacheData(new ObservableCollection<ChooseBusStopModel>(chooseBusStopCollection), CacheKeys.CHOOSE_BUS_STOP_MODEL_OBSERVABALE_COLLECTION);
        }
    }
}