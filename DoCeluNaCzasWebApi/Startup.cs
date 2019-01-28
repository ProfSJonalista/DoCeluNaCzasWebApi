using DCNC.Service.PublicTransport.UpdateService;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(DoCeluNaCzasWebApi.Startup))]

namespace DoCeluNaCzasWebApi
{
    public partial class Startup
    {
        public async void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration
            {
                IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always // Add this line to enable detail mode in release
            };

            WebApiConfig.Register(config);
            app.UseWebApi(config);
            ConfigureAuth(app);
            app.MapSignalR();
            //UpdateDataService updateDataService = new UpdateDataService();
            await UpdateDataService.Init();
            UpdateDataService.SetTimer();
        }
    }
}
