using DCNC.Bussiness.PublicTransport.RouteSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using DCNC.Bussiness.PublicTransport.JsonData;

namespace DCNC.Service.PublicTransport.RouteSearch
{
    public class RouteSearchService
    {
        static readonly ObjectCache _cache = MemoryCache.Default;

        //public List<string> SearchRoute(int startStopId, int destinationStopId)
        //{
        //    var generalData = _cache[CacheKeys.GENERAL_DATA_KEY] as GeneralData;
        //    var joinedTrips = generalData.JoinedTrips;
        //    var startLines = new List<String>();
        //    var stopLines = new List<String>();
        //    var lineStopMap = new Dictionary<string, List<StopTripViewModel>>();

        //    joinedTrips.ForEach(joinedTrip =>
        //        joinedTrip.JoinedTrips.ForEach(trip =>
        //            trip.Stops.ForEach(stop =>
        //            {
        //                if (stop.StopId == startStopId)
        //                {
        //                    startLines.Add(joinedTrip.BusLineName);
        //                    lineStopMap[joinedTrip.BusLineName] = trip.Stops;
        //                }
        //                if (stop.StopId == destinationStopId)
        //                {
        //                    stopLines.Add(joinedTrip.BusLineName);
        //                    lineStopMap[joinedTrip.BusLineName] = trip.Stops;

        //                }
        //            })));

        //    var directLines = new List<String>();
        //    var changeLines = new List<Change>();
        //    foreach (var stop in stopLines)
        //    {
        //        foreach (var stop2 in startLines)
        //        {
        //            if (stop2.Equals(stop))
        //            {
        //                directLines.Add(stop2);
        //            }
        //        }
        //    }

        //    // todo break if any direc..

        //    startLines.ForEach(startLine =>
        //        stopLines.ForEach(stopLine =>
        //            lineStopMap[startLine].ForEach(startStop =>
        //                lineStopMap[stopLine].ForEach(desStop =>
        //                {
        //                    if (startStop.StopId == desStop.StopId)
        //                    {
        //                        var change = new Change()
        //                        {
        //                            FirstLine = startLine,
        //                            SecondLine = stopLine,
        //                            StopName = startStop.StopName
        //                        };

        //                        changeLines.Add(change);
        //                    }
        //                }))));

        //    return directLines;
        //}
    }
}//2214
 //2102