using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCNC.Bussiness.PublicTransport.JoiningTrips;

namespace DCNC.Service.Tests.PublicTransport.JoiningTrips.Helpers.ObjectMakers
{
    internal static class StopMaker
    {
        public static (Stop, Stop) GetEqualStops()
        {
            return (
                new Stop
                {
                    StopId = 1,
                    Name = "Sopocka",
                    StopLat = 69.420,
                    StopLon = 69.666
                },
                new Stop
                {
                    StopId = 1,
                    Name = "Sopocka",
                    StopLat = 69.420,
                    StopLon = 69.666
                }
                );
        }

        public static (Stop, Stop) GetNotEqualStops()
        {
            return (
                new Stop
                {
                    StopId = 1,
                    Name = "Sopocka",
                    StopLat = 69.420,
                    StopLon = 69.666
                },
                new Stop
                {
                    StopId = 1,
                    Name = "Sopocka",
                    StopLat = 69.420,
                    StopLon = 420.69
                }
            );
        }
    }
}
