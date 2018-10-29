using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DCNC.Service.PublicTransport.DataFolder;
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

            await UpdateDataService.Init();
            UpdateDataService.SetTimer();
        }
    }
}
