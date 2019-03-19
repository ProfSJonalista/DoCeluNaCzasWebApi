using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace DCNC.Service.PublicTransport.JsonData.Interfaces
{
    internal interface IJsonDataService
    {
        List<T> GetList<T>(JObject dataAsJObject);
        object Converter(JToken dataAsJToken);
    }
}
