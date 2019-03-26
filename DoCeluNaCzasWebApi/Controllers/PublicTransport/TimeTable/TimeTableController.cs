using DCNC.Bussiness.PublicTransport.TimeTable;
using DoCeluNaCzasWebApi.Services.UpdateService;
using System.Collections.Generic;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers.PublicTransport.TimeTable
{
    public class TimeTableController : ApiController
    {
        public List<TimeTableData> GetByRouteId(int id)
        {
            return UpdateTimeTableService.GetTimeTableData(id);
        }
    }
}
