using System.Collections.Generic;

namespace DCNC.Bussiness.PublicTransport.JsonData.TimeTable
{
    public class StopTime
    {
        public int RouteId { get; set; }
        public List<string> Urls { get; set; }
    }
}