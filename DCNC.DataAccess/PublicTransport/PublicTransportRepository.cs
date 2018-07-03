using DCNC.DataAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace DCNC.DataAccess.PublicTransport
{
    public class PublicTransportRepository
    {
        public async static Task<string> GetBusStops()
        {
            string bus_stops = "";

            using(HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(Constants.BUS_STOPS);
                var json = await response.Content.ReadAsStringAsync();

                bus_stops = json;
            }

            return bus_stops;
        }
    }
}