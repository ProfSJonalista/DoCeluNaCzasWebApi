using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.DataAccess.PublicTransport.Interfaces;
using DCNC.Service.Database.Interfaces;
using DCNC.Service.PublicTransport.JsonData.Abstracts.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DCNC.Service.PublicTransport.JsonData.Abstracts
{
    public abstract class DataAbstractService : IJsonDataService
    {
        readonly IDocumentStoreRepository _documentStoreRepository;
        readonly IPublicTransportRepository _publicTransportRepository;

        protected DataAbstractService(IDocumentStoreRepository documentStoreRepository, IPublicTransportRepository publicTransportRepository)
        {
            _documentStoreRepository = documentStoreRepository;
            _publicTransportRepository = publicTransportRepository;
        }

        public async Task<JObject> GetDataAsJObjectAsync(string url, JsonType type)
        {
            var json = await _publicTransportRepository.DownloadData(url);

            try
            {
                if (!string.IsNullOrEmpty(json))
                {
                    var oldJson = _documentStoreRepository.GetDbJson(type);

                    if (oldJson != null && !string.IsNullOrEmpty(oldJson.Id))
                        _documentStoreRepository.Delete(oldJson.Id);

                    _documentStoreRepository.Save(new DbJson {Json = json, Type = type});
                }
                else
                {
                    json = _documentStoreRepository.GetDbJson(type).Json;
                }
            }
            catch (OutOfMemoryException e)
            {
                json = _documentStoreRepository.GetDbJson(type).Json;
            }

            return JsonConvert.DeserializeObject<JObject>(json);
        }

        public virtual List<T> GetData<T>(JObject dataAsJObject)
        {
            var jsonDataList = new List<T>();

            if (!dataAsJObject.HasValues)  return jsonDataList;

            foreach (var item in dataAsJObject.Children())
            {
                jsonDataList.Add((T) Converter(item));
            }

            return jsonDataList;
        }

        protected abstract object Converter(JToken dataAsJToken);
    }
}
