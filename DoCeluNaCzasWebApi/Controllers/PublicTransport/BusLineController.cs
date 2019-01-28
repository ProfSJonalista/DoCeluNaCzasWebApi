using DCNC.Bussiness.PublicTransport;
using DCNC.Service.PublicTransport.Resources;
using DCNC.Service.PublicTransport.UpdateService;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Http;
using DCNC.Bussiness.PublicTransport.JsonData;

namespace DoCeluNaCzasWebApi.Controllers.PublicTransport
{
    public class BusLineController : ApiController
    {
        public async Task<BusLineData> GetAsync(bool hasData)
        {
            ObjectCache _cache = MemoryCache.Default;

            var generalData = _cache[CacheKeys.GENERAL_DATA_KEY] as GeneralData;

            if(generalData == null)
            {
                await UpdateDataService.Init();
                generalData = _cache[CacheKeys.GENERAL_DATA_KEY] as GeneralData;
            }

            return generalData.BusLineData;
        }
    }
}
