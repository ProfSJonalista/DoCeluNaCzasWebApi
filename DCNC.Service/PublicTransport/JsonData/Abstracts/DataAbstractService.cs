using DCNC.Bussiness.PublicTransport.JsonData.Shared;
using DCNC.Service.PublicTransport.JsonData.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using DCNC.Service.PublicTransport.Caching;

namespace DCNC.Service.PublicTransport.JsonData.Abstracts
{
    public abstract class DataAbstractService : DataDownloadService, IJsonDataService
    {
        private CacheService _cacheService;

        protected DataAbstractService()
        {
            _cacheService = new CacheService();
        }

        public List<T> GetMappedListAndCacheData<T>(JObject jsonDataAsJObject, string cacheKey)
        {
            var jsonDataList = GetList<T>(jsonDataAsJObject);
            
            _cacheService.CacheData(jsonDataList, cacheKey);

            return jsonDataList.ToList();
        }

        public virtual IEnumerable<T> GetList<T>(JObject dataAsJObject)
        {
            var jsonDataList = new List<T>();

            foreach (var item in dataAsJObject.Children())
            {
                jsonDataList.Add((T)(object)Converter(item));
            }

            return jsonDataList;
        }

        public virtual Common Converter(JToken dataAsJToken)
        {
            return new Common();
        }
    }
}