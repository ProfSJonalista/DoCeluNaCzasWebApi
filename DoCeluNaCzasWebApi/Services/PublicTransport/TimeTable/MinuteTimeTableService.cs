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
        private IDocumentStoreRepository _documentStoreRepository;

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
                    foreach (var trip in joinedTrip.JoinedTrips)
                    {
                        var timeTableData = _documentStoreRepository.GetTimeTableDataByRouteId(trip.RouteId);
                        //jeśli to weekday lub sobota, sprawdzić czy nie równa się niedzieli (i świętom)
                        //jeśli weekday równa się niedzieli, pobrać inny dzień (inny z tego samego tygodnia/po nazwie dnia tygodnia) i ponownie sprawdzić

                        foreach (var tripStop in trip.Stops)
                        {

                            var minuteTimeTable =
                                _documentStoreRepository.GetTimeTableDataByRouteIdAndStopId(tripStop.RouteId, tripStop.StopId)
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
                                for (var hour = 0; hour < 24; hour++)
                                {
                                    var stopTimesByHour = day.StopTimes.Where(x => x.StopId == tripStop.StopId && x.DepartureTime.Hour == hour).ToList();
                                    //dla i = godzina ze StopTimes, pobrać WSZYSTKIE wyniki dla tej godziny
                                }
                            }

                            //order by ascending
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