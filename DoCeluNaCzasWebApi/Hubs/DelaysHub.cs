using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using DoCeluNaCzasWebApi.Models.PublicTransport.Delay;
using DoCeluNaCzasWebApi.Services.Delays;
using Microsoft.AspNet.SignalR;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DoCeluNaCzasWebApi.Hubs
{
    public class DelaysHub : Hub
    {
        public static DelayService DelayService { get; set; }

        public async Task<ObservableCollection<DelayModel>> GetDelays(int stopId)
        {
            return await DelayService.GetDelays(stopId);
        }

        public ObservableCollection<ChooseBusStopModel> GetChooseBusStopModelObservableCollection()
        {
            return CacheService.GetData<ObservableCollection<ChooseBusStopModel>>(CacheKeys.CHOOSE_BUS_STOP_MODEL_OBSERVABALE_COLLECTION);
        }
    }
}