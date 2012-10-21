using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdiMessages
{
    public static class Extensions
    {
        public static string PrintAll<T>(this IEnumerable<T> items) 
        {
            if (items == null) return string.Empty;
            var result = new StringBuilder();
            items.ToList().ForEach(x => result.Append(x + Environment.NewLine));
            return result.ToString();
        }
        
        public static void Add(this IEnumerable<ShippedLine> lines, ShippedLine line)
        {
            ((List<ShippedLine>)lines).Add(line); 
        }
    }
}