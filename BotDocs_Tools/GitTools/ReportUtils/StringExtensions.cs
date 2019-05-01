using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportUtils
{
    public static class StringExtensions
    {
        /// <summary>CSV-escapes a string.</summary>
        /// <param name="value">The string to escape.</param>
        /// <returns>The CSV-escaped string.</returns>
        public static string CsvEscape(this string value)
        {
            var escaped = value
                .Replace("\r", @"\r")
                .Replace("\n", @"\n")
                .Replace("\t", @"\t")
                .Replace("\"", "\"\"");

            return '"' + escaped + '"';
        }

        public static int IndexOf(this string s, Func<char,bool> predicate, int start = 0)
        {
            return s.ToCharArray().IndexOf(predicate, start);
        }
    }

    public static class EnumerableExtensions
    {
        /// <summary>Gets the index of the first item in a list or array that matches a criteria.</summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The list in which to search.</param>
        /// <param name="predicate">The crietia to use to find a match.</param>
        /// <param name="start">The index in the list to start the search from.</param>
        /// <returns>The index of the first item in the list to match the criteria; or -1 if no match is found.</returns>
        public static int IndexOf<T>(this IList<T> list, Func<T, bool> predicate, int start = 0)
        {
            for (int i = start; i<list.Count; i++)
            {
                if (predicate(list[i])) return i;
            }
            return -1;
        }
    }
}
