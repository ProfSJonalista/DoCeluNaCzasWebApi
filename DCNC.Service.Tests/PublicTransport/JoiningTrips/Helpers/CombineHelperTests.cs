using DCNC.Service.PublicTransport.JoiningTrips.Helpers.Keys;
using DCNC.Service.Tests.PublicTransport.Helpers.ObjectMakers.JoiningTrips;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace DCNC.Service.PublicTransport.JoiningTrips.Helpers.Tests
{
    [TestClass()]
    public class CombineHelperTests
    {
        [TestMethod()]
        public void GetMainRoute_Start_Test()
        {
            var list = TripMaker.GetOrganizedTripList(true);
            var ch = new CombineHelper(new StopComparer());

            var tripToCheck = ch.GetMainRoute(list, TripKey.START);
            Assert.IsTrue(tripToCheck.MainRoute);
        }

        [TestMethod()]
        public void SetMainRoute_Test()
        {
            var trip = TripMaker.GetTrip();

            trip = CombineHelper.SetMainRoute(trip);

            Assert.IsTrue(trip.MainRoute);
            Assert.IsTrue(trip.Stops.First().MainTrip);
        }

        [TestMethod()]
        public void GetJoinedStopsTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void GetStopsTest()
        {
            throw new NotImplementedException();
        }
    }
}