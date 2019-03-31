using System.Text.RegularExpressions;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.Helpers
{
    public static class JoinTripHelper
    {
        private static readonly char[] CharactersToDeleteFromString = { ' ', '+' };
        private const string PARENTHESIS_TO_DELETE = "(\\[.*\\])|(\".*\")|('.*')|(\\(.*\\))";

        public static string GetFirstStopName(string tripHeadsign)
        {
            var belongsToGdynia = tripHeadsign.Contains(">");
            var index = 0;

            tripHeadsign = tripHeadsign.Trim(CharactersToDeleteFromString);

            if (belongsToGdynia)
                index = tripHeadsign.IndexOf('>') - 1;
            else
                index = tripHeadsign.IndexOf('-') - 1;

            var input = tripHeadsign.Substring(0, index);

            return Regex.Replace(input, PARENTHESIS_TO_DELETE, "");
        }

        public static string GetDestinationStopName(string tripHeadsign)
        {
            var belongsToGdynia = tripHeadsign.Contains(">");
            var index = 0;

            if (belongsToGdynia)
                index = tripHeadsign.IndexOf('>') + 2;
            else
                index = tripHeadsign.IndexOf('-') + 2;

            var length = tripHeadsign.Length - index;

            var input = tripHeadsign.Substring(index, length);

            return Regex.Replace(input, PARENTHESIS_TO_DELETE, "");
        }
    }
}