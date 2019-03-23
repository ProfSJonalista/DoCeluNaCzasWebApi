using DCNC.Service.PublicTransport.TimeTable;
using System.Timers;

namespace DoCeluNaCzasWebApi.Services.UpdateService
{
    public static class UpdateTimeTableService
    {
        private static Timer _timer;
        private static TimeTableService _timeTableService;

        public static async void Init()
        {
            _timeTableService = new TimeTableService();
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
    }
}