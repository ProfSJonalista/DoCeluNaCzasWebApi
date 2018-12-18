﻿using DCNC.Bussiness;
using DCNC.DataAccess;
using DCNC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers
{
    [Authorize]
    public class AuthorizedValuesController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public string Get(int id)
        {
            return "pamparam";
        }

        public void Post([FromBody]string value)
        {
        }

        public void Put(int id, [FromBody]string value)
        {
        }
        
        public void Delete(int id)
        {
        }
    }
}
