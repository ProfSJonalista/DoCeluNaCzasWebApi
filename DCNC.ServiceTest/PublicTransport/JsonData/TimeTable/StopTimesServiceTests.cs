using DCNC.Bussiness.PublicTransport.JsonData;
using DCNC.Bussiness.PublicTransport.JsonData.TimeTable;
using DCNC.Service.Database;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace DCNC.Service.PublicTransport.JsonData.TimeTable.Tests
{
    [TestFixture()]
    public class StopTimesServiceTests
    {
        Mock<StopTimesService> _jsonDataService;

        [Test]
        public void cos()
        {
            _jsonDataService = new Mock<StopTimesService>(MockBehavior.Strict) { CallBase = true };
            //_jsonDataService.
            
        }
        //private const string INITIAL_DATA = "{ " + "\"2\": [ " + "\"http://87.98.237.99:88/stopTimes?date=2019-03-31&routeId=2\", " + "\"http://87.98.237.99:88/stopTimes?date=2019-04-01&routeId=2\"," + "\"http://87.98.237.99:88/stopTimes?date=2019-04-02&routeId=2\"," + "\"http://87.98.237.99:88/stopTimes?date=2019-04-03&routeId=2\"" + "]," + "\"3\": [" + "\"http://87.98.237.99:88/stopTimes?date=2019-03-31&routeId=3\"," + "\"http://87.98.237.99:88/stopTimes?date=2019-04-01&routeId=3\"," + "\"http://87.98.237.99:88/stopTimes?date=2019-04-02&routeId=3\"" + "]," + "\"4\": [" + "\"http://87.98.237.99:88/stopTimes?date=2019-04-01&routeId=4\"," + "\"http://87.98.237.99:88/stopTimes?date=2019-04-02&routeId=4\"," + "\"http://87.98.237.99:88/stopTimes?date=2019-04-03&routeId=4\"" + "]" + "}";
        //private StopTimesService _stopTimesService = new Mock<StopTimesService>(MockBehavior.Strict) { CallBase = true};

        //public StopTimesServiceTests()
        //{
        //    _stopTimesService = new StopTimesService(new DocumentStoreRepository());
        //}

        //[Test()]
        //public void GetList_Success()
        //{
        //    var dataAsJObject = JsonConvert.DeserializeObject<JObject>(INITIAL_DATA);
        //    var result = _stopTimesService.GetList<StopTimeUrl>(dataAsJObject);

        //    Assert.NotNull(result);
        //    Assert.IsTrue(result.Count > 0);
        //}

        //[Test]
        //public void GetList_Failure()
        //{
        //    var dataAsJObject = JsonConvert.DeserializeObject<JObject>(string.Empty);
        //    var result = _stopTimesService.GetList<StopTimeUrl>(dataAsJObject);

        //    Assert.IsNull(result);
        //}
    }
}