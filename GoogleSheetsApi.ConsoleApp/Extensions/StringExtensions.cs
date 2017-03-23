using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsApi.Extensions
{
    public static class StringExtensions
    {
        public static List<string> ToList(this string s, char delimiter = ' ')
        {
            return s.Split(delimiter).ToList();
        }
    }
}
