using System.Collections.Generic;
using EDIDocsProcessing.Common.EDIStructures;

namespace EDIDocsProcessing.Common
{
    public interface IEdiInContainer
    {
        void AddSegments(EdiSegmentCollection segs);
        IEnumerable<Segment> InnerSegments { get; }
        IEdiInContainer CreateChild(EdiSegmentCollection segs);
    }
}