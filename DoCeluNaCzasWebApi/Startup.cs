using DCNC.Service.Database;
using DoCeluNaCzasWebApi.Services.UpdateService;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

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
            app.MapSignalR();

            var dsr = new DocumentStoreRepository();
            UpdateDataService.Init(dsr);
            UpdateTimeTableService.Init(dsr);
        }
    }
}
