using System;
using System.Collections.Generic;

namespace DoCeluNaCzasWebApi.Models.PublicTransport
{
    public class JoinedTripsModel
    {
        public string BusLineName { get; set; }
        public bool ContainsMultiplyTrips { get; set; }
        public List<JoinedTripModel> JoinedTrips { get; set; }
    }

    public class JoinedTripModel
    {
        public string BusLineName { get; set; }
        public string FirstStopName { get; set; }
        public string DestinationStopName { get; set; }
        public bool MainRoute { get; set; }
        public bool TechnicalTrip { get; set; }
        public DateTime ActivationDate { get; set; }
        public List<JoinedStopModel> Stops { get; set; }
    }

    public class JoinedStopModel
    {
        public int RouteId { get; set; }
        public int TripId { get; set; }
        public int AgencyId { get; set; }
        public int DirectionId { get; set; }
        public int StopId { get; set; }
        public string StopName { get; set; }
        public string TripHeadsign { get; set; }
        public bool OnDemand { get; set; }
        public double StopLat { get; set; }
        public double StopLon { get; set; }
        public int StopSequence { get; set; }
        public string RouteShortName { get; set; }
        public bool BelongsToMainTrip { get; set; }
    }
}