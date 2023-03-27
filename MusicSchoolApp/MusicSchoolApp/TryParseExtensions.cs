using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSchoolApp
{
    internal static class TryParseExtensions
    {
        public static bool TryParseNullableInt(string value, out int? result)
        {
            if (int.TryParse(value, out int intValue))
            {
                result = intValue;
                return true;
            }
            else if (string.IsNullOrEmpty(value))
            {
                result = null;
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }
    }
}
