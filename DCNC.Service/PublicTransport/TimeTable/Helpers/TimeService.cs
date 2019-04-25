using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers
{
    public class TimeService
    {
        public List<StopTimeUrl> FilterStopTimesByDate(List<StopTimeUrl> convertedStopTimes)
        {
            foreach (var stopTime in convertedStopTimes)
            {
                stopTime.Urls = stopTime.Urls
                    .Select(url => new { url, dt = GetDateFromUrl(url) })
                    .Where(x => x.dt.Date >= DateTime.Today)
                    .OrderBy(x => x.dt)
                    .Select(x => x.url)
                    .ToList();
            }

            return convertedStopTimes;
        }

        public DateTime GetDateFromUrl(string url)
        {
            return DateTime.Parse(url.Substring(38, 10));
        }
    }
}