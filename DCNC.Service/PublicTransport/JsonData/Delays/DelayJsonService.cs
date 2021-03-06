﻿using DCNC.Bussiness.PublicTransport.Delays;
using DCNC.DataAccess.PublicTransport;
using Newtonsoft.Json;
using System.Threading.Tasks;
using DCNC.DataAccess.PublicTransport.Interfaces;

namespace DCNC.Service.PublicTransport.JsonData.Delays
{
    public class DelayJsonService
    {
        readonly IPublicTransportRepository _publicTransportRepository;

        public DelayJsonService(IPublicTransportRepository publicTransportRepository)
        {
            _publicTransportRepository = publicTransportRepository;
        }

        public async Task<DelayData> GetData(string url)
        {
            var json = await _publicTransportRepository.DownloadData(url);
            return JsonConvert.DeserializeObject<DelayData>(json);
        }
    }
}