﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Bussiness.PublicTransport.TimeTable
{
    public class TimeTableJson
    {
        public string Id { get; set; }
        public int RouteId { get; set; }
        public string Json { get; set; }
    }
}