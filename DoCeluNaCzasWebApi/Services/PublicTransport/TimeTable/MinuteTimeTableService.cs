using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using DCNC.Service.Database.Interfaces;
using DoCeluNaCzasWebApi.Services.PublicTransport.TimeTable.Helpers;
using System.Collections.Generic;
using System.Linq;
using DCNC.Bussiness.PublicTransport.JoiningTrips;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.TimeTable
{
    public class MinuteTimeTableService
    {
        readonly MinuteTimeTableBuilder _minuteTimeTableBuilder;
        readonly IDocumentStoreRepository _documentStoreRepository;

        public MinuteTimeTableService(MinuteTimeTableBuilder minuteTimeTableBuilder, IDocumentStoreRepository documentStoreRepository)
        {
            _minuteTimeTableBuilder = minuteTimeTableBuilder;
            _documentStoreRepository = documentStoreRepository;
        }

        public void SetMinuteTimeTables()
        {
            var groupedJoinedTrips = _documentStoreRepository.GetGroupedJoinedModels();

            foreach (var group in groupedJoinedTrips)
            {
                foreach (var joinedTripModel in group.JoinedTripModels)
                {
                    var routeIds = joinedTripModel.JoinedTrips.SelectMany(x => x.Stops.Select(y => y.RouteId)).Distinct().ToList();
                    var minuteTimeTableList = _documentStoreRepository.GetMinuteTimeTableListByBusLineName(joinedTripModel.BusLineName);
                    var existingStopIds = joinedTripModel.JoinedTrips.SelectMany(x => x.Stops.Select(y => y.StopId).ToList()).Distinct().OrderBy(x => x).ToList();
                    var minuteTimeTableToDeleteList = minuteTimeTableList.Where(x => existingStopIds.All(y => y != x.StopId)).Select(x => x.Id).ToList();
                    minuteTimeTableList = minuteTimeTableList.Where(x => minuteTimeTableToDeleteList.All(y => !y.Equals(x.Id))).ToList();

                    _documentStoreRepository.Delete(minuteTimeTableToDeleteList);

                    minuteTimeTableList = _minuteTimeTableBuilder.BuildList(minuteTimeTableList, routeIds, joinedTripModel.JoinedTrips);

                    _documentStoreRepository.Delete(minuteTimeTableList.Select(x => x.Id).ToList());
                    _documentStoreRepository.Save(minuteTimeTableList);
                }
            }
        }
    }
}