using DCNC.DataAccess.PublicTransport;
using DCNC.Service.PublicTransport.Delays;
using DCNC.Service.PublicTransport.JsonData.Delays;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers
{
    public class ValuesController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        public async Task<string> Get(int id)
        {
            var cos = await new DelayService(new DelayJsonService(new PublicTransportRepository())).GetDelays(/*30129*//*1206*/id);
            return "Pamparampampam";
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
