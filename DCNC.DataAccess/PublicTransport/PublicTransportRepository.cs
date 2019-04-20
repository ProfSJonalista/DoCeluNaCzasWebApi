﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DCNC.DataAccess.PublicTransport
{
    public class PublicTransportRepository : IPublicTransportRepository
    {
        private static readonly HttpClient Client = new HttpClient();

        public async Task<string> DownloadData(string url)
        {
            try
            {
                return await Client.GetStringAsync(url);
            }
            catch (HttpRequestException httpRequestException)
            {
                return string.Empty;
            }
            catch (Exception exception)
            {
                return string.Empty;
            }
        }

        public async Task<string> DownloadData(string url, HttpClient client)
        {
            try
            {
                return await client.GetStringAsync(url);
            }
            catch (HttpRequestException httpRequestException)
            {
                return string.Empty;
            }
            catch (Exception exception)
            {
                return string.Empty;
            }
        }
    }
}