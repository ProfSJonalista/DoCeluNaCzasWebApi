using DCNC.Bussiness.PublicTransport.General;
using DCNC.Bussiness.PublicTransport.JsonData.General;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.Joining.Helpers
{
    public class StopMapper
    {
        public static ObservableCollection<StopModel> GetMappedStopList(List<Stop> joinedStopList)
        {
            return new ObservableCollection<StopModel>(joinedStopList.Select(stop => new StopModel()
            {
                StopId = stop.StopId,
                StopDesc = stop.StopDesc,
                StopLat = stop.StopLat,
                StopLon = stop.StopLon,
                TicketZoneBorder = stop.TicketZoneBorder,
                OnDemand = stop.OnDemand,
                ActivationDate = stop.ActivationDate
            }));
        }
    }
}