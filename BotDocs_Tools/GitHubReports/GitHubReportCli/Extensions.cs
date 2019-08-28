using System;
using System.Collections.Generic;
using System.Text;

namespace GitHubReportCli
{
    public static class Extensions
    {
        public static string Truncate(this string input, int maxLength)
        {
            return (input != null && input.Length > maxLength) ? input.Substring(0, maxLength) : input;
        }
    }
}
