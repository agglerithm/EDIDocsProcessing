using System;
using System.Collections.Generic;
using System.Linq;

namespace EDIDocsOut.Common
{
    public static class EnumerableExtensions
    {
        
        public static void RemoveWhile(this  List<Segment> segs, Func<Segment, bool> predicate)
        {
            var lst = segs.TakeWhile(predicate);
            lst.ToList().ForEach(seg => segs.Remove(seg));  
        }

        public static Segment FindSegmentByLabel(this IEnumerable<Segment> segs, 
                                                 string label_value)
        {
            return segs.ToList().Find(seg =>  seg.Label == label_value);
        }
  
    }
}