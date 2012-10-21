using System;
using System.Collections.Generic;
using System.Linq;

namespace EDIDocsProcessing.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> lst, Action<T> actn)
        {
            if(lst == null)
                throw new ApplicationException("Cannot call 'ForEach' on a non-existant list!");
             lst.ToList().ForEach(actn);
        }

        public static T Find<T>(this IEnumerable<T> lst, Predicate<T> actn)
        {
            return lst.ToList().Find(actn);
        }

    }
}
