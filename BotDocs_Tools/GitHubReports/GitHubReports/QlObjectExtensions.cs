using GitHubQl.Models.GitHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitHubReports
{
    /// <summary>Contains extension methods for working with GraphQL objects.</summary>
    public static class QlObjectExtensions
    {
        /// <summary>Gets the number of items contained in a connection object.</summary>
        /// <typeparam name="T">The type of objects contained in the connection.</typeparam>
        /// <param name="conn">The connection object.</param>
        /// <returns>A representation of the number of items in the connection.</returns>
        public static string GetCount<T>(this Connection<T> conn)
        {
            if (conn is null) return string.Empty;

            // TODO Review logic and figure out how to represent the count when we don't know
            //      the total but do know there are more pages of data.

            string count = null;
            if (conn?.Edges != null)
            {
                count = conn.Edges.Count.ToString();
            }
            else if (conn?.Nodes != null)
            {
                count = conn.Nodes.Count.ToString();
            }

            var tot = conn?.TotalCount.ToString();

            return
                count is null ? tot ?? string.Empty
                : tot is null ? count
                : count == tot ? count
                : $"{count} of {tot}";
        }

        /// <summary>Converts a connection to a string representing the connection's contained items.</summary>
        /// <typeparam name="T">The type of objects contained in the connection.</typeparam>
        /// <param name="conn">The connection object.</param>
        /// <param name="sep">A string to use to separate each item in the connection.</param>
        /// <param name="op">A function for converting an item to a string.</param>
        /// <returns>A representation to the connection as a list of its items.</returns>
        public static string Concat<T>(this Connection<T> conn, string sep, Func<T, string> op)
        {
            if (conn is null) return string.Empty;

            // TODO Review logic and figure out how to represent the list when we know there are
            //      more pages of data.

            if (conn.Nodes != null)
            {
                return string.Join(sep, conn.Nodes.Select(op));
            }

            if (conn.Edges != null)
            {
                return string.Join(sep, conn.Edges.Select(
                    e => op((e != null) ? e.Node : default)));
            }

            return conn.TotalCount != null && conn.TotalCount > 0
                ? "??" : string.Empty;
        }
    }
}
