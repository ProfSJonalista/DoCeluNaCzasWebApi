using DCNC.Bussiness.PublicTransport;
//using DCNC.Bussiness.PublicTransport.JoinedTrips;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers
{
    public class MapperService
    {
        readonly static Char[] CharactersToDeleteFromString = new Char[] { ' ', '+' };
        readonly static string ParenthesisToDelete = "(\\[.*\\])|(\".*\")|('.*')|(\\(.*\\))";

        //public JoinedTripsViewModel JoinTripsMapper(JoinedTripsModel trips)
        //{
        //    return new JoinedTripsViewModel()
        //    {
        //        BusLineName = trips.BusLineName,
        //        ContainsMultiplyTrips = trips.Trips.Count > 1,
        //        JoinedTrips = StopTripDataMapper(trips.Trips)
        //    };
        //}

        //private List<StopTripDataViewModel> StopTripDataMapper(List<StopTripDataModel> joinedTrips)
        //{
        //    List<StopTripDataViewModel> listToReturn = new List<StopTripDataViewModel>();

        //    foreach (var stopTripData in joinedTrips)
        //    {
        //        var firstStopName = GetFirstStopName(stopTripData.TripHeadsign);
        //        var destinationStopName = GetDestinationStopName(stopTripData.TripHeadsign);

        //        StopTripDataViewModel stopTripDataViewModelToAdd = new StopTripDataViewModel()
        //        {
        //            BusLineName = stopTripData.BusLineName,
        //            FirstStopName = firstStopName,
        //            DestinationStopName = destinationStopName,
        //            MainRoute = stopTripData.MainRoute,
        //            TechnicalTrip = stopTripData.TechnicalTrip,
        //            ActivationDate = stopTripData.ActivationDate,
        //            Stops = StopsMapper(stopTripData.Stops)
        //        };

        //        listToReturn.Add(stopTripDataViewModelToAdd);
        //    }

        //    return listToReturn;
        //}

        //private string GetFirstStopName(string tripHeadsign)
        //{
        //    var belongsToGdynia = tripHeadsign.Contains(">");
        //    var index = 0;

        //    tripHeadsign = tripHeadsign.Trim(CharactersToDeleteFromString);

        //    if (belongsToGdynia)
        //        index = tripHeadsign.IndexOf('>') - 1;
        //    else
        //        index = tripHeadsign.IndexOf('-') - 1;

        //    var input = tripHeadsign.Substring(0, index);

        //    return Regex.Replace(input, ParenthesisToDelete, "");
        //}

        //private string GetDestinationStopName(string tripHeadsign)
        //{
        //    var belongsToGdynia = tripHeadsign.Contains(">");
        //    var index = 0;

        //    if (belongsToGdynia)
        //        index = tripHeadsign.IndexOf('>') + 2;
        //    else
        //        index = tripHeadsign.IndexOf('-') + 2;

        //    var length = tripHeadsign.Length - index;

        //    var input = tripHeadsign.Substring(index, length);

        //    return Regex.Replace(input, ParenthesisToDelete, "");
        //}

        //private List<StopTripViewModel> StopsMapper(List<StopTripModel> stops)
        //{
        //    List<StopTripViewModel> stopsViewModelList = new List<StopTripViewModel>();

        //    foreach (var stop in stops)
        //    {
        //        StopTripViewModel stopTripViewModelToAdd = new StopTripViewModel()
        //        {
        //            RouteId = stop.RouteId,
        //            TripId = stop.TripId,
        //            AgencyId = stop.AgencyId,
        //            DirectionId = stop.DirectionId,
        //            StopId = stop.StopId,
        //            StopName = stop.StopName,
        //            TripHeadsign = stop.TripHeadsign,
        //            OnDemand = stop.OnDemand,
        //            StopLat = stop.StopLat,
        //            StopLon = stop.StopLon,
        //            StopSequence = stop.StopSequence,
        //            RouteShortName = stop.RouteShortName,
        //            BelongsToMainTrip = stop.BelongsToMainTrip
        //        };

        //        stopsViewModelList.Add(stopTripViewModelToAdd);
        //    }

        //    return stopsViewModelList;
        //}
    }
}