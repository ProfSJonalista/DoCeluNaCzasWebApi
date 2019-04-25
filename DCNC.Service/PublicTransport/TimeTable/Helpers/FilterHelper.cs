using DCNC.Bussiness.PublicTransport.TimeTable;
using System.Collections.Generic;
using System.Linq;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers
{
    public class FilterHelper
    {
        public List<TimeTableData> Filter(List<TimeTableData> entitiesToDelete, List<TimeTableDateTime> notDownloadedEntitiesByRouteId)
        {
            return entitiesToDelete.Where(entity => entity.StopTimes.Any(stop => notDownloadedEntitiesByRouteId.All(x => x.Date.DayOfWeek != stop.Date.DayOfWeek))).ToList();
        }
    }
}