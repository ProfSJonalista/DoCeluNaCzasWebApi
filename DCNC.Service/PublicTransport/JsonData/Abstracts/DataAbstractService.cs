using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.DataAccess.PublicTransport;
using DCNC.Service.Database;
using DCNC.Service.PublicTransport.JsonData.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DCNC.Service.PublicTransport.JsonData.Abstracts
{
    public abstract class DataAbstractService : IJsonDataService
    {
        private readonly IDocumentStoreRepository _documentStoreRepository;

        protected DataAbstractService() { }

        protected DataAbstractService(IDocumentStoreRepository documentStoreRepository)
        {
            _documentStoreRepository = documentStoreRepository;
        }

        public async Task<JObject> GetDataAsJObjectAsync(string url, JsonType type)
        {
            var json = await PublicTransportRepository.DownloadData(url);

            if (type == JsonType.Delay) return JsonConvert.DeserializeObject<JObject>(json);

            if (!string.IsNullOrEmpty(json))
            {
                var oldJson = _documentStoreRepository.GetDbJson(type);

                if (oldJson != null && !string.IsNullOrEmpty(oldJson.Id))
                    _documentStoreRepository.Delete(oldJson.Id);

                _documentStoreRepository.Save(new DbJson() { Json = json, Type = type });
            }
            else
            {
                json = _documentStoreRepository.GetDbJson(type).Json;
            }

            return JsonConvert.DeserializeObject<JObject>(json);
        }

        public virtual List<T> GetList<T>(JObject dataAsJObject)
        {
            var jsonDataList = new List<T>();

            foreach (var item in dataAsJObject.Children())
            {
                jsonDataList.Add((T)(object)Converter(item));
            }

            return jsonDataList;
        }

        public abstract object Converter(JToken dataAsJToken);
    }
}