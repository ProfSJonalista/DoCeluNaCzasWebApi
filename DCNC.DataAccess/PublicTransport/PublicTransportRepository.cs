﻿using DCNC.DataAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace DCNC.DataAccess.PublicTransport
{
    public class PublicTransportRepository
    {
        public async static Task<string> GetBusStops()
        {
            return await DownloadData(Constants.BUS_STOPS);
        }

        public async static Task<string> GetAgencies()
        {
            return await DownloadData(Constants.AGENCIES);
        }

        public async static Task<string> GetBusLines()
        {
            return await DownloadData(Constants.BUS_LINES);
        }

        public async static Task<string> GetTrips()
        {
            return await DownloadData(Constants.TRIPS);
        }

        public async static Task<string> GetStopsInTrips()
        {
            return await DownloadData(Constants.STOPS_IN_TRIPS);
        }

        public async static Task<string> GetExpeditionData()
        {
            return await DownloadData(Constants.EXPEDITION);
        }

        private async static Task<string> DownloadData(string url)
        {
            var data = "";
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                data = json;
            }

            return data;
        }
    }
}