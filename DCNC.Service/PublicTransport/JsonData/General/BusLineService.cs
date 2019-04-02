using DCNC.Bussiness.PublicTransport.JsonData.General;
using DCNC.Service.PublicTransport.JsonData.Abstracts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DCNC.Service.Database;

namespace DCNC.Service.PublicTransport.JsonData.General
{
    public class BusLineService : DataAbstractService
    {
        public BusLineService(IDocumentStoreRepository documentStoreRepository) : base(documentStoreRepository) { }

        public override object Converter(JToken busLine)
        {
            var busLineData = new BusLineData()
            {
                Day = DateTime.Parse(busLine.Path),
                Routes = new List<Route>()
            };

            foreach (var item in busLine.Children())
            {
                busLineData.LastUpdate = item.Value<DateTime>("lastUpdate");

                var routeList = item.Value<JArray>("routes");

                foreach (var line in routeList.Children<JObject>())
                {
                    var routeToAdd = new Route()
                    {
                        RouteId = line.Value<int>("routeId"),
                        AgencyId = line.Value<int>("agencyId"),
                        RouteShortName = line.Value<string>("routeShortName"),
                        RouteLongName = line.Value<string>("routeLongName"),
                        ActivationDate = line.Value<DateTime>("activationDate")
                    };

                    busLineData.Routes.Add(routeToAdd);
                }
            }

            return busLineData;
        }
    }
}