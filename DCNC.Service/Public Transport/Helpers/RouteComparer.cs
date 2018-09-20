﻿using DCNC.Bussiness.Public_Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Service.Public_Transport.Helpers
{
    public class RouteComparer : IEqualityComparer<StopTripModel>
    {
        public bool Equals(StopTripModel x, StopTripModel y)
        {
            return x.StopName.Equals(y.StopName);
        }
        
        public int GetHashCode(StopTripModel obj)
        {
            return 0;
        }
    }
}