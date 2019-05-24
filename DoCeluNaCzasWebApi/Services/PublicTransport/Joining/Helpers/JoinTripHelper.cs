using System.Text.RegularExpressions;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.Joining.Helpers
{
    public static class JoinTripHelper
    {
        private static readonly char[] CharactersToDeleteFromString = { ' ', '+' };
        private const string PATTERN = "(\\[.*\\])|(\".*\")|('.*')|(\\(.*\\))";

        public static string GetFirstStopName(string tripHeadsign)
        {
            var belongsToGdynia = tripHeadsign.Contains(">");
            int index;

            tripHeadsign = tripHeadsign.Trim(CharactersToDeleteFromString);

            if (belongsToGdynia) index = tripHeadsign.IndexOf('>') - 1;
            else index = tripHeadsign.IndexOf('-') - 1;

            var input = tripHeadsign.Substring(0, index);
            var output = Regex.Replace(input, PATTERN, "");

            return output;
        }

        public static string GetDestinationStopName(string tripHeadsign)
        {
            var belongsToGdynia = tripHeadsign.Contains(">");
            int index;

            if (belongsToGdynia) index = tripHeadsign.IndexOf('>') + 2;
            else index = tripHeadsign.IndexOf('-') + 2;

            var length = tripHeadsign.Length - index;
            var input = tripHeadsign.Substring(index, length);
            var output = Regex.Replace(input, PATTERN, "");

            return output;
        }
    }
}