using DCNC.Service.Tests.PublicTransport.JoiningTrips.Helpers.ObjectMakers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DCNC.Service.PublicTransport.JoiningTrips.Helpers.Tests
{
    [TestClass()]
    public class StopComparerTests
    {
        [TestMethod()]
        public void Equals_Positive_Test()
        {
            var (stop1, stop2) = StopMaker.GetEqualStops();
            var sc = new StopComparer();

            var areEqual = sc.Equals(stop1, stop2);

            Assert.IsTrue(areEqual);
        }

        [TestMethod()]
        public void Equals_Negative_Test()
        {
            var (stop1, stop2) = StopMaker.GetNotEqualStops();
            var sc = new StopComparer();

            var areEqual = sc.Equals(stop1, stop2);

            Assert.IsFalse(areEqual);
        }
    }
}