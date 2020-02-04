﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class EnumerableExtensions
    {
        public static IList<T> GetValues<T>()
        {
            return ((T[])Enum.GetValues(typeof(T))).ToList();
        }

        /// <summary>Gets the index of the first item in a list or array that matches a criteria.</summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="list">The list in which to search.</param>
        /// <param name="predicate">The criteria to use to find a match.</param>
        /// <param name="start">The index in the list to start the search from.</param>
        /// <returns>The index of the first item in the list to match the criteria; or -1 if no match is found.</returns>
        public static int IndexOf<T>(this IList<T> list, Func<T, bool> predicate, int start = 0)
        {
            for (int i = start; i < list.Count; i++)
            {
                if (predicate(list[i])) return i;
            }
            return -1;
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach(var item in items) { collection.Add(item); }
        }
    }
}
