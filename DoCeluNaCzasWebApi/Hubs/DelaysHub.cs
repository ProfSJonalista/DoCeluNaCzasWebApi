using DCNC.Bussiness.PublicTransport.Delays;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DCNC.Bussiness.PublicTransport.RouteSearch;
using DCNC.Service.PublicTransport.Delays;

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

        [HubMethodName("GetOneDelay")]
        public async Task<DelayModel> GetOneDelay(StopChange stopChange)
        {
            return await DelayService.GetOneDelay(stopChange);
        }
    }
}