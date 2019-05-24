using DoCeluNaCzasWebApi.Services.PublicTransport.Joining.Helpers;
using NUnit.Framework;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.Helpers.Tests
{
    [TestFixture()]
    public class JoinTripHelperTests
    {
        private const string TRIP_HEADSIGN_GDYNIA = "Karwiny Tuwima > Witomino Leśniczówka";
        private const string TRIP_HEADSIGN_GDANSK = "Jelitkowo(201) - Zajezdnia Wrzeszcz(9999)";
            
        [TestCase(TRIP_HEADSIGN_GDYNIA, "Karwiny Tuwima")]
        [TestCase(TRIP_HEADSIGN_GDANSK, "Jelitkowo")]
        public void GetFirstStopName_Gdynia_Success(string tripHeadsign, string expectedResult)
        {
            var result = JoinTripHelper.GetFirstStopName(tripHeadsign);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(TRIP_HEADSIGN_GDYNIA, "Karwiny Tuwi")]
        [TestCase(TRIP_HEADSIGN_GDANSK, "Jelitko")]
        public void GetFirstStopName_Failure(string tripHeadsign, string expectedResult)
        {
            var result = JoinTripHelper.GetFirstStopName(tripHeadsign);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(expectedResult, result);
        }

        [TestCase(TRIP_HEADSIGN_GDYNIA, "Witomino Leśniczówka")]
        [TestCase(TRIP_HEADSIGN_GDANSK, "Zajezdnia Wrzeszcz")]
        public void GetDestinationStopName_Success(string tripHeadsign, string expectedResult)
        {
            var result = JoinTripHelper.GetDestinationStopName(tripHeadsign);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(TRIP_HEADSIGN_GDYNIA, "Witomino Leśniczów")]
        [TestCase(TRIP_HEADSIGN_GDANSK, "Zajezdnia Wrzesz")]
        public void GetDestinationStopName_Failure(string tripHeadsign, string expectedResult)
        {
            var result = JoinTripHelper.GetDestinationStopName(tripHeadsign);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(expectedResult, result);
        }
    }
}