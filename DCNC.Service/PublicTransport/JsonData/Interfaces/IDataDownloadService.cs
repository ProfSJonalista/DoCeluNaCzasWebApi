using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace DCNC.Service.PublicTransport.JsonData.Interfaces
{
    public interface IDataDownloadService
    {
        Task<JObject> GetDataAsJObjectAsync(string url);
    }
}
