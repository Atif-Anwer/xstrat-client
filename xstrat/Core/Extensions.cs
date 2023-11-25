﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xstrat
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
        public static string RemoveAllWhitespace(this string str)
        {
            if (str == null) return null;
            return string.Concat(str.Where(c => !char.IsWhiteSpace(c)));
        }
        public static bool IsDigitsOnly(this string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

        public static bool ToBool(this string input)
        {
            return (input.Trim() == "1");
        }
    }
    public static class DateTimeExtensions
    {
        public static DateTime SetTime(this DateTime dateTime, int hour, int minute, int second)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, minute, second, dateTime.Kind);
        }
    }
    public static class ListExtensions
    {
        // Extension method to make the list empty if it's null
        public static List<T> EmptyIfNull<T>(this List<T> list)
        {
            return list ?? new List<T>();
        }
    }
}
