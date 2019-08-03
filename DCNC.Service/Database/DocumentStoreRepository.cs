using System;
using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Bussiness.PublicTransport.TimeTable;
using System.Collections.Generic;
using System.Linq;
using DCNC.Bussiness.PublicTransport.JoiningTrips;
using DCNC.Service.Database.Interfaces;

namespace DCNC.Service.Database
{
    public class DocumentStoreRepository : IDocumentStoreRepository
    {
        #region SaveEntities
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
                if (objectsToSave.Count <= 0) return;

                objectsToSave.ForEach(item => { if (item != null) session.Store(item); });
                session.SaveChanges();
            }
        }
        #endregion

        #region DeleteEntities
        public void Delete(string idToDelete)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                session.Delete(idToDelete);
                session.SaveChanges();
            }
        }

        public void Delete(List<string> objectsIdToDelete)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                if (objectsIdToDelete.Count <= 0) return;

                objectsIdToDelete.ForEach(item => { if (item != null) session.Delete(item); });
                session.SaveChanges();
            }
        }
        #endregion

        #region GetEntities

        #region TimeTableJson

        public List<TimeTableJson> GetJsonsByRouteId(int routeId)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                return session.Query<TimeTableJson>()
                    .Where(x => x.RouteId == routeId)
                    .ToList();
            }
        }

        public void DeleteAllTimeTableJsons()
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                var ids = session.Query<TimeTableJson>().Select(x => x.Id).ToList();
                Delete(ids);
            }
        }

        #endregion

        #region MinuteTimeTable

        public List<MinuteTimeTable> GetMinuteTimeTableListByBusLineName(string busLineName)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                return session.Query<MinuteTimeTable>()
                    .Where(x => x.BusLineName.Equals(busLineName))
                    .ToList();
            }
        }

        public MinuteTimeTable GetMinuteTimeTableByRouteIdAndStopId(int routeId, int stopId)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                return session.Query<MinuteTimeTable>()
                    .SingleOrDefault(x => x.StopId == stopId && x.RouteIds.Any(y => y == routeId));
            }
        }

        #endregion

        #region TimeTableData

        public List<TimeTableData> GetTimeTableDataByRouteId(int routeId)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                return session.Query<TimeTableData>()
                    .Where(x => x.RouteId == routeId)
                    .ToList();
            }
        }

        public List<TimeTableData> GetTimeTableDataByRouteId(List<int> routeIds)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                var listToReturn = new List<TimeTableData>();

                foreach (var id in routeIds)
                {
                    listToReturn.AddRange(session.Query<TimeTableData>()
                        .Where(x => x.RouteId == id)
                        .ToList());
                }

                return listToReturn;
            }
        }

        public TimeTableData GetTimeTableDataByRouteIdAndDayOfWeek(int routeId, DayOfWeek dayOfWeek)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                return session.Query<TimeTableData>()
                    .FirstOrDefault(x => x.RouteId == routeId && x.Date.DayOfWeek == dayOfWeek);
            }
        }

        #endregion

        #region DbJson

        public DbJson GetDbJson(JsonType type)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                return session.Query<DbJson>().FirstOrDefault(x => x.Type == type);
            }
        }

        #endregion

        #region TripsWithBusStops

        public void DeleteTripsWithBusStops()
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                var ids = session.Query<TripsWithBusStops>().Select(x => x.Id).ToList();
                Delete(ids);
            }
        }

        public TripsWithBusStops GetTripsByDayOfWeek(DayOfWeek dayOfWeek)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                return session.Query<TripsWithBusStops>().FirstOrDefault(x => x.Day.DayOfWeek == dayOfWeek);
            }
        }

        #endregion

        #endregion
    }
}