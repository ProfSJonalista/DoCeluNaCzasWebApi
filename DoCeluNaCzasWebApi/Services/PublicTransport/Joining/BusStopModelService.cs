using DCNC.Bussiness.PublicTransport.General;
using DCNC.Bussiness.PublicTransport.JsonData.General;
using DoCeluNaCzasWebApi.Services.PublicTransport.Joining.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.Joining
{
    public class BusStopModelService
    {
        public BusStopDataModel JoinBusStopData(List<BusStopData> busStopDataList)
        {
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

            return new BusStopDataModel
            {
                Day = firstResult.Day,
                LastUpdate = firstResult.LastUpdate,
                Stops = StopMapper.GetMappedStopList(stopList)
            };
        }
    }
}