using DCNC.Bussiness.PublicTransport.JoiningTrips;
using System.Collections.Generic;

namespace DCNC.Service.PublicTransport.JoiningTrips.Helpers
{
    public class StopComparer : IEqualityComparer<Stop>
    {
        public bool Equals(Stop x, Stop y)
        {
            return x.StopId == y.StopId
                   && x.Name.Equals(y.Name)
                   && x.StopLat.Equals(y.StopLat)
                   && x.StopLon.Equals(y.StopLon);
        }

        public int GetHashCode(Stop obj)
        {
            return obj.GetHashCode();
        }
    }
}