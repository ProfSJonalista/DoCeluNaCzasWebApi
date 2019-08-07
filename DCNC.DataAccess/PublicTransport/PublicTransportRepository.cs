using System;
using System.Net.Http;
using System.Threading.Tasks;
using DCNC.DataAccess.PublicTransport.Interfaces;

namespace DCNC.DataAccess.PublicTransport
{
    public class PublicTransportRepository : IPublicTransportRepository
    {
        static readonly HttpClient Client = new HttpClient();

        public async Task<string> DownloadData(string url)
        {
            try
            {
                return await Client.GetStringAsync(url);
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
            catch (Exception exception)
            {
                return string.Empty;
            }
        }
    }
}