using System;

namespace Utilities
{

    public static class DateTimeOffsetExtensions
    {
        /// <summary>Returns a file-appropriate representation of the date and time.</summary>
        public static string AsShortDate(this DateTimeOffset date)
        {
            return $"{date.Year:0000}{date.Month:00}{date.Day:00}_{date.Hour:00}{date.Minute:00}";
        }
    }
}
