using System.Collections.Generic;

namespace DCNC.Bussiness.PublicTransport.JsonData.TimeTable
{
    public class StopTimeUrl
    {
        public int RouteId { get; set; }
        public List<string> Urls { get; set; }
    }
}