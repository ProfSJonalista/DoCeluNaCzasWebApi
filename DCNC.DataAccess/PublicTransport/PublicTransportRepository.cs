using System.Net.Http;
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
    }
}