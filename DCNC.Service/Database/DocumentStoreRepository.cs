using System.Collections.Generic;
using System.Linq;
using DCNC.Bussiness.PublicTransport.TimeTable;
using Raven.Client.Documents;

namespace DCNC.Service.Database
{
    public class DocumentStoreRepository
    {
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

        public void Delete(List<TimeTableJson> objectsToDelete)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                objectsToDelete.ForEach(item => session.Delete(item.Id));
                session.SaveChanges();
            }
        }
    }
}