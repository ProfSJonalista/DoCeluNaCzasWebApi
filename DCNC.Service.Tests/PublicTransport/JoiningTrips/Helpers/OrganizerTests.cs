using DCNC.Service.Tests.PublicTransport.JoiningTrips.Helpers.ObjectMakers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DCNC.Service.PublicTransport.JoiningTrips.Helpers.Tests
{
    [TestClass()]
    public class OrganizerTests
    {
        [TestMethod()]
        public void GetTripsTest()
        {
            var trips = OrganizerMaker.GetTrips();
            var organizer = new Organizer();

            var organizedTrips = organizer.GetTrips(trips);
            var containsStartKey = organizedTrips.ContainsKey(1);

            IsNotNull(organizedTrips);
            IsTrue(containsStartKey);
        }
    }
}