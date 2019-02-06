using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.DataAccess.Helpers;
using DCNC.Service.PublicTransport.JsonData;
using DCNC.Service.PublicTransport.UpdateData;
using DoCeluNaCzasWebApi.Models.PublicTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace DoCeluNaCzasWebApi.Services.UpdateService
{
    public static class UDS
    {
        private static TimeService _timeService;
        private static TripService _tripService;
        private static BusStopService _busStopService;
        private static BusLineService _busLineService;
        private static ExpeditionService _expeditionService;
        private static StopInTripService _stopInTripService;

        private static Timer _timer;

        private static DateTime _lastTripsUpdate;
        private static DateTime _lastBusStopUpdate;
        private static DateTime _lastBusLineUpdate;
        private static DateTime _lastExpeditionUpdate;
        private static DateTime _lastStopsInTripUpdate;

        private static BusStopDataModel _busStopDataModel;
        private static List<JoinedTripsModel> _joinedTripsModelList;

        public static async Task Init()
        {
            InitializeServices();

            var tripsAsJObject = await _tripService.GetDataAsJObjectAsync(Urls.TRIPS);
            var busStopsAsJObject = await _busStopService.GetDataAsJObjectAsync(Urls.BUS_STOPS);
            var busLinesAsJObject = await _busLineService.GetDataAsJObjectAsync(Urls.BUS_LINES);
            var expeditionsAsJObject = await _expeditionService.GetDataAsJObjectAsync(Urls.EXPEDITION);
            var stopsInTripsAsJObject = await _stopInTripService.GetDataAsJObjectAsync(Urls.STOPS_IN_TRIPS);

            var tripDataList = _tripService.GetMappedListAndCacheData<TripData>(tripsAsJObject);
            var busStopDataList = _busStopService.GetMappedListAndCacheData<BusStopData>(busStopsAsJObject);
            var busLineDataList = _busLineService.GetMappedListAndCacheData<BusLineData>(busLinesAsJObject);
            var stopInTripDataList = _stopInTripService.GetMappedListAndCacheData<StopInTripData>(stopsInTripsAsJObject);
            var expeditionObject = _expeditionService.GetMappedListAndCacheData<ExpeditionData>(expeditionsAsJObject).FirstOrDefault();

            SetTimer();
            /*
             TODO ściągnąć dane (w formie list),
             przemapować dane na Modele (łącząc dane z list w jeden obiekt), 
             ustawić lastUpdates
            */
        }

        private static void InitializeServices()
        {
            _timeService = new TimeService();
            _tripService = new TripService();
            _busStopService = new BusStopService();
            _busLineService = new BusLineService();
            _expeditionService = new ExpeditionService();
            _stopInTripService = new StopInTripService();
        }

        public static void SetTimer()
        {
            const int timeInMilliseconds = 3600000; //1 hour
            _timer = new Timer(timeInMilliseconds);
            _timer.Elapsed += UpdateDataEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private static void UpdateDataEvent(object source, ElapsedEventArgs e)
        {
            /*
             TODO ściągnąć dane (w formie list)
             porównać daty lastUpdate
             JEŚLI daty się różnią - 
             przemapować dane na Modele (łącząc dane z list w jeden obiekt), 
             ustawić lastUpdates
             JEŚLI SIĘ NIE RÓŻNIĄ - nie robić nic
            */

        }

        public static List<JoinedTripsModel> GetJoinedTrips()
        {
            return _joinedTripsModelList;
        }

        public static BusStopDataModel GetBusStops()
        {
            return _busStopDataModel;
        }
    }
}