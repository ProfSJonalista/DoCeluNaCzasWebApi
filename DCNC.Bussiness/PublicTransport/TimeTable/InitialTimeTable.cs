using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Bussiness.PublicTransport.TimeTable
{
    public class InitialTimeTable
    {
        public int RouteId { get; set; }
        public List<string> UrlsWithTimeTables { get; set; }
    }
}