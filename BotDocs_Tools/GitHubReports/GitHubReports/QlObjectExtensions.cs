using GitHubQl.Models.GitHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitHubReports
{
    public static class QlObjectExtensions
    {
        public static string GetCount<T>(this Connection<T> conn)
        {
            if (conn is null) return string.Empty;

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

        public static string Concat<T>(this Connection<T> conn, string sep, Func<T, string> op)
        {
            if (conn is null) return string.Empty;

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
