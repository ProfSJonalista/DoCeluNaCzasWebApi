using DCNC.Service.Caching;
using DCNC.Service.Caching.Helpers;
using DoCeluNaCzasWebApi.Models.PublicTransport.Delay;
using System.Collections.ObjectModel;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers.PublicTransport.General
{
    public class ChooseBusStopController : ApiController
    {
        public ObservableCollection<ChooseBusStopModel> Get()
        {
            return CacheService.GetData<ObservableCollection<ChooseBusStopModel>>(CacheKeys.CHOOSE_BUS_STOP_MODEL_OBSERVABALE_COLLECTION);
        }
    }
}
