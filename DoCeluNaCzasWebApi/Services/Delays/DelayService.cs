using DCNC.Bussiness.PublicTransport.Delays;
using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Bussiness.PublicTransport.JsonData.General;
using DCNC.DataAccess.PublicTransport.Helpers;
using DCNC.Service.PublicTransport.JsonData.Delays;
using DoCeluNaCzasWebApi.Models.PublicTransport.Delay;
using DoCeluNaCzasWebApi.Services.PublicTransport.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
// ReSharper disable PossibleNullReferenceException

namespace DoCeluNaCzasWebApi.Services.Delays
{
    public class DelayService
    {
        public static BusLineData BusLineData { get; set; }
        public static TripData TripData { get; set; }
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

                return new DelayModel()
                {
                    RouteId = item.RouteId,
                    TripId = item.TripId,
                    BusLineName = routeShortName,
                    Headsign = headsign,
                    DelayInSeconds = item.DelayInSeconds,
                    TheoreticalTime = item.TheoreticalTime,
                    EstimatedTime = item.EstimatedTime,
                    Timestamp = item.TimeStamp
                };
            }).ToList();

            return new ObservableCollection<DelayModel>(convertedData);
        }
    }
}