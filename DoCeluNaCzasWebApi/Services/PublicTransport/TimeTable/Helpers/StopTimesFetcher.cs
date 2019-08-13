using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.Service.Database.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.TimeTable.Helpers
{
    public class StopTimesFetcher
    {
        readonly DateChecker _dateChecker;
        readonly IDocumentStoreRepository _documentStoreRepository;

        public StopTimesFetcher(DateChecker dateChecker, IDocumentStoreRepository documentStoreRepository)
        {
            _dateChecker = dateChecker;
            _documentStoreRepository = documentStoreRepository;
        }

        public (List<StopTime>, List<StopTime>, List<StopTime>) FetchStopTimes(List<int> routeIds)
        {
            var combinedTimeTableDatas = _documentStoreRepository.GetTimeTableDataByRouteId(routeIds);
            var dates = combinedTimeTableDatas.Select(x => x.Date).Distinct().ToList();
            var (weekday, saturday, sunday) = _dateChecker.GetDates(dates);

            var weekdayTtds = combinedTimeTableDatas.Where(x => x.Date.Date == weekday.Date).ToList();
            var saturdayTtds = combinedTimeTableDatas.Where(x => x.Date.Date == saturday.Date).ToList();
            var sundayTtds = combinedTimeTableDatas.Where(x => x.Date.Date == sunday.Date).ToList();

            var weekDayCombinedStopTimes = weekdayTtds.SelectMany(x => x.StopTimes).ToList();
            var saturdayCombinedStopTimes = saturdayTtds.SelectMany(x => x.StopTimes).ToList();
            var sundayCombinedStopTimes = sundayTtds.SelectMany(x => x.StopTimes).ToList();

            return (weekDayCombinedStopTimes, saturdayCombinedStopTimes, sundayCombinedStopTimes);
        }
    }
}