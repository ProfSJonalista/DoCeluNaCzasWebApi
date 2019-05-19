using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DCNC.Bussiness.PublicTransport.TimeTable;
using PublicHoliday;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.TimeTable.Helpers
{
    public class DateChecker
    {
        private readonly PolandPublicHoliday _polandPublicHoliday;

        public DateChecker(PolandPublicHoliday polandPublicHoliday)
        {
            _polandPublicHoliday = polandPublicHoliday;
        }

        public TimeTableData GetProperDate(DayType dayType, List<TimeTableData> timeTableDataList)
        {
            switch (dayType)
            {
                case DayType.Weekday:
                    return GetWeekday(timeTableDataList);
                case DayType.Saturday:
                    return GetSaturday(timeTableDataList);
                case DayType.SundayAndHoliday:
                    return GetSundayAndHoliday(timeTableDataList);
                default:
                    return new TimeTableData();
            }
        }

        private TimeTableData GetWeekday(List<TimeTableData> timeTableDataList)
        {
            if (timeTableDataList.Count <= 0) return new TimeTableData();

            var firstElement = timeTableDataList.FirstOrDefault();

            if (firstElement == null) return new TimeTableData();

            DateTime dateTime;

            switch (firstElement.Date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    dateTime = firstElement.Date.AddDays(2);
                    break;
                case DayOfWeek.Sunday:
                    dateTime = firstElement.Date.AddDays(1);
                    break;
                default:
                    dateTime = firstElement.Date;
                    break;
            }

            var elementToReturn = timeTableDataList.SingleOrDefault(x => x.Date.Date == dateTime.Date);
            //todo check for holidays
            return elementToReturn;
        }

        private TimeTableData GetSaturday(List<TimeTableData> timeTableDataList)
        {
            var day = timeTableDataList.FirstOrDefault(x => x.Date.DayOfWeek == DayOfWeek.Saturday);
            //todo check for holidays
            return day;
        }

        private TimeTableData GetSundayAndHoliday(List<TimeTableData> timeTableDataList)
        {
            var sunday = timeTableDataList.FirstOrDefault(x => x.Date.DayOfWeek == DayOfWeek.Sunday);

            if (sunday != null)
                return sunday;
            //todo fix code below
            //var firstElement = timeTableDataList.FirstOrDefault();
            //var lastElement = timeTableDataList.LastOrDefault();

            //if (firstElement != null && lastElement != null)
            //{
            //    var holidayDates = _polandPublicHoliday.GetHolidaysInDateRange(firstElement.Date, lastElement.Date);

            //    if (holidayDates != null && holidayDates.Count > 0)
            //    {
            //        var firstHoliday = holidayDates.FirstOrDefault();
            //        var dateToReturn = timeTableDataList.FirstOrDefault(x => x.Date.Date == firstHoliday.ObservedDate.Date);
            //        return dateToReturn;
            //    }
            //}

            return new TimeTableData { StopTimes = new List<StopTime>() };
        }
    }
}