using DCNC.Bussiness.PublicTransport.JsonData.Shared;
using DCNC.Service.PublicTransport.JsonData.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace DCNC.Service.PublicTransport.JsonData.Abstracts
{
    public abstract class DataAbstractService : DataDownloadService, IJsonDataService
    {
        public virtual List<T> GetList<T>(JObject dataAsJObject)
        {
            var jsonDataList = new List<T>();

            foreach (var item in dataAsJObject.Children())
            {
                jsonDataList.Add((T)(object)Converter(item));
            }

            return jsonDataList;
        }

        public virtual object Converter(JToken dataAsJToken)
        {
            return new Common();
        }
    }
}