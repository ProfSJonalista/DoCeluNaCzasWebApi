using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DCNC.DataAccess.PublicTransport
{
    public class PublicTransportRepository
    {
        private static readonly HttpClient Client = new HttpClient();

        public static async Task<string> DownloadData(string url)
        {
            try
            {
                return await Client.GetStringAsync(url);
            }
            catch (Exception e)
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
            catch (Exception e)
            {
                return string.Empty;
            }
        }
    }
}