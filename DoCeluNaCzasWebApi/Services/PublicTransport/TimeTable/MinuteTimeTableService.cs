using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using DCNC.Service.Database;
using DoCeluNaCzasWebApi.Models.PublicTransport.General;
using DoCeluNaCzasWebApi.Services.PublicTransport.TimeTable.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.TimeTable
{
    public class MinuteTimeTableService
    {
        
        private readonly MinuteTimeTableBuilder _minuteTimeTableBuilder;
        private readonly IDocumentStoreRepository _documentStoreRepository;

        public MinuteTimeTableService(MinuteTimeTableBuilder minuteTimeTableBuilder, IDocumentStoreRepository documentStoreRepository)
        {
            _minuteTimeTableBuilder = minuteTimeTableBuilder;
            _documentStoreRepository = documentStoreRepository;
        }

        public void SetMinuteTimeTables()
        {
            var groupedJoinedTrips = CacheService.GetData<List<GroupedJoinedModel>>(CacheKeys.GROUPED_JOINED_TRIPS);

            foreach (var group in groupedJoinedTrips)
            {
                foreach (var joinedTrip in group.JoinedTripModels)
                {
                    //todo move to different class
                    var minuteTimeTableList = _documentStoreRepository.GetMinuteTimeTableListByBusLineName(joinedTrip.BusLineName);
                    var existingStopIds = joinedTrip.JoinedTrips.SelectMany(x => x.Stops.Select(y => y.StopId).ToList()).Distinct().OrderBy(x => x).ToList();
                    minuteTimeTableList = minuteTimeTableList.Where(x => existingStopIds.Any(y => y == x.StopId)).ToList();

                    var minutesTimeTablesToDelete = minuteTimeTableList.Where(x => existingStopIds.All(y => y != x.StopId)).Select(x => x.Id).ToList();
                    _documentStoreRepository.Delete(minutesTimeTablesToDelete);
                    //

                    foreach (var trip in joinedTrip.JoinedTrips)
                    {
                        var timeTableDataList = _documentStoreRepository.GetTimeTableDataByRouteId(trip.RouteId);
                        var day = timeTableDataList.FirstOrDefault(x => x.Date.DayOfWeek != DayOfWeek.Saturday || x.Date.DayOfWeek != DayOfWeek.Sunday);

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
                                minuteTimeTable.MinuteDictionary = _minuteTimeTableBuilder.Build(dayType, timeTableDataList, minuteTimeTable.MinuteDictionary, tripStop.StopId);
                            }

                            var minuteTimeTableIndex = minuteTimeTableList.FindIndex(x => x.Id == minuteTimeTable.Id);

                            if (minuteTimeTableIndex >= 0)
                                minuteTimeTableList[minuteTimeTableIndex] = minuteTimeTable;
                            else
                                minuteTimeTableList.Add(minuteTimeTable);

                            //jeśli nie jest nullem, porównać minuteTimeTableToSave wraz ze starym wynikiem
                            //jeśli się nie różnią, olać minuteTimeTableToSave
                            //jeśli się różnią LUB wynik z bazy jest nullem - zapisać w bazie
                        }
                    }

                    _documentStoreRepository.Delete(minuteTimeTableList.Select(x => x.Id).ToList());
                    _documentStoreRepository.Save(minuteTimeTableList);
                }
            }
        }
    }
}