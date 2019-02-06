﻿using DCNC.Bussiness.PublicTransport.JsonData.Shared;
using DCNC.Service.PublicTransport.JsonData.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace DCNC.Service.PublicTransport.JsonData.Abstracts
{
    public abstract class DataAbstractService : DataDownloadService, IJsonDataService
    {
        public IEnumerable<T> GetMappedListAndCacheData<T>(JObject jsonDataAsJObject)
        {
            var jsonDataList = GetList<T>(jsonDataAsJObject);
            //TODO create cache service to cache data
            return jsonDataList;
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