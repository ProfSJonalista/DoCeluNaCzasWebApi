using DCNC.Bussiness.PublicTransport.JsonData;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DCNC.Service.PublicTransport.JsonData.Interfaces
{
    public interface IJsonDataService
    {
        Task<JObject> GetDataAsJObjectAsync(string url, JsonType type);
        List<T> GetList<T>(JObject dataAsJObject);
        object Converter(JToken dataAsJToken);
    }
}
