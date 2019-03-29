using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.Service.PublicTransport.TimeTable;
using System.Collections.Generic;
using System.Timers;
using DCNC.Service.Database;

namespace DoCeluNaCzasWebApi.Services.UpdateService
{
    public static class UpdateTimeTableService
    {
        private static Timer _timer;
        private static TimeTableService _timeTableService;

        public static async void Init(DocumentStoreRepository dsr)
        {
            _timeTableService = new TimeTableService(dsr);
            await _timeTableService.SetTimeTables();

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

        private static async void UpdateDataEvent(object source, ElapsedEventArgs e)
        {
            await _timeTableService.SetTimeTables();
        }

        public static List<TimeTableData> GetTimeTableData(int routeId)
        {
            return _timeTableService.GetTimeTableDataByRouteId(routeId);
        }
    }
}