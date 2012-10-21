using System;
using System.Collections.Generic;
using System.Linq;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Common.Extensions;

namespace EDIDocsProcessing.Common
{
    public static class EnumerableExtensions
    {
        public static List<Segment> RemoveGroup(this IList<Segment> source, Func<Segment, bool> findPredicate, Func<Segment, bool> stopPredicate)
        {
            return ((List<Segment>) source).RemoveGroup(findPredicate, stopPredicate);
        }

        public static List<Segment> RemoveGroup(this List<Segment> source, Func<Segment,bool> findPredicate, Func<Segment, bool> stopPredicate)
        {
            var subList = new List<Segment>();
            var returnList = new List<Segment>();
            bool stop = false;
            source.ForEach(s =>
                               {
                                   if (!stop)
                                   {
                                       stop = stopPredicate(s);
                                       subList.Add(s);
                                   } 
                               });
            subList.ForEach(s =>
                                {
                                    if (findPredicate(s))
                                    {
                                        returnList.Add(s);
                                        source.Remove(s);
                                    }
                                  

                                });
            return returnList;
        }

        public static List<List<Segment>> SplitLoop(this IEnumerable<Segment> lst)
        {
            var listList = new List<List<Segment>>();
            var looplabel = lst.First().Label;
            var subList = new List<Segment>();
            lst.ForEach(seg => build_list_list(seg, looplabel, ref subList, listList));
            listList.Add(subList);
            return listList;
        }

        private static void build_list_list(Segment seg, SegmentLabel looplabel, ref List<Segment> subList, List<List<Segment>> listList)
        {
            if(seg.Label == looplabel && subList.Count > 0)
            { 
                listList.Add(subList);
                subList = new List<Segment>();  
            }
            subList.Add(seg);
        }

        public static void RemoveWhile(this  List<Segment> segs, Func<Segment, bool> predicate)
        {
            var lst = segs.TakeWhile(predicate);
            lst.ForEach(seg => segs.Remove(seg));  
        }

        public static Segment FindSegmentByLabel(this IEnumerable<Segment> segs, 
                                                 SegmentLabel labelValue)
        {
            return segs.Find(seg =>  seg.Label == labelValue);
        }

        public static IEnumerable<Segment> FindSegmentsByLabel(this IEnumerable<Segment> segs,
                                         SegmentLabel labelValue)
        {
            return segs.Where(seg => seg.Label == labelValue);
        }

        public static void SetContexts(this List<Segment> segList)
        {
            var context = "root";
            foreach (var seg in segList)
            {
                if (seg.Label == SegmentLabel.PurchaseOrder)
                {
                    context = seg.Context;
                }
                if (seg.Label == SegmentLabel.ReferenceLabel)
                {
                    seg.Context = context;
                }
            }
        }

        public static DocumentLineItemEntity BuildInnerResponse(this IEnumerable<Segment> segList, int lineNumber)
        {
            if (segList == null) return null;
            IEnumerable<Segment> refs = from s in segList.ToList()
                                        where s.Label == SegmentLabel.ReferenceLabel
                                        select s;
            return refs.Count() == 0 
                ? 
                new DocumentLineItemEntity { LineIdentifier = lineNumber.ToString(), ResponseElements = new List<LineResponseElementEntity>() } 
                : 
                new DocumentLineItemEntity { LineIdentifier = lineNumber.ToString(), ResponseElements = refs.BuildLineResponseElements() };
        }

        public static IList<LineResponseElementEntity> BuildLineResponseElements(this IEnumerable<Segment> refSegs)
        {
            if (refSegs == null) return null;
            var responseEls = new List<LineResponseElementEntity>();
            refSegs.ForEach(s => add_line_response_element(s, responseEls));
            return responseEls;

        }

        private static void add_line_response_element(Segment seg, ICollection<LineResponseElementEntity> responseEls)
        {
            var elDelim = seg.Contents.Substring(3, 1);
            var arr = seg.GetElements(elDelim);
            responseEls.Add(new LineResponseElementEntity
            {
                ElementName = "REF02",
                Qualifier = arr[1],
                Value = arr[2]
            });
        }
    }
}