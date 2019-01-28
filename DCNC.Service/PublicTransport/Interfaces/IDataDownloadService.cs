using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCNC.Service.PublicTransport.Interfaces
{
    public interface IDataDownloadService
    {
        T GetData<T>(string url, Type dataType);
    }
}
