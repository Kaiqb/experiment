namespace RepoTools
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
    }
}
