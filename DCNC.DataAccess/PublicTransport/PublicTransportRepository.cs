using DCNC.DataAccess.Helpers;
using System.Net.Http;
using System.Threading.Tasks;

namespace DCNC.DataAccess.PublicTransport
{
    public class PublicTransportRepository
    {
        public async Task<string> GetExpeditionData()
        {
            return await DownloadData(Urls.EXPEDITION);
        }

        public static async Task<string> DownloadData(string url)
        {
            var data = "";
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                data = json;
            }

            return data;
        }
    }
}