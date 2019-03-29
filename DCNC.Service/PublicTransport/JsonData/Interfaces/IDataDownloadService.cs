using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using DCNC.Bussiness.PublicTransport.JsonData;

namespace DCNC.Service.PublicTransport.JsonData.Interfaces
{
    public interface IDataDownloadService
    {
        Task<JObject> GetDataAsJObjectAsync(string url, JsonType type);
    }
}
