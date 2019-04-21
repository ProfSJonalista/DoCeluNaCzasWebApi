using DoCeluNaCzasWebApi.Models.PublicTransport.Delay;
using DoCeluNaCzasWebApi.Services.Delays;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DCNC.Service.PublicTransport.JsonData.Delays;
using Raven.Client.Documents.Operations.Attachments;

namespace DoCeluNaCzasWebApi.Hubs
{
    public class DelaysHub : Hub
    {
        public static DelayService DelayService { get; set; }

        public async Task<ObservableCollection<DelayModel>> GetDelays(int stopId)
        {
            return await DelayService.GetDelays(stopId);
        }
    }
}