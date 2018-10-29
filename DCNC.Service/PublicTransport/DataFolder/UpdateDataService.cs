using DCNC.Bussiness.PublicTransport;
using DCNC.Service.PublicTransport.TimeTable;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Timers;
using System.Web;


namespace DCNC.Service.PublicTransport.DataFolder
{
    public class UpdateDataService
    {
        static Timer _timer;
        static TripData _trips;
        static BusLineData _busLines;
        static BusStopData _busStops;
        static StopInTripData _stopsInTrips;
        static ExpeditionData _expeditionData;

        public async static Task Init()
        {
            await DownloadData();
            await AllocateData();
            
            Data.TripsWithBusStops = TripService.TripsWithBusStopsMapper(Data.BusLineData, Data.TripData, Data.StopInTripData, Data.ExpeditionData, Data.BusStopData);
            Data.JoinedTrips = JoinTripService.JoinTrips(Data.BusLineData, Data.TripData, Data.StopInTripData, Data.ExpeditionData, Data.BusStopData, Data.TripsWithBusStops);
            Data.JoinedTripsAsJson = JsonConvert.SerializeObject(Data.JoinedTrips);
        }

        //co określony okres czasu sprawdza czy nie zostały zaktualizowane dane na zewnętrznym API - jeśli tak, aktualizuje je
        public static void SetTimer()
        {
            var timeInMilliseconds = 3600000; //1 godzina
            _timer = new Timer(timeInMilliseconds);
            _timer.Elapsed += UpdateDataEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private static void UpdateDataEvent(Object source, ElapsedEventArgs e)
        {
            if (Data.BusLineData != null && Data.BusStopData != null && Data.ExpeditionData != null
                && Data.TripData != null && Data.StopInTripData != null)
            {

                var hasUpdates = CheckForUpdates();
                if (hasUpdates)
                    UpdateJoinedTrips();
            }
        }

        private static void UpdateJoinedTrips()
        {
            Data.TripsWithBusStops = TripService.TripsWithBusStopsMapper(Data.BusLineData, Data.TripData, Data.StopInTripData, Data.ExpeditionData, Data.BusStopData);
            Data.JoinedTrips = JoinTripService.JoinTrips(Data.BusLineData, Data.TripData, Data.StopInTripData, Data.ExpeditionData, Data.BusStopData, Data.TripsWithBusStops);
            Data.JoinedTripsAsJson = JsonConvert.SerializeObject(Data.JoinedTrips);
        }

        public static async Task DownloadData()
        {
            _trips = await TripService.GetTripData();
            _busLines = await BusLineService.GetBusLineData();
            _busStops = await BusStopService.GetBusStopData();
            _stopsInTrips = await StopInTripService.GetStopInTripData();
            _expeditionData = await ExpeditionService.GetExpeditionData();
        }

        public async static Task AllocateData()
        {
            Data.TripData = _trips;
            Data.BusLineData = _busLines;
            Data.BusStopData = _busStops;
            Data.StopInTripData = _stopsInTrips;
            Data.ExpeditionData = _expeditionData;
        }

        public static bool CheckForUpdates()
        {

            if (Data.TripData.LastUpdate < _trips.LastUpdate)
                return true;
            if (Data.BusLineData.LastUpdate < _busLines.LastUpdate)
                return true;
            if (Data.BusStopData.LastUpdate < _busStops.LastUpdate)
                return true;
            if (Data.StopInTripData.LastUpdate < _stopsInTrips.LastUpdate)
                return true;
            if (Data.ExpeditionData.LastUpdate < _expeditionData.LastUpdate)
                return true;

            return false;
        }

        public async static Task<string> GetActualJoinedTrips(bool hasData)
        {
            await DownloadData();
            var hasUpdates = CheckForUpdates();
            
            if (hasUpdates)
            {
                await AllocateData();

                Data.TripsWithBusStops = TripService.TripsWithBusStopsMapper(Data.BusLineData, Data.TripData, Data.StopInTripData, Data.ExpeditionData, Data.BusStopData);
                Data.JoinedTrips = JoinTripService.JoinTrips(Data.BusLineData, Data.TripData, Data.StopInTripData, Data.ExpeditionData, Data.BusStopData, Data.TripsWithBusStops);

                Data.JoinedTripsAsJson = JsonConvert.SerializeObject(Data.JoinedTrips);

                return Data.JoinedTripsAsJson;
            }
            else if (!hasData)
            {
                return Data.JoinedTripsAsJson;
            }
            else if (!hasUpdates && hasData)
            {
                //jeśli użytkownik końcowy, na swoim urządzeniu, ma pobrane dane i są one aktualne, zwracany jest pusty string w celu zmniejszenia zużycia danych
                return "";
            }

            return Data.JoinedTripsAsJson;
        }
    }
}
