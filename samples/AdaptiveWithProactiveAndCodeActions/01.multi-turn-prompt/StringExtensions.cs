using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveScratchBot
{
    public static class StringExtensions
    {
        public static string Oxford(this IEnumerable<string> items, string separator, string conjunction)
        {
            if (items is null || !items.Any()) return string.Empty;

            var list = items.ToList();
            switch (list.Count)
            {
                case 1: return list[0];
                case 2: return $"{list[0]} {conjunction} {list[1]}";
                default:
                    var sb = new StringBuilder();

                    for (int i = 0; i<list.Count-1; i++)
                    {
                        sb.Append(list[i]);
                        sb.Append(separator);
                    }
                    sb.Append(conjunction + " ");
                    sb.Append(list.Last());

                    return sb.ToString();
            }
        }
    }
}
