using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Bussiness.PublicTransport.TimeTable;
using System.Collections.Generic;

namespace DCNC.Service.Database
{
    public interface IDocumentStoreRepository
    {
        void Save<T>(T objectToSave);
        void Save<T>(List<T> objectsToSave);
        void Delete(string idToDelete);
        void Delete(List<string> objectsIdToDelete);
        List<TimeTableJson> GetJsonsByRouteId(int routeId);
        List<TimeTableData> GetTimeTableDataByRouteId(int routeId);
        DbJson GetDbJson(JsonType type);
    }
}
