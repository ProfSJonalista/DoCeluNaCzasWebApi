﻿using DoCeluNaCzasWebApi.Models.PublicTransport.Shared;
using System;
using System.Collections.Generic;

namespace DoCeluNaCzasWebApi.Models.PublicTransport
{
    public class BusStopDataModel : CommonModel
    {
        public List<StopModel> Stops { get; set; }
    }

    public class StopModel
    {
        public int StopId { get; set; }
        public string StopCode { get; set; }
        public string StopName { get; set; }
        public string StopShortName { get; set; }
        public string StopDesc { get; set; }
        public string SubName { get; set; }
        public DateTime Date { get; set; }
        public double StopLat { get; set; }
        public double StopLon { get; set; }
        public int? ZoneId { get; set; }
        public string ZoneName { get; set; }
        public bool? VirtualBusStop { get; set; }
        public bool? NonPassenger { get; set; }
        public bool? Depot { get; set; }
        public bool? TicketZoneBorder { get; set; }
        public bool? OnDemand { get; set; }
        public DateTime ActivationDate { get; set; }
    }
}