using System.Collections.Generic;
using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.Service.PublicTransport.TimeTable;
using System.Timers;
using DoCeluNaCzasWebApi.Services.PublicTransport.TimeTable;

namespace DoCeluNaCzasWebApi.Services.UpdateService
{
    public static class UpdateTimeTableService
    {
        static Timer _timer;
        static TimeTableService _timeTableService;
        static MinuteTimeTableService _minuteTimeTableService;

        public static async void Init(TimeTableService timeTableService, MinuteTimeTableService minuteTimeTableService)
        {
            _timeTableService = timeTableService;
            _minuteTimeTableService = minuteTimeTableService;

            await _timeTableService.SetTimeTables();
            _minuteTimeTableService.SetMinuteTimeTables();

            SetTimer();
        }

        public static void SetTimer()
        {
            const int timeInMilliseconds = 43200000; //12 h
            _timer = new Timer(timeInMilliseconds);
            _timer.Elapsed += UpdateDataEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        static async void UpdateDataEvent(object source, ElapsedEventArgs e)
        {
            await _timeTableService.SetTimeTables();
        }

        public static MinuteTimeTable GetMinuteTimeTableByRouteIdAndStopId(int routeId, int stopId)
        {
            return _timeTableService.GetMinuteTimeTableByRouteIdAndStopId(routeId, stopId);
        }

        public static List<MinuteTimeTable> GetByBusLineName(string busLineName)
        {
            return _timeTableService.GetByBusLineName(busLineName);
        }
    }
}