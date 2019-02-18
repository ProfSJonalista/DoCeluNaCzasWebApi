using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DCNC.Service.PublicTransport.JsonData.Interfaces
{
    public interface IDataDownloadService
    {
        Task<JObject> GetDataAsJObjectAsync(string url);
    }
}
