using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Bussiness.Public_Transport
{
    public class StopInTripData
    {
        public string Day { get; set; }
        public string LastUpdate { get; set; }
        public IList<StopInTrip> StopsInTrip { get; set; }
    }

    public class StopInTrip
    {
        public int RouteId { get; set; }
        public int TripId { get; set; }
        public int StopId { get; set; }
        public int StopSequence { get; set; }
        public int AgencyId { get; set; }
        public int TopologyVersionId { get; set; }
        public string TripActivationDate { get; set; }
        public string StopActivationDate { get; set; }
    }
}