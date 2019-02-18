﻿using System.Collections.Generic;
using System.Linq;
using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Service.PublicTransport.JoiningTrips.Helpers;
using DCNC.Service.PublicTransport.TimeTable.Helpers;
//using DCNC.Bussiness.PublicTransport.JoinedTrips;

namespace DCNC.Service.PublicTransport.JoiningTrips
{
    public class JoinTripService
    {
        readonly StopHelper _stopHelper;
        readonly JoinTripHelper _joinTripHelper;
        readonly Organizer _dictionaryOrganizer;

        public JoinTripService()
        {
            _stopHelper = new StopHelper();
            _joinTripHelper = new JoinTripHelper();
            _dictionaryOrganizer = new Organizer();
        }

        //public List<JoinedTripsViewModel> JoinTrips(BusLineData busLineData, TripData tripData, StopInTripData stopInTripData, 
        //                                               ExpeditionData expeditionData, BusStopData busStopData, List<StopTripDataModel> tripsWithBusStops)
        //{
        //    var joinedTripsModelList = new List<JoinedTripsModel>();
        //    var distinctBusLines = busLineData.Routes.GroupBy(x => x.RouteShortName).Select(x => x.FirstOrDefault()).ToList();

        //    distinctBusLines.ForEach(busLine => joinedTripsModelList.Add(JoinTripsForEachBusLine(busLine, tripsWithBusStops)));

        //    return _joinTripHelper.JoinedTripsMapper(joinedTripsModelList);
        //}

        //private JoinedTripsModel JoinTripsForEachBusLine(Route busLine, List<StopTripDataModel> tripsWithBusStops)
        //{
        //    var tripsWithBusStopsForBusLine = tripsWithBusStops.Where(x => x.BusLineName.Equals(busLine.RouteShortName)).ToList();

        //    var grouped = _dictionaryOrganizer.GroupAndOrder(tripsWithBusStopsForBusLine);
        //    var joinedTrips = new JoinedTripsModel()
        //    {
        //        BusLineName = busLine.RouteShortName,
        //        Trips = new List<StopTripDataModel>()
        //    };

        //    foreach (var direction in grouped)
        //    {
        //        var mainTrip = direction.Value.FirstOrDefault();

        //        var joinedTripToAdd = new StopTripDataModel()
        //        {
        //            BusLineName = mainTrip.BusLineName,
        //            TripHeadsign = mainTrip.TripHeadsign,
        //            Stops = new List<StopTripModel>()
        //        };

        //        foreach (var trip in direction.Value)
        //        {
        //            foreach (var stop in mainTrip.Stops)
        //            {
        //                foreach (var stopToCheck in trip.Stops)
        //                {
        //                    var isSame = _stopHelper.CheckIfStopsAreTheSame(stop, stopToCheck);
        //                    var alreadyExist = JoinTripHelper.CheckIfBusStopAlreadyExists(stopToCheck.RouteShortName, stopToCheck.StopLat, stopToCheck.StopLon, joinedTripToAdd);

        //                    if (isSame || alreadyExist) continue;
        //                    if (joinedTripToAdd.Stops.Count == 0)
        //                        joinedTripToAdd.Stops.Add(stopToCheck);
        //                    else
        //                    {
        //                        var index = trip.Stops.IndexOf(stopToCheck);

        //                        var indexToAdd = 0;
        //                        indexToAdd = index == 0 ? index : JoinTripHelper.GetIndexToAdd(trip.Stops, index, joinedTripToAdd);

        //                        joinedTripToAdd.Stops.Insert(indexToAdd, stopToCheck);
        //                    }
        //                }
        //            }
        //        }

        //        var sequence = 0;

        //        foreach (var stop in joinedTripToAdd.Stops)
        //        {
        //            stop.StopSequence = sequence;
        //            sequence++;
        //        }

        //        joinedTrips.Trips.Add(joinedTripToAdd);
        //    }

        //    return joinedTrips;
        //}
    }
}