using DCNC.DataAccess.PublicTransport;
using DCNC.Service.PublicTransport.JsonData.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace DCNC.Service.PublicTransport.JsonData.Abstracts
{
    public abstract class DataDownloadService : IDataDownloadService
    {
        public async Task<JObject> GetDataAsJObjectAsync(string url)
        {
            var json = await PublicTransportRepository.DownloadData(url);
            return JsonConvert.DeserializeObject<JObject>(json);
        }
    }
}
