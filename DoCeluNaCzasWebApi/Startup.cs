using Microsoft.Owin;
using Owin;
using System.Web.Http;
using DoCeluNaCzasWebApi.Services.UpdateService;

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
            //TODO - fix auth - UserManager appears to be null
            //ConfigureAuth(app);
            app.MapSignalR();
            
            await UpdateDataService.Init();
        }
    }
}
