using System.Collections.Generic;

namespace DCNC.Bussiness.PublicTransport.JoiningTrips
{
    public class GroupedJoinedModel
    {
        public string Id { get; set; }
        public Group Group { get; set; }
        public List<JoinedTripsModel> JoinedTripModels { get; set; }
    }

    public enum Group
    {
        Buses, Trams, Trolleys
    }

    public class JoinedTripsModel
    {
        public string BusLineName { get; set; }
        public bool ContainsMultiplyTrips { get; set; }
        public List<JoinedTripModel> JoinedTrips { get; set; }
    }

    public class JoinedTripModel
    {
        public string BusLineName { get; set; }
        public int TripId { get; set; }
        public int RouteId { get; set; }
        public int AgencyId { get; set; }
        public string FirstStopName { get; set; }
        public string DestinationStopName { get; set; }
        public bool MainRoute { get; set; }
        public int DirectionId { get; set; }
        public List<JoinedStopModel> Stops { get; set; }
    }

    public class JoinedStopModel
    {
        public string Name { get; set; }
        public int RouteId { get; set; }
        public int TripId { get; set; }
        public int StopId { get; set; }
        public double StopLat { get; set; }
        public double StopLon { get; set; }
        public bool? TicketZoneBorder { get; set; }
        public bool? OnDemand { get; set; }
        public int StopSequence { get; set; }
        public bool MainTrip { get; set; }
    }
}