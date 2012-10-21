using System;
using System.Collections.Generic;
using System.Linq;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Common.Extensions;

namespace EDIDocsProcessing.Common
{
    public class HierarchySplitter : IHierarchySplitter
    { 

        public IEnumerable<GroupContainer> SplitByGroup(EdiSegmentCollection segments, InterchangeContainer isa)
        {
            var groups =   split(segments, SegmentLabel.GroupLabel, SegmentLabel.GroupClose, isa).Select(c => (GroupContainer)c);
            groups.ForEach(g => g.AddDocuments(SplitByDocument(new EdiSegmentCollection(g.InnerSegments, segments.ElementDelimiter), g)));
            return groups;
        }

        public IEnumerable<InterchangeContainer> SplitByInterchange(EdiSegmentCollection segments)
        {
            var interchanges = split(segments, SegmentLabel.InterchangeLabel, SegmentLabel.InterchangeClose, new EdiFileContainer(segments)).Select(i => (InterchangeContainer)i); 
            interchanges.ForEach(i => i.AddGroups(SplitByGroup(new EdiSegmentCollection(i.InnerSegments, segments.ElementDelimiter), i)));
            return interchanges;
        }

        public IEnumerable<DocContainer> SplitByDocument(EdiSegmentCollection segments, GroupContainer parent)
        { 
            var docs = split(segments, SegmentLabel.DocumentLabel, SegmentLabel.DocumentClose, parent).Select(c => (DocContainer)c);
            return docs;
        }

        private IEnumerable<IEdiInContainer> split(EdiSegmentCollection segments, SegmentLabel labelType, 
            SegmentLabel closeType, IEdiInContainer parentContainer)  
        {
            IList<IEdiInContainer> returnList = new List<IEdiInContainer>();
            IList<Segment> segs = parentContainer.InnerSegments.ToList();
            segs =  RemoveNullSegments(segs);
            var ctxtCount = segs.Where(s => s != null).ToList().Count(s => s.Label == labelType); 
            for (int i = 0; i < ctxtCount; i++)
            {

                returnList.Add(parentContainer.CreateChild(extract(new EdiSegmentCollection(segs, segments.ElementDelimiter), labelType, closeType, i)));
            }
            return returnList;
        }

        private static EdiSegmentCollection extract(EdiSegmentCollection segments, SegmentLabel labelType, SegmentLabel closeType, int ndx)
        {
            IList<Segment> lst;
            var segs = segments.SegmentList.ToList();

            lst = getPreviousSegmentsToStrip(segments, closeType, ndx, labelType);


            lst.ToList().ForEach(segment => segs.Remove(segment));


            if (segs.Count == 0)
                throw new Exception(string.Format("{0} label was missing!", labelType));

            var close_seg = segs.First();

            if(ndx > 0)
                segs.Remove(close_seg);

            lst = segs.TakeWhile(s => s.Label.Value != closeType.Value).ToList();

            lst.ToList().ForEach(segment => segs.Remove(segment));

            if (segs.Count == 0)
                throw new Exception(string.Format("{0} label was missing!", labelType));

            close_seg = segs.First();

            if (close_seg.Label != closeType)
                throw new Exception("This should never happen!");


            lst.Add(close_seg); 
            return new EdiSegmentCollection(lst,segments.ElementDelimiter);
        }

        private static IList<Segment> getPreviousSegmentsToStrip(EdiSegmentCollection segments, SegmentLabel closeType, int ndx, SegmentLabel labelType)
        {
            IList<Segment> lst;
            try
            {
                int counter = 0;
                lst = segments.SegmentList.TakeWhile(s =>
                                                         {
                                                             if (s.Label == closeType)
                                                                 counter++;
                                                             return counter < ndx;
                                                         }).ToList();

            }
            catch
            {
                throw new Exception(string.Format("{0} label was missing!", labelType));
            }
            return lst;
        }

        private static IList<Segment> RemoveNullSegments(ICollection<Segment> segments)
        {
            return segments.Where(seg => seg != null).ToList();
        }
    }
}