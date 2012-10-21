using System;
using System.Collections.Generic;
using EDIDocsProcessing.Common.EDIStructures;

namespace EDIDocsProcessing.Common
{
    public class EdiFileContainer : IEdiInContainer
    {
        private readonly IList<InterchangeContainer> _interchanges = new List<InterchangeContainer>();
        private EdiSegmentCollection _segments;

        public EdiFileContainer(EdiSegmentCollection segs)
        {
            _segments = segs;
        }
        public void AddSegments(EdiSegmentCollection segs)
        {
            CreateChild(segs);
        }

        public IEnumerable<Segment> InnerSegments
        {
            get { return _segments.SegmentList; }
        }

        public IEnumerable<InterchangeContainer> Interchanges
        {
            get { return _interchanges; } 
        }


        public IEdiInContainer CreateChild(EdiSegmentCollection segs)
        {
            var child = new InterchangeContainer(segs);
            _interchanges.Add(child);
            return child;
        }
 
    }
}