using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DoCeluNaCzasWebApi.Models.PublicTransport.Delay;
using DoCeluNaCzasWebApi.Services.Delays;
using Microsoft.AspNet.SignalR;

namespace DoCeluNaCzasWebApi.Hubs
{
    public class DelaysHub : Hub
    {
        private readonly DelayService _delayService;

        public DelaysHub()
        {
            _delayService = new DelayService();
        }

        public async Task<List<DelayModel>> GetDelays(int stopId)
        {
            return await _delayService.GetDelays(stopId);
        }
    }
}