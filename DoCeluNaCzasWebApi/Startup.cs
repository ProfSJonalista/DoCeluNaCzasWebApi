using DCNC.Service.PublicTransport.UpdateService;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DoCeluNaCzasWebApi.Startup))]

namespace DoCeluNaCzasWebApi
{
    public partial class Startup
    {
        public async void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
            await UpdateDataService.Init();
            UpdateDataService.SetTimer();
        }
    }
}
