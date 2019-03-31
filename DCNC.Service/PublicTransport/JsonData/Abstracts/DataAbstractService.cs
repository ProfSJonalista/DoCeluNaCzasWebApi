using DCNC.Service.PublicTransport.JsonData.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using DCNC.Bussiness.PublicTransport.JsonData.General.Shared;
using DCNC.Service.Database;

namespace DCNC.Service.PublicTransport.JsonData.Abstracts
{
    public abstract class DataAbstractService : DataDownloadService, IJsonDataService
    {
        protected DataAbstractService() { }

        protected DataAbstractService(DocumentStoreRepository documentStoreRepository) : base(documentStoreRepository) { }

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