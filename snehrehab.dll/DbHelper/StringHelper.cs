using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DbHelper
{
    public enum TitleCase
    {
        First,
        All
    }

    public static class StringHelper
    {
        private static CultureInfo ci = new CultureInfo("en-US");

        public static string ToTitleCase(this string str)
        {
            str = str.ToLower();
            var strArray = str.Split(' ');
            if (strArray.Length > 1)
            {
                strArray[0] = ci.TextInfo.ToTitleCase(strArray[0]);
                return string.Join(" ", strArray);
            }
            return ci.TextInfo.ToTitleCase(str);
        }

        public static string ToTitleCase(this string str, TitleCase tcase)
        {
            str = str.ToLower();
            switch (tcase)
            {
                case TitleCase.First:
                    var strArray = str.Split(' ');
                    if (strArray.Length > 1)
                    {
                        strArray[0] = ci.TextInfo.ToTitleCase(strArray[0]);
                        return string.Join(" ", strArray);
                    }
                    break;
                case TitleCase.All:
                    return ci.TextInfo.ToTitleCase(str);
                default:
                    break;
            }
            return ci.TextInfo.ToTitleCase(str);
        }

        public static string RemoveWhiteSpace(string str1)
        {
            if (string.IsNullOrEmpty(str1)) { return string.Empty; }
            Regex rx = new Regex("\\s+");
            return rx.Replace(str1, " ");  // call to replace space 
        }
    }
}
