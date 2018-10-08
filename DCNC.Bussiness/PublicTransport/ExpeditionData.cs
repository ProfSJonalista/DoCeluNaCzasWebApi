using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Bussiness.PublicTransport
{
    public class ExpeditionData
    {
        public DateTime LastUpdate { get; set; }
        public List<Expedition> Expeditions { get; set; }
    }

    public class Expedition
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RouteId { get; set; }
        public int TripId { get; set; }
        public bool TechnicalTrip { get; set; }
        public bool MainRoute { get; set; }
    }
}