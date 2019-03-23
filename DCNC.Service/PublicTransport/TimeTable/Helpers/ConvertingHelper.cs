using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.Bussiness.PublicTransport.TimeTable.Shared;
using DCNC.Service.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers
{
    public class ConvertingHelper
    {
        private readonly Converter _converter;
        private readonly FilterHelper _filterHelper;
        private readonly DocumentStoreRepository _documentStoreRepository;

        public ConvertingHelper(DocumentStoreRepository documentStoreRepository)
        {
            _converter = new Converter();
            _filterHelper = new FilterHelper();
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

                _documentStoreRepository.Save(timeTableDataList);
                _documentStoreRepository.Delete(jsonsToConvert.Cast<Common>().ToList());
                _documentStoreRepository.Delete(entitiesToDelete.Cast<Common>().ToList());
            }
        }
    }
}