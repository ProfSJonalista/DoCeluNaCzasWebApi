using DCNC.Bussiness.PublicTransport.JsonData.Shared;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace DCNC.Service.PublicTransport.JsonData.Interfaces
{
    internal interface IJsonDataService
    {
        List<T> GetMappedListAndCacheData<T>(JObject jsonDataAsJObject, string cacheKey);
        IEnumerable<T> GetList<T>(JObject dataAsJObject);
        Common Converter(JToken dataAsJToken);
    }
}
