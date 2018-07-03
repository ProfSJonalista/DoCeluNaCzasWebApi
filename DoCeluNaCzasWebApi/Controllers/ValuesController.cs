﻿using DCNC.Bussiness;
using DCNC.DataAccess;
using DCNC.DataAccess.PublicTransport;
using DCNC.Service;
using DCNC.Service.Bus_stops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public async Task<string> Get(int id)
        {
            DataClass data = new DataClass();
            BussinessClass bussiness = new BussinessClass();
            ServiceClass service = new ServiceClass();

            BusStopsService busStopsService = new BusStopsService();
            var derp = await busStopsService.GetStopsForCurrentDay();

            return "Data " + data.data + ", Bussiness: " + bussiness.bussiness + ", Service: " + service.service + ", WORK GODDAMIT" + ", Test CD. Oooh eeh oooh ah ah ting tang, walla walla bang bang";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
