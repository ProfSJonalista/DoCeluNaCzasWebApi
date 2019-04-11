using DCNC.Bussiness.PublicTransport.Delays;
using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Bussiness.PublicTransport.JsonData.General;
using DCNC.DataAccess.PublicTransport.Helpers;
using DCNC.Service.PublicTransport.JsonData.Delays;
using DoCeluNaCzasWebApi.Models.PublicTransport.Delay;
using DoCeluNaCzasWebApi.Services.PublicTransport.Helpers;
using System.Collections.Generic;
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


        public DelayService()
        {
            _delayJsonService = new DelayJsonService();
        }

        public async Task<List<DelayModel>> GetDelays(int stopId)
        {
            var url = string.Format(Urls.Delays, stopId);
            var jObject = await _delayJsonService.GetDataAsJObjectAsync(url, JsonType.Delay);
            var delayModelList = new List<DelayModel>();

            if (!jObject.HasValues) return delayModelList;

            var data = _delayJsonService.GetData<DelayData>(jObject).FirstOrDefault();

            foreach (var item in data.Delays)
            {
                var routeShortName = BusLineData.Routes.FirstOrDefault(x => x.RouteId == item.RouteId).RouteShortName;
                var tripHeadsign = TripData.Trips.FirstOrDefault(x => x.RouteId == item.RouteId && x.TripId == item.TripId).TripHeadsign;

                var headsign = JoinTripHelper.GetDestinationStopName(tripHeadsign);

                delayModelList.Add(
                    new DelayModel()
                    {
                        RouteId = item.RouteId,
                        TripId = item.TripId,
                        BusLineName = routeShortName,
                        Headsign = headsign,
                        DelayInSeconds = item.DelayInSeconds,
                        TheoreticalTime = item.TheoreticalTime,
                        EstimatedTime = item.EstimatedTime
                    });
            }

            return delayModelList;
        }
    }
}