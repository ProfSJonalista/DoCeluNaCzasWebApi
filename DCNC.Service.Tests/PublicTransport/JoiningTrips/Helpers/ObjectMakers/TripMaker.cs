using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCNC.Bussiness.PublicTransport.JoiningTrips;
using DCNC.Service.PublicTransport.JoiningTrips.Helpers.Keys;

namespace DCNC.Service.Tests.PublicTransport.Helpers.ObjectMakers.JoiningTrips
{
    public static class TripMaker
    {
        public static List<OrganizedTrips> GetOrganizedTripList(bool mainRoute)
        {
            var trips = new Dictionary<int, List<Trip>>();
            var listTrip = new List<Trip>
            {
                new Trip
                {
                    BusLineName = "150",
                    MainRoute = mainRoute,
                    Stops = new List<Stop>
                    {
                        new Stop
                        {
                            Name = "Witomino Centrum",
                            MainTrip = mainRoute
                        },
                        new Stop
                        {
                            Name = "Rolnicza",
                            MainTrip = mainRoute
                        }
                    }
                }
            };
            trips.Add(TripKey.START, listTrip);
            trips.Add(TripKey.RETURN, listTrip);
    
            var list = new List<OrganizedTrips>
            {
                new OrganizedTrips
                {
                    Day = new DateTime(),
                    BusLineName = "150",
                    Trips = trips
                }
            };

            return list;
        }

        public static Trip GetTrip()
        {
            return new Trip
            {
                BusLineName = "150",
                MainRoute = false,
                Stops = new List<Stop>
                {
                    new Stop
                    {
                        Name = "Witomino Centrum",
                        MainTrip = false
                    },
                    new Stop
                    {
                        Name = "Rolnicza",
                        MainTrip = false
                    }
                }
            };
        }
    }
}
