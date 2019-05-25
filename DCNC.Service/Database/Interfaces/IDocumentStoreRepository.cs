﻿using System.Collections.Generic;
using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Bussiness.PublicTransport.TimeTable;

namespace DCNC.Service.Database.Interfaces
{
    public interface IDocumentStoreRepository
    {
        void Save<T>(T objectToSave);
        void Save<T>(List<T> objectsToSave);
        void Delete(string idToDelete);
        void Delete(List<string> objectsIdToDelete);
        List<TimeTableJson> GetJsonsByRouteId(int routeId);
        MinuteTimeTable GetMinuteTimeTableByRouteIdAndStopId(int routeId, int stopId);
        List<MinuteTimeTable> GetMinuteTimeTableListByBusLineName(string busLineName);
        List<TimeTableData> GetTimeTableDataByRouteId(int routeId);
        List<TimeTableData> GetTimeTableDataByRouteId(List<int> routeIds);
        DbJson GetDbJson(JsonType type);
    }
}