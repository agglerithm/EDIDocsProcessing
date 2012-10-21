using System;
using System.Collections.Generic;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;

namespace EDIDocsProcessing.Common
{
    public class EdiSegmentCollection
    {
        public EdiSegmentCollection(IEnumerable<Segment> segs, string elDelimiter)
        {
            ElementDelimiter = elDelimiter;
            SegmentList = segs;
        }

        public string ElementDelimiter { get; private set; }

        public IEnumerable<Segment> SegmentList
        {
            get; private set;
        }

        public Segment FirstWith(SegmentLabel label)
        {
            return SegmentList.FindSegmentByLabel(label);
        }
    }
}