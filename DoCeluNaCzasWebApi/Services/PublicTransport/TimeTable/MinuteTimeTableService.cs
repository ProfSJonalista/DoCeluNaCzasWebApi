﻿using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using DCNC.Service.Database;
using DoCeluNaCzasWebApi.Models.PublicTransport.General;
using DoCeluNaCzasWebApi.Services.PublicTransport.TimeTable.Helpers;
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
                foreach (var joinedTripModel in group.JoinedTripModels)
                {
                    var routeIds = joinedTripModel.JoinedTrips.SelectMany(x => x.Stops.Select(y => y.RouteId)).Distinct().ToList();
                    var minuteTimeTableList = _documentStoreRepository.GetMinuteTimeTableListByBusLineName(joinedTripModel.BusLineName);
                    var existingStopIds = joinedTripModel.JoinedTrips.SelectMany(x => x.Stops.Select(y => y.StopId).ToList()).Distinct().OrderBy(x => x).ToList();
                    var minuteTimeTableToDeleteList = minuteTimeTableList.Where(x => existingStopIds.All(y => y != x.StopId)).Select(x => x.Id).ToList();
                    _documentStoreRepository.Delete(minuteTimeTableToDeleteList);

                    minuteTimeTableList = _minuteTimeTableBuilder.BuildList(minuteTimeTableList, routeIds, joinedTripModel.JoinedTrips);

                    _documentStoreRepository.Delete(minuteTimeTableList.Select(x => x.Id).ToList());
                    _documentStoreRepository.Save(minuteTimeTableList);
                }
            }
        }
    }
}