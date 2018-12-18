using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCNC.Service.PublicTransport.UpdateService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCNC.Service.PublicTransport.UpdateService.Tests
{
    [TestClass]
    public class UpdateDataServiceTests
    {
        [TestMethod]
        public async void AllocateDataTest()
        {
            UpdateDataService updateDataService = new UpdateDataService();
            await UpdateDataService.DownloadData();
            UpdateDataService.AllocateData();

        }
    }
}