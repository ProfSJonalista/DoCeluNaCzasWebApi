using System;
using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Bussiness.PublicTransport.TimeTable;
using System.Collections.Generic;
using System.Linq;
using DCNC.Bussiness.PublicTransport.General;
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

                objectsToSave.ForEach(item =>
                {
                    if (item != null) session.Store(item);
                });

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

                objectsIdToDelete.ForEach(item =>
                {
                    if (item != null) session.Delete(item);
                });
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

        #region BusStopDataModel

        public void UpdateBusStopDataModel(BusStopDataModel busStopDataModel)
        {
            if(busStopDataModel == null) 
                return;

            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                var id = session.Query<BusStopDataModel>().FirstOrDefault()?.Id;

                if (string.IsNullOrEmpty(id))
                {
                    Save(busStopDataModel);
                    return;
                }

                var bsdm = session.Load<BusStopDataModel>(id);

                bsdm.Day = busStopDataModel.Day;
                bsdm.LastUpdate = busStopDataModel.LastUpdate;
                bsdm.Stops = busStopDataModel.Stops;

                session.SaveChanges();
            }
        }

        public BusStopDataModel GetBusStopDataModel()
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                return session.Query<BusStopDataModel>().FirstOrDefault();
            }
        }

        #endregion

        #region JoinedTrips

        public void UpdateGroupedJoinedModels(List<GroupedJoinedModel> groupedJoinedModels)
        {
            if (groupedJoinedModels == null || groupedJoinedModels.Count == 0)
                return;

            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                var gjm = session.Query<GroupedJoinedModel>().ToList();

                if (gjm.Count == 0)
                {
                    Save(groupedJoinedModels);
                    return;
                }

                var busId = gjm.First(x => x.Group == Group.Buses).Id;
                var buses = session.Load<GroupedJoinedModel>(busId);
                var newBuses = groupedJoinedModels.First(x => x.Group == Group.Buses);

                buses.JoinedTripModels = newBuses.JoinedTripModels;

                var tramId = gjm.First(x => x.Group == Group.Trams).Id;
                var trams = session.Load<GroupedJoinedModel>(tramId);
                var newTrams = groupedJoinedModels.First(x => x.Group == Group.Trams);

                trams.JoinedTripModels = newTrams.JoinedTripModels;

                var trolleyId = gjm.First(x => x.Group == Group.Buses).Id;
                var trolleys = session.Load<GroupedJoinedModel>(trolleyId);
                var newTrolleys = groupedJoinedModels.First(x => x.Group == Group.Buses);

                trolleys.JoinedTripModels = newTrolleys.JoinedTripModels;

                session.SaveChanges();
            }
        }

        public List<GroupedJoinedModel> GetGroupedJoinedModels()
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                return session.Query<GroupedJoinedModel>().ToList();
            }
        }

        #endregion

        #endregion
    }
}