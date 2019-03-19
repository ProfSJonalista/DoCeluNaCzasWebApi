using DCNC.Bussiness.PublicTransport.JsonData.General.Shared;
using System;
using System.Collections.Generic;

namespace DCNC.Bussiness.PublicTransport.JsonData.General
{
    public class StopInTripData : Common
    {
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
        public DateTime TripActivationDate { get; set; }
        public DateTime StopActivationDate { get; set; }
    }
}