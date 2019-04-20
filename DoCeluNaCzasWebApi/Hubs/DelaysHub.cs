using DoCeluNaCzasWebApi.Models.PublicTransport.Delay;
using DoCeluNaCzasWebApi.Services.Delays;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using DCNC.Service.PublicTransport.JsonData.Delays;

namespace DoCeluNaCzasWebApi.Hubs
{
    public class DelaysHub : Hub
    {
        private readonly DelayService _delayService;

        public DelaysHub()
        {
            _delayService = new DelayService(new DelayJsonService());
        }

        public async Task<List<DelayModel>> GetDelays(int stopId)
        {
            return await _delayService.GetDelays(stopId);
        }
    }
}