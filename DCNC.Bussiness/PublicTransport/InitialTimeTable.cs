using System.Collections.Generic;

namespace DCNC.Bussiness.PublicTransport
{
    public class InitialTimeTable
    {
        public int RouteId { get; set; }
        public List<string> UrlsWithTimeTables { get; set; }
    }
}