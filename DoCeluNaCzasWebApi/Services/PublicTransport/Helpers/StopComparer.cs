using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DCNC.Bussiness.PublicTransport.JsonData;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.Helpers
{
    public class StopComparer : IEqualityComparer<Stop>
    {
        public bool Equals(Stop x, Stop y)
        {
            return x.StopId == y.StopId;
        }

        public int GetHashCode(Stop obj)
        {
            return 0;
        }
    }
}