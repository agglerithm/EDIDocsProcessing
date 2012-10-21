using System;
using System.Collections.Generic;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Core.DocsIn.impl;

namespace EDIDocsProcessing.Core.DocsIn
{
    public interface ISegmentExtractor
    {
        void RegisterSegmentList(IList<Segment> segmentList);
        void ValidateSegmentList();
        List<Segment> ExtractSegment(
                                     Func<Segment, bool> searchPredicate, Func<Segment, bool> stopPredicate);

        int ExtractedCount { get;   }
        void Clear();
        Segment ExtractSegment(Func<Segment, bool> searchPredicate);
    }
}