using DCNC.Bussiness.PublicTransport.JsonData.General;
using DoCeluNaCzasWebApi.Models.PublicTransport.General;
using System.Collections.Generic;
using System.Linq;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.Helpers
{
    public class StopMapper
    {
        public static List<StopModel> GetMappedStopList(List<Stop> joinedStopList)
        {
            return joinedStopList.Select(stop => new StopModel()
            {
                StopId = stop.StopId,
                StopDesc = stop.StopDesc,
                StopLat = stop.StopLat,
                StopLon = stop.StopLon,
                TicketZoneBorder = stop.TicketZoneBorder,
                OnDemand = stop.OnDemand,
                ActivationDate = stop.ActivationDate
            }).ToList();
        }
    }
}