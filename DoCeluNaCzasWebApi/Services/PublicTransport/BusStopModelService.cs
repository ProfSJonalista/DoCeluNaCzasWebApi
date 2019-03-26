using DCNC.Bussiness.PublicTransport.JsonData.General;
using DoCeluNaCzasWebApi.Models.PublicTransport.General;
using DoCeluNaCzasWebApi.Services.PublicTransport.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace DoCeluNaCzasWebApi.Services.PublicTransport
{
    public class BusStopModelService
    {
        public BusStopDataModel JoinBusStopData(List<BusStopData> busStopDataList)
        {
            //TODO - check this in client apps
            var firstResult = busStopDataList.FirstOrDefault();
            var stopList = busStopDataList.SelectMany(x => x.Stops)
                                          .GroupBy(x => new
                                          {
                                              x.StopId,
                                              x.StopLat,
                                              x.StopLon,
                                              x.StopDesc
                                          })
                                          .Select(x => x.FirstOrDefault())
                                          .ToList();

            return new BusStopDataModel()
            {
                Day = firstResult.Day,
                LastUpdate = firstResult.LastUpdate,
                Stops = StopMapper.GetMappedStopList(stopList)
            };
        }
    }
}