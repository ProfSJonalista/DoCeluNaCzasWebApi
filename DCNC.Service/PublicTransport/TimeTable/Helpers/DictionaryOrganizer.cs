using DCNC.Bussiness.PublicTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers
{
    public static class DictionaryOrganizer
    {
        public static Dictionary<bool, List<StopTripDataModel>> GroupAndOrder(List<StopTripDataModel> tripsWithBusStopsForBusLine)
        {
            var grouped = GroupByDirection(tripsWithBusStopsForBusLine);
            grouped = Order(grouped);

            return grouped;
        }

        private static Dictionary<bool, List<StopTripDataModel>> GroupByDirection(List<StopTripDataModel> tripsWithBusStopsForBusLine)
        {
            var groupedByDirection = new Dictionary<bool, List<StopTripDataModel>>();

            foreach (var trip in tripsWithBusStopsForBusLine)
            {
                var key = trip.Stops.Any(x => x.DirectionId == 1);
                if (!groupedByDirection.ContainsKey(key))
                {
                    groupedByDirection.Add(key, new List<StopTripDataModel>() { trip });
                }
                else
                {
                    groupedByDirection[key].Add(trip);
                }
            }

            return groupedByDirection;
        }

        private static Dictionary<bool, List<StopTripDataModel>> Order(Dictionary<bool, List<StopTripDataModel>> groupedByDirection)
        {
            var orderedDictionary = new Dictionary<bool, List<StopTripDataModel>>();

            foreach (var direction in groupedByDirection)
            {
                var orderedList = direction.Value.OrderByDescending(x => x.MainRoute).ToList();
                orderedDictionary.Add(direction.Key, orderedList);
            }

            return orderedDictionary;
        }
    }
}