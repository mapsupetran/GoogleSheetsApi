using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsApi.Extensions
{
    public static class EnumerableExtensions
    {
        public static string ToDelimitedString(this IEnumerable<string> strings, char delimiter = ' ')
        {
            var sb = new StringBuilder();
            foreach (var str in strings)
            {
                sb.Append(string.Format("{0} ", str));
            }
            return sb.ToString().Trim();
        }
    }
}
