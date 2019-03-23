using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers
{
    public class TimeService
    {
        public List<StopTimeUrl> FilterStopTimesByDate(List<StopTimeUrl> convertedStopTimes)
        {
            convertedStopTimes.ForEach(stopTime =>
            {
                stopTime.Urls = stopTime.Urls
                    .Select(url => new { url, dt = GetDateFromUrl(url) })
                    .Where(x => x.dt.Date >= DateTime.Now.Date)
                    .OrderBy(x => x.dt)
                    .Select(x => x.url)
                    .Take(7)
                    .ToList();
            });

            return convertedStopTimes;
        }

        public DateTime GetDateFromUrl(string url)
        {
            return DateTime.Parse(url.Substring(38, 10));
        }
    }
}