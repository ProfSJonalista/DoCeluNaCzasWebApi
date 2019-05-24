using PublicHoliday;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.TimeTable.Helpers
{
    public class DateChecker
    {
        readonly PolandPublicHoliday _polandPublicHoliday;

        public DateChecker(PolandPublicHoliday polandPublicHoliday)
        {
            _polandPublicHoliday = polandPublicHoliday;
        }
        //todo checking for holidays and shit
        public (DateTime, DateTime, DateTime) GetDates(List<DateTime> dates)
        {
            var weekday = dates.FirstOrDefault(x => (int)x.DayOfWeek != (int)DayOfWeek.Saturday && (int)x.DayOfWeek != (int)DayOfWeek.Sunday);
            var saturday = dates.FirstOrDefault(x => (int)x.DayOfWeek == (int)DayOfWeek.Saturday);
            var sunday = dates.FirstOrDefault(x => (int)x.DayOfWeek == (int)DayOfWeek.Sunday);

            return (weekday, saturday, sunday);
        }
    }
}