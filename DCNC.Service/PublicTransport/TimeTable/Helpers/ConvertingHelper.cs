using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using DCNC.Bussiness.PublicTransport.TimeTable;
using DCNC.Service.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers
{
    public class ConvertingHelper
    {
        private readonly Converter _converter;
        private readonly DocumentStoreRepository _documentStoreRepository;
        public ConvertingHelper(DocumentStoreRepository documentStoreRepository)
        {
            _converter = new Converter();
            _documentStoreRepository = documentStoreRepository;
        }


        public void ChangeTimeTableJsonsToObjectsAndSaveToDb(List<StopTimeUrl> convertedStopTimes)
        {
            foreach (var stopTime in convertedStopTimes)
            {
                var jsonsToConvert = _documentStoreRepository.GetJsonsByRouteId(stopTime.RouteId);
                var timeTableDataList = new List<TimeTableData>();

                foreach (var item in jsonsToConvert)
                {
                    var jsonAsJObject = JsonConvert.DeserializeObject<JObject>(item.Json);
                    timeTableDataList.Add(_converter.Deserialize(jsonAsJObject));
                }

                _documentStoreRepository.Save(timeTableDataList);
                _documentStoreRepository.Delete(jsonsToConvert);
            }
        }
    }
}