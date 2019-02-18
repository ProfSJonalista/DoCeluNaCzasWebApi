using DCNC.Bussiness.PublicTransport.JsonData;
using DoCeluNaCzasWebApi.Models.PublicTransport;
using DoCeluNaCzasWebApi.Services.PublicTransport.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace DoCeluNaCzasWebApi.Services.PublicTransport
{
    public class BusStopModelService
    {
        public BusStopDataModel JoinBusStopData(List<BusStopData> busStopDataList)
        {
            var firstResult = busStopDataList.FirstOrDefault();

            var joinedBusStopModel = new BusStopDataModel()
            {
                Day = firstResult.Day,
                LastUpdate = firstResult.LastUpdate,
                Stops = new List<StopModel>()
            };

            var joinedStopList = busStopDataList.FirstOrDefault().Stops;

            busStopDataList.ForEach(
                busStopData => 
                    joinedStopList.Union(busStopData.Stops, new StopComparer()));

            joinedBusStopModel.Stops = StopMapper.GetMappedStopList(joinedStopList);

            return joinedBusStopModel;
        }

    }
}