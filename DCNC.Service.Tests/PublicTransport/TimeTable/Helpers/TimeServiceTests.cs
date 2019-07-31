using Microsoft.VisualStudio.TestTools.UnitTesting;
using DCNC.Service.PublicTransport.TimeTable.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCNC.Service.PublicTransport.TimeTable.Helpers.Tests
{
    [TestClass()]
    public class TimeServiceTests
    {
        [TestMethod()]
        public void FilterStopTimesByDateTest()
        {
            //zrobić listę StopTimeUrl (routeId + url)
            //w linkach dać jakieś kosmiczne wartości w datach,
            //ponieważ znajduje się tam DateTime.Today
            //przepuścić przez filtr
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void GetDateFromUrlTest()
        {

            throw new NotImplementedException();
        }
    }
}