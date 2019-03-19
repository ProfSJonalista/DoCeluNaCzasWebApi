using DCNC.Bussiness.PublicTransport.JsonData.General;
using DoCeluNaCzasWebApi.Models.PublicTransport;
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
                StopCode = stop.StopCode,
                StopName = stop.StopName,
                StopShortName = stop.StopShortName,
                StopDesc = stop.StopDesc,
                SubName = stop.SubName,
                Date = stop.Date,
                StopLat = stop.StopLat,
                StopLon = stop.StopLon,
                ZoneId = stop.ZoneId,
                ZoneName = stop.ZoneName,
                VirtualBusStop = stop.VirtualBusStop,
                NonPassenger = stop.NonPassenger,
                Depot = stop.Depot,
                TicketZoneBorder = stop.TicketZoneBorder,
                OnDemand = stop.OnDemand,
                ActivationDate = stop.ActivationDate
            }).ToList();
        }
    }
}