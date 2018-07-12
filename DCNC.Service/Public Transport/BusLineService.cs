using DCNC.Bussiness.Public_Transport;
using DCNC.DataAccess.PublicTransport;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DCNC.Service.Public_Transport
{
    public class BusLineService
    {
        public async static Task<string> GetLinesForCurrentDay()
        {
            var json = await PublicTransportRepository.GetBusLines();
            JObject lines = (JObject)JsonConvert.DeserializeObject(json);
            var data = BusLineConverter(lines.First);
        
            var jsonToSend = JsonConvert.SerializeObject(data);
            return jsonToSend;
        }

        private static BusLineData BusLineConverter(JToken busLine)
        {
            BusLineData busLineData = new BusLineData();
            busLineData.Routes = new List<Route>();
            busLineData.Day = busLine.Path;

            foreach(var item in busLine.Children())
            {
                busLineData.LastUpdate = item.Value<string>("lastUpdate");

                var routeList = item.Value<JArray>("routes");

                foreach(JObject line in routeList.Children<JObject>())
                {
                    Route routeToAdd = new Route()
                    {
                        RouteId = line.Value<int>("routeId"),
                        AgencyId = line.Value<int>("agencyId"),
                        RouteShortName = line.Value<string>("routeShortName"),
                        RouteLongName = line.Value<string>("routeLongName"),
                        ActivationDate = line.Value<string>("activationDate")
                    };

                    busLineData.Routes.Add(routeToAdd);
                }
            }

            return busLineData;
        }
    }
}