using System;
using System.Collections.ObjectModel;
using DCNC.Bussiness.PublicTransport.General.Shared;

namespace DCNC.Bussiness.PublicTransport.General
{
    public class BusStopDataModel : CommonModel
    {
        public string Id { get; set; }
        public ObservableCollection<StopModel> Stops { get; set; }
    }

    public class StopModel
    {
        public int StopId { get; set; }
        public string StopDesc { get; set; }
        public double StopLat { get; set; }
        public double StopLon { get; set; }
        public string BusLineNames { get; set; }
        public string DestinationHeadsigns { get; set; }
        public bool? TicketZoneBorder { get; set; }
        public bool? OnDemand { get; set; }
        public DateTime ActivationDate { get; set; }
    }
}