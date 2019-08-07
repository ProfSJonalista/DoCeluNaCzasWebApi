using DCNC.Bussiness.PublicTransport.Delays;
using DoCeluNaCzasWebApi.Services.Delays;
using Microsoft.AspNet.SignalR;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace DoCeluNaCzasWebApi.Hubs
{
    [HubName("DelaysHub")]
    public class DelaysHub : Hub
    {
        public static DelayService DelayService { get; set; }

        [HubMethodName("GetDelays")]
        public async Task<ObservableCollection<DelayModel>> GetDelays(int stopId)
        {
            return await DelayService.GetDelays(stopId);
        }
    }
}