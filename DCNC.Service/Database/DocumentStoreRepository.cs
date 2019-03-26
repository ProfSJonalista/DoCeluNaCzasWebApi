using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.Bussiness.PublicTransport.TimeTable.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Sparrow.Json;

namespace DCNC.Service.Database
{
    public class DocumentStoreRepository
    {
        public void Save<T>(T objectToSave)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                session.Store(objectToSave);
                session.SaveChanges();
            }
        }

        public void Save<T>(List<T> objectsToSave)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                objectsToSave.ForEach(item => session.Store(item));
                session.SaveChanges();
            }
        }

        public List<TimeTableJson> GetJsonsByRouteId(int routeId)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                return session.Query<TimeTableJson>()
                    .Where(x => x.RouteId == routeId)
                    .ToList();
            }
        }

        public List<TimeTableData> GetTimeTableDataByRouteId(int routeId)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                return session.Query<TimeTableData>()
                    .Where(x => x.RouteId == routeId)
                    .ToList();
            }
        }

        public void Delete(List<Common> objectsToDelete)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                objectsToDelete.ForEach(item => session.Delete(item.Id));
                session.SaveChanges();
            }
        }
    }
}