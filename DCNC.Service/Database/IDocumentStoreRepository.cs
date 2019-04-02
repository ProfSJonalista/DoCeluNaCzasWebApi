using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Bussiness.PublicTransport.TimeTable;

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
