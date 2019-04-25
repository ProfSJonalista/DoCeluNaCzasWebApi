using DCNC.DataAccess.PublicTransport;
using DCNC.Service.Database;
using DCNC.Service.PublicTransport.JoiningTrips;
using DCNC.Service.PublicTransport.JoiningTrips.Helpers;
using DCNC.Service.PublicTransport.JsonData.Delays;
using DCNC.Service.PublicTransport.JsonData.General;
using DCNC.Service.PublicTransport.JsonData.TimeTable;
using DCNC.Service.PublicTransport.TimeTable;
using DCNC.Service.PublicTransport.TimeTable.Helpers;
using DoCeluNaCzasWebApi.Hubs;
using DoCeluNaCzasWebApi.Services.Delays;
using DoCeluNaCzasWebApi.Services.PublicTransport;
using DoCeluNaCzasWebApi.Services.UpdateService;
using DoCeluNaCzasWebApi.Services.UpdateService.Helpers;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(DoCeluNaCzasWebApi.Startup))]

namespace DoCeluNaCzasWebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration
            {
                IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always // Add this line to enable detail mode in release
            };

            WebApiConfig.Register(config);
            app.UseWebApi(config);
            //TODO - fix auth - UserManager appears to be null
            //ConfigureAuth(app);

            var hubConfiguration = new HubConfiguration {EnableDetailedErrors = true};
            app.MapSignalR(hubConfiguration);

            ConfigureServices();
        }

        void ConfigureServices()
        {
            var documentStoreRepository = new DocumentStoreRepository();
            var publicTransportRepository = new PublicTransportRepository();

            var delayJsonService = new DelayJsonService(publicTransportRepository);
            DelaysHub.DelayService = new DelayService(delayJsonService);
            
            var timeService = new DCNC.Service.PublicTransport.Time.TimeService();

            var stopComparer = new StopComparer();
            var combineHelper = new CombineHelper(stopComparer);
            var combiner = new Combiner(combineHelper);
            var combineTripService = new CombineTripService(combiner);

            var joinTripMappingService = new JoinTripMappingService();

            var organizer = new Organizer();
            var stopHelper = new StopHelper();
            var tripsWithBusStopsService = new TripsWithBusStopsService(organizer, stopHelper);
            var joiner = new Joiner(combineTripService,joinTripMappingService, tripsWithBusStopsService);

            var grouper = new Grouper();

            var tripService = new TripService(documentStoreRepository, publicTransportRepository);
            var busStopService = new BusStopService(documentStoreRepository, publicTransportRepository);
            var busLineService = new BusLineService(documentStoreRepository, publicTransportRepository);
            var expeditionService = new ExpeditionService(documentStoreRepository, publicTransportRepository);
            var stopInTripService = new StopInTripService(documentStoreRepository, publicTransportRepository);
            var busStopModelService = new BusStopModelService();

            var updateServiceHelper = new UpdateServiceHelper(joiner, grouper, timeService,
            tripService, busStopService, busLineService, expeditionService, stopInTripService, busStopModelService);
            UpdateDataService.Init(timeService, updateServiceHelper);

            var converter = new Converter();
            var filterHelper = new FilterHelper();
            var helperTimeService = new TimeService();
            var convertingHelper = new ConvertingHelper(converter, filterHelper, documentStoreRepository);
            var stopTimesService = new StopTimesService(documentStoreRepository, publicTransportRepository);
            var downloadHelper = new DownloadHelper(documentStoreRepository, helperTimeService, publicTransportRepository);
            var timeTableService = new TimeTableService(documentStoreRepository, helperTimeService, convertingHelper, stopTimesService, downloadHelper);

            UpdateTimeTableService.Init(timeTableService);
        }
    }
}
