using System;
using System.Collections.Generic;
using System.Linq;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;

namespace EDIDocsProcessing.Core.DocsIn.impl
{
    public class SegmentExtractor : ISegmentExtractor
    {
        private IList<Segment> _segList;
        private IList<Segment> _depletedList;

        public void RegisterSegmentList(IList<Segment> segmentList)
        {
            _segList = segmentList;
            _depletedList = segmentList.ToList();
        }

        public void ValidateSegmentList()
        {
            ensure_registered();
            if (_segList.Count != ExtractedCount)
                throw new InvalidEDIDocumentException("Not all segments were extracted from list!");
        }

        public List<Segment> ExtractSegment( Func<Segment, bool> searchPredicate, Func<Segment, bool> stopPredicate)
        {
            ensure_registered();
            return _depletedList.RemoveGroup(searchPredicate, stopPredicate); 
        }

        public void Clear()
        { 
                _segList = null;
                _depletedList = null; 
        }

        public Segment ExtractSegment(Func<Segment, bool> searchPredicate)
        {
            var lst = ExtractSegment(searchPredicate, s => false);
            if (lst.Count == 0) return null;
            return lst.First();
        }

        public int ExtractedCount
        {
            get
            {
                ensure_registered();
                return _segList.Count - _depletedList.Count;
            }
        }

        private void ensure_registered()
        {
            if (_segList == null)
                throw new InvalidEDIDocumentException("Segment list must be registered before SegmentExtractor is used!");
        }
    }
}