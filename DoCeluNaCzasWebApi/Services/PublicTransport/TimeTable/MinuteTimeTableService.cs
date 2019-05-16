using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using DCNC.Service.Database;
using DoCeluNaCzasWebApi.Models.PublicTransport.General;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.TimeTable
{
    public class MinuteTimeTableService
    {
        private readonly IDocumentStoreRepository _documentStoreRepository;

        public MinuteTimeTableService(IDocumentStoreRepository documentStoreRepository)
        {
            _documentStoreRepository = documentStoreRepository;
        }

        public async Task SetMinuteTimeTables()
        {
            var groupedJoinedTrips = CacheService.GetData<List<GroupedJoinedModel>>(CacheKeys.GROUPED_JOINED_TRIPS);

            foreach (var group in groupedJoinedTrips)
            {
                foreach (var joinedTrip in group.JoinedTripModels)
                {
                    var minuteTimeTableList = _documentStoreRepository.GetMinuteTimeTableListByBusLineName(joinedTrip.BusLineName);
                    //create entities to delete - entities to delete are the ones which does not have StopId equivalent in lists below
                    //var existingStopIds = 
                    foreach (var trip in joinedTrip.JoinedTrips)
                    {
                        var timeTableData = _documentStoreRepository.GetTimeTableDataByRouteId(trip.RouteId);

                        foreach (var tripStop in trip.Stops)
                        {
                            var minuteTimeTable =
                                minuteTimeTableList.SingleOrDefault(x => x.StopId == tripStop.StopId)
                             ?? new MinuteTimeTable
                             {
                                 StopId = tripStop.StopId,
                                 BusLineName = trip.BusLineName,
                                 RouteIds = joinedTrip.JoinedTrips.Select(x => x.RouteId).ToList(),
                                 MinuteDictionary = new Dictionary<DayType, Dictionary<int, List<int>>>()
                             };

                            foreach (var dayType in (DayType[])Enum.GetValues(typeof(DayType)))
                            {
                                //tu pobrać odpowiedni rodzaj dnia
                                //jeśli to weekday lub sobota, sprawdzić czy nie równa się niedzieli (i świętom)
                                //jeśli weekday równa się niedzieli, pobrać inny dzień (inny z tego samego tygodnia/po nazwie dnia tygodnia) i ponownie sprawdzić
                                //todo create something to differantiate days
                                var day = timeTableData.FirstOrDefault();
                                var hourAndMinuteDictionary = minuteTimeTable.MinuteDictionary.ContainsKey(dayType) ? minuteTimeTable.MinuteDictionary[dayType] : new Dictionary<int, List<int>>();

                                for (var hour = 0; hour < 24; hour++)
                                {
                                    var containsHour = hourAndMinuteDictionary.ContainsKey(hour);
                                    //todo handle when day is null
                                    var stopTimesByHour = day.StopTimes.Where(x => x.StopId == tripStop.StopId && x.DepartureTime.Hour == hour).ToList();
                                    var minuteList = stopTimesByHour.Select(x => x.DepartureTime.Minute).OrderBy(y => y).ToList();

                                    if (!containsHour)
                                    {
                                        hourAndMinuteDictionary.Add(hour, new List<int>());
                                    }

                                    hourAndMinuteDictionary[hour] = minuteList;
                                }

                                minuteTimeTable.MinuteDictionary[dayType] = hourAndMinuteDictionary;
                            }

                            var minuteTimeTableIndex = minuteTimeTableList.FindIndex(x => x.Id == minuteTimeTable.Id);

                            if (minuteTimeTableIndex >= 0)
                                minuteTimeTableList[minuteTimeTableIndex] = minuteTimeTable;
                            else
                                minuteTimeTableList.Add(minuteTimeTable);

                            //pobrać odpowiedni MinuteTimeTable po routeId i stopId
                            //jeśli nie jest nullem, porównać minuteTimeTableToSave wraz ze starym wynikiem
                            //jeśli się nie różnią, olać minuteTimeTableToSave
                            //jeśli się różnią LUB wynik z bazy jest nullem - zapisać w bazie
                        }
                    }
                }
            }
        }
    }
}