using System;
using System.Collections.Generic;
using System.Linq;
using EDIDocsProcessing.Common;

namespace EDIDocsProcessing.Core.DocsIn.impl
{
    public class Splitter : ISplitter
    {
        private readonly List<IEnumerable<Segment>> _splitSegments = new List<IEnumerable<Segment>>();

        public List<IEnumerable<Segment>> SplitByGroup(List<Segment> segments)
        {
            segments.RemoveWhile(seg => seg.Label != EDIConstants.GroupLabel);
            return split(segments, EDIConstants.GroupLabel, EDIConstants.GroupClose);
        }

        public List<IEnumerable<Segment>> SplitByInterchange(List<Segment> segments)
        {
            return split(segments, EDIConstants.InterchangeLabel, EDIConstants.InterchangeClose);
        }

        public List<IEnumerable<Segment>> SplitByDocument(List<Segment> segments)
        {
            segments.RemoveWhile(seg => seg.Label != EDIConstants.DocumentLabel);
            return split(segments, EDIConstants.DocumentLabel, EDIConstants.DocumentClose);
        }

        private List<IEnumerable<Segment>> split(ICollection<Segment> segments, string labelType, string closeType)
        {
            var ctxtCount = segments.ToList().Count(s => s.Label == labelType);
            var splitSegments = new List<IEnumerable<Segment>>();
            for (int i = 0; i < ctxtCount; i++)
            {
                splitSegments.Add(extract(segments, labelType, closeType));
            }
            return splitSegments;
        }

        private static IList<Segment> extract(ICollection<Segment> segments, string labelType, string closeType)
        {
            IList<Segment> lst;
            try
            {
                lst = segments.TakeWhile(s => s.Label != closeType).ToList();

            }
            catch
            {
                throw new Exception(string.Format("{0} label was missing!", labelType));
            }


            lst.ToList().ForEach(segment => segments.Remove(segment));
            if (segments.Count == 0)
                throw new Exception(string.Format("{0} label was missing!", labelType));

            var close_seg = segments.First();

            if (close_seg.Label != closeType)
                throw new Exception("This should never happen!");


            lst.Add(close_seg);
            segments.Remove(close_seg);
            return lst;
        }
    }
}