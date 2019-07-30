using System;
using System.Collections.Generic;

namespace DoCeluNaCzasWebApi.Services.PublicTransport.Joining.Helpers
{
    internal class SemiNumericComparer : IComparer<string>
    {
        public int Compare(string s1, string s2)
        {
            if (IsNumeric(s1) && IsNumeric(s2))
            {
                if (Convert.ToInt32(s1) > Convert.ToInt32(s2)) return 1;
                if (Convert.ToInt32(s1) < Convert.ToInt32(s2)) return -1;
                if (Convert.ToInt32(s1) == Convert.ToInt32(s2)) return 0;
            }

            if (IsNumeric(s1) && !IsNumeric(s2))
                return -1;

            if (!IsNumeric(s1) && IsNumeric(s2))
                return 1;

            return string.Compare(s1, s2, true);
        }

        static bool IsNumeric(string value)
        {
            try
            {
                var i = Convert.ToInt32(value);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}