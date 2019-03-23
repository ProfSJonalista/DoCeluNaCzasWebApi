using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DCNC.Bussiness.PublicTransport.TimeTable.Shared;

namespace DCNC.Bussiness.PublicTransport.TimeTable
{
    public class TimeTableJson : Common
    {
        public int RouteId { get; set; }
        public string Json { get; set; }
    }
}