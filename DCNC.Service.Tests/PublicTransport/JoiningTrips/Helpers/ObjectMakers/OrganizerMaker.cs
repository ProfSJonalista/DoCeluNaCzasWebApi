using DCNC.Bussiness.PublicTransport.JoiningTrips;
using System.Collections.Generic;

namespace DCNC.Service.Tests.PublicTransport.JoiningTrips.Helpers.ObjectMakers
{
    internal static class OrganizerMaker
    {
        public static List<Trip> GetTrips()
        {
            return new List<Trip>
            {
                new Trip
                {
                    AgencyId = 19,
                    BusLineName = "F5",
                    DirectionId = 1,
                    Id = "R585T101",
                    MainRoute = true,
                    RouteId = 585,
                    Stops = new List<Stop>
                    {
                        new Stop
                        {
                            MainTrip = true,
                            Name = "Żabi Kruk",
                            OnDemand = false,
                            RouteId = 585,
                            StopId = 14748,
                            StopLat = 54.32,
                            StopLon = 18.64,
                            StopSequence = 0,
                            TicketZoneBorder = false,
                            TripId = 101,
                            ZoneName = null
                        },
                        new Stop
                        {
                            MainTrip = true,
                            Name = "Zielony Most",
                            OnDemand = false,
                            RouteId = 585,
                            StopId = 14749,
                            StopLat = 54.32,
                            StopLon = 18.64,
                            StopSequence = 1,
                            TicketZoneBorder = false,
                            TripId = 101,
                            ZoneName = null
                        }
                    },
                    TripHeadsign = "Żabi Kruk - Brzeźno",
                    TripId = 101
                },
                new Trip
                {
                    AgencyId = 19,
                    BusLineName = "F5",
                    DirectionId = 2,
                    Id = "R585T102",
                    MainRoute = true,
                    RouteId = 585,
                    Stops = new List<Stop>
                    {
                        new Stop
                        {
                            MainTrip = true,
                            Name = "Brzeźno",
                            OnDemand = false,
                            RouteId = 585,
                            StopId = 14756,
                            StopLat = 54.32,
                            StopLon = 18.64,
                            StopSequence = 0,
                            TicketZoneBorder = false,
                            TripId = 102,
                            ZoneName = null
                        },
                        new Stop
                        {
                            MainTrip = true,
                            Name = "Latarnia Morska",
                            OnDemand = false,
                            RouteId = 585,
                            StopId = 14749,
                            StopLat = 54.32,
                            StopLon = 18.64,
                            StopSequence = 1,
                            TicketZoneBorder = false,
                            TripId = 102,
                            ZoneName = null
                        }
                    },
                    TripHeadsign = "Brzeźno - Żabi Kruk",
                    TripId = 102
                }
            };
        }
    }
}
