﻿using System.Text.RegularExpressions;

namespace MusicSchoolAsp.Helpers
{
    public static class StringHelper
    {
        public static string MyTrim(this string str)
        {
            str = Regex.Replace(str, @"\s{2,}", " ");
            str = str.Trim();
            return str;
        }
    }
}
