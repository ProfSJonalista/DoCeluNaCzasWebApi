using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoCeluNaCzasWebApi.Models.PublicTransport.Shared;

namespace DoCeluNaCzasWebApi.Models.PublicTransport
{
    public class BusLineDataModel : CommonModel
    {
        private List<RouteViewModel> Routes { get; set; }
    }

    public class RouteViewModel
    {
        public int RouteId { get; set; }
        public int AgencyId { get; set; }
        public string RouteShortName { get; set; }
        public string RouteLongName { get; set; }
        public DateTime ActivationDate { get; set; }
    }
}