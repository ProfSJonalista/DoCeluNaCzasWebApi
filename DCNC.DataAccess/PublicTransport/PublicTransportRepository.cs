using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DCNC.DataAccess.PublicTransport
{
    public class PublicTransportRepository
    {
        private static readonly HttpClient Client = new HttpClient();

        public static async Task<string> DownloadData(string url)
        {
            return await Client.GetStringAsync(url);
        }

        public async Task<string> DownloadData(string url, HttpClient client)
        {
            return await client.GetStringAsync(url);
        }
    }
}