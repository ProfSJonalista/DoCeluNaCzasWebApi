using NUnit.Framework;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.Helpers.Tests
{
    [TestFixture()]
    public class JoinTripHelperTests
    {
        private const string TRIP_HEADSIGN_GDYNIA = "Karwiny Tuwima > Witomino Leśniczówka";
        private const string TRIP_HEADSIGN_GDANSK = "Jelitkowo(201) - Zajezdnia Wrzeszcz(9999)";

        [TestCase(TRIP_HEADSIGN_GDYNIA)]
        public void GetFirstStopName_Gdynia_Success(string tripHeadsign)
        {
            var result = JoinTripHelper.GetFirstStopName(tripHeadsign);

            Assert.IsNotNull(result);
            Assert.AreEqual("Karwiny Tuwima", result);
        }

        [TestCase(TRIP_HEADSIGN_GDYNIA)]
        public void GetFirstStopName_Gdynia_Failure(string tripHeadsign)
        {
            var result = JoinTripHelper.GetFirstStopName(tripHeadsign);

            Assert.IsNotNull(result);
            Assert.AreNotEqual("Karwiny Tuwi", result);
        }

        [Test()]
        public void GetFirstStopName_Gdansk_Success()
        {
            var result = JoinTripHelper.GetFirstStopName(TRIP_HEADSIGN_GDANSK);

            Assert.IsNotNull(result);
            Assert.AreEqual("Jelitkowo", result);
        }

        [Test()]
        public void GetFirstStopName_Gdansk_Failure()
        {
            var result = JoinTripHelper.GetFirstStopName(TRIP_HEADSIGN_GDANSK);

            Assert.IsNotNull(result);
            Assert.AreNotEqual("Jelitko", result);
        }

        [TestCase(TRIP_HEADSIGN_GDYNIA)]
        public void GetDestinationStopName_Gdynia_Success(string tripHeadsign)
        {
            var result = JoinTripHelper.GetDestinationStopName(tripHeadsign);

            Assert.IsNotNull(result);
            Assert.AreEqual("Witomino Leśniczówka", result);
        }

        [TestCase(TRIP_HEADSIGN_GDYNIA)]
        public void GetDestinationStopName_Gdynia_Failure(string tripHeadsign)
        {
            var result = JoinTripHelper.GetDestinationStopName(tripHeadsign);

            Assert.IsNotNull(result);
            Assert.AreNotEqual("Witomino Leśniczów", result);
        }

        [TestCase(TRIP_HEADSIGN_GDANSK)]
        public void GetDestinationStopName_Gdansk_Success(string tripHeadsign)
        {
            var result = JoinTripHelper.GetDestinationStopName(tripHeadsign);

            Assert.IsNotNull(result);
            Assert.AreEqual("Zajezdnia Wrzeszcz", result);
        }

        [TestCase(TRIP_HEADSIGN_GDANSK)]
        public void GetDestinationStopName_Gdansk_Failure(string tripHeadsign)
        {
            var result = JoinTripHelper.GetDestinationStopName(tripHeadsign);

            Assert.IsNotNull(result);
            Assert.AreNotEqual("Zajezdnia Wrzesz", result);
        }
    }
}