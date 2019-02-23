﻿using DCNC.Bussiness.PublicTransport.JsonData;
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

            //var joinedStopList = busStopDataList.FirstOrDefault().Stops;

            //var stopComparer = new StopComparer();

            //busStopDataList.ForEach(
            //    busStopData => 
            //        joinedStopList.Union(busStopData.Stops, stopComparer));

            //joinedBusStopModel.Stops = StopMapper.GetMappedStopList(joinedStopList);

            //return joinedBusStopModel;
        }

    }
}