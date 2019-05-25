using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.Service.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using DCNC.Service.Database.Interfaces;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers
{
    public class ConvertingHelper
    {
        readonly Converter _converter;
        readonly FilterHelper _filterHelper;
        readonly IDocumentStoreRepository _documentStoreRepository;

        public ConvertingHelper(Converter converter, FilterHelper filterHelper, IDocumentStoreRepository documentStoreRepository)
        {
            _converter = converter;
            _filterHelper = filterHelper;
            _documentStoreRepository = documentStoreRepository;
        }
        
        public void ChangeTimeTableJsonsToObjectsAndSaveToDb(List<StopTimeUrl> convertedStopTimes, List<TimeTableDateTime> entitiesThatWerentDownloaded)
        {
            foreach (var stopTime in convertedStopTimes)
            {
                var notDownloadedEntitiesByRouteId = entitiesThatWerentDownloaded.Where(x => x.RouteId == stopTime.RouteId).ToList();
                var entitiesToDelete = _documentStoreRepository.GetTimeTableDataByRouteId(stopTime.RouteId);

                if(notDownloadedEntitiesByRouteId.Count > 0) entitiesToDelete = _filterHelper.Filter(entitiesToDelete, notDownloadedEntitiesByRouteId);

                var jsonsToConvert = _documentStoreRepository.GetJsonsByRouteId(stopTime.RouteId);
                var timeTableDataList = new List<TimeTableData>();

                if (jsonsToConvert.Count <= 0) continue;

                foreach (var item in jsonsToConvert)
                {
                    var jsonAsJObject = JsonConvert.DeserializeObject<JObject>(item.Json);
                    timeTableDataList.Add(_converter.Deserialize(jsonAsJObject));
                }

                timeTableDataList = timeTableDataList.Where(x => x.StopTimes.Count > 0).ToList();

                _documentStoreRepository.Save(timeTableDataList);
                _documentStoreRepository.Delete(jsonsToConvert.Select(x => x.Id).ToList());
                _documentStoreRepository.Delete(entitiesToDelete.Select(x => x.Id).ToList());
            }
        }
    }
}