﻿using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
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
                    //.Take(7)
                    .ToList();
            }

            return convertedStopTimes;
        }

        public DateTime GetDateFromUrl(string url)
        {
            var startIndex = url.IndexOf('=') + 1;
            return DateTime.Parse(url.Substring(startIndex, 10));
        }
    }
}