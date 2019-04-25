using System.Collections.Generic;
using System.Threading.Tasks;
using DCNC.Bussiness.PublicTransport.JsonData;
using Newtonsoft.Json.Linq;

namespace DCNC.Service.PublicTransport.JsonData.Abstracts.Interfaces
{
    public interface IJsonDataService
    {
        Task<JObject> GetDataAsJObjectAsync(string url, JsonType type);
        List<T> GetData<T>(JObject dataAsJObject);
    }
}
