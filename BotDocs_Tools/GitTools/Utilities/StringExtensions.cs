using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Utilities
{
    public static class StringExtensions
    {
        /// <summary>CSV-escapes a string.</summary>
        /// <param name="value">The string to escape.</param>
        /// <returns>The CSV-escaped string.</returns>
        public static string CsvEscape(this string value)
        {
            if (value is null) return string.Empty;

            var escaped = value
                .Replace("\r", @"\r")
                .Replace("\n", @"\n")
                .Replace("\t", @"\t")
                .Replace("\"", "\"\"");

            return '"' + escaped + '"';
        }

        public static int IndexOf(this string s, Func<char, bool> predicate, int start = 0)
        {
            return s.ToCharArray().IndexOf(predicate, start);
        }
    }

    public static class HttpClientExtenstions
    {
        public static async Task<HttpResponseMessage> PostAsync(
            this HttpClient client,
            string jsonQuery)
        {
            using (Stream stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(jsonQuery);
                    writer.Flush();
                }
                return await client.PostAsync(client.BaseAddress, new StreamContent(stream));
            }
        }
    }
}
