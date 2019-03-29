using System;
using DCNC.DataAccess.PublicTransport;
using DCNC.Service.PublicTransport.JsonData.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Service.Database;

namespace DCNC.Service.PublicTransport.JsonData.Abstracts
{
    public abstract class DataDownloadService : IDataDownloadService
    {
        private readonly DocumentStoreRepository _documentStoreRepository;

        protected DataDownloadService(DocumentStoreRepository documentStoreRepository)
        {
            _documentStoreRepository = documentStoreRepository;
        }

        public async Task<JObject> GetDataAsJObjectAsync(string url, JsonType type)
        {
            var json = await PublicTransportRepository.DownloadData(url);

            if (!string.IsNullOrEmpty(json))
            {
                var oldJson = _documentStoreRepository.GetDbJson(type);

                if(oldJson != null && !string.IsNullOrEmpty(oldJson.Id)) _documentStoreRepository.Delete(oldJson.Id); 

                _documentStoreRepository.Save(new DbJson() { Json = json, Type = type });
            }
            else
            {
                json = _documentStoreRepository.GetDbJson(type).Json;
            }

            return JsonConvert.DeserializeObject<JObject>(json);
        }
    }
}
