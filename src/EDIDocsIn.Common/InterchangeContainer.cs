using System;
using System.Collections.Generic;
using System.Linq;
using AFPST.Common.Extensions;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;

namespace EDIDocsProcessing.Common
{
    public class InterchangeContainer : IEdiInContainer
    {
        private EdiSegmentCollection _segments;
        private IList<GroupContainer> _groups = new List<GroupContainer>();

        public InterchangeContainer(EdiSegmentCollection segs)
        {
            _segments = segs;
            var seg = _segments.FirstWith(SegmentLabel.InterchangeLabel);
            if (seg == null) return;
            var arr = seg.GetElements(segs.ElementDelimiter);
            SenderId = arr[6].Trim();
            ControlNumber = arr[10].Trim();
        }

        protected string ControlNumber { get; private set; }

        public IEnumerable<GroupContainer> Groups
        {
            get { return _groups; } 
        }

        public string SenderId { get; private set; }
        public void AddSegments(EdiSegmentCollection segs)
        {
            _segments = segs;
        }

        public IEnumerable<Segment> InnerSegments
        {
            get
            {
                return
                    _segments.SegmentList.Where(
                        s =>
                        s.Label.Value != SegmentLabel.InterchangeLabel.Value && s.Label.Value != SegmentLabel.InterchangeClose.Value);
            }
        }

        public IEdiInContainer CreateChild(EdiSegmentCollection segs)
        {
            var child = new GroupContainer(segs);
            _groups.Add(child);
            return child;
        }


        public void AddGroups(IEnumerable<GroupContainer> groups)
        {
            _groups.Clear();
            _groups.AddRange(groups);
        }
    }
}