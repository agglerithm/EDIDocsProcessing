using System;
using System.Collections.Generic;
using System.Linq; 
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsIn.impl;
using EdiMessages;

namespace InitechEDIDocsIn
{
    public interface IInitech850LineParser
    {
        IList<DocumentLineItemEntity> ProcessLines(List<Segment> lst, IEdiMessage doc);
        int SegmentCount { get;  }
    }

    public class Initech850LineParser : IInitech850LineParser
    {
        private readonly IList<DocumentLineItemEntity> _lines = new List<DocumentLineItemEntity>();
        private readonly IPOLineParser _poParser;

        public Initech850LineParser(IPOLineParser poParser)
        {
            _poParser = poParser;
        }

        public   IList<DocumentLineItemEntity> ProcessLines(List<Segment> lst, IEdiMessage doc)
        {
            _lines.Clear();
            var co = doc as OrderRequestReceivedMessage;
            if (co == null) return null;
            lst.RemoveWhile(seg => seg.Label != SegmentLabel.PurchaseOrder);
            _poParser.ProcessLines(lst, doc);
            CustomerOrderLine line;
            do
            {
                line = build_line(lst, SegmentLabel.SummaryLabel);
                if(line != null) co.Add(line); 
            
            } while (line != null) ;
            return _lines;
        }

        private CustomerOrderLine build_line(List<Segment> lst, SegmentLabel terminator)
        {
            
            var nextSeg = lst.FirstOrDefault();
            var segmentCount = 0;
            if (nextSeg == null) return null;
            if (nextSeg.Label.Text == terminator.Text) return null;
            CustomerOrderLine lineItem = _poParser.CreateLine(nextSeg);
            if (lineItem == null) return null; 
            lst.Remove(nextSeg);

            lineItem.ItemId = "";

            IEnumerable<Segment> remainingSegments = lst.TakeWhile(seg => seg.Label == SegmentLabel.ReferenceLabel ||
                                                                           seg.Label == SegmentLabel.ScheduleLabel);

            var line = remainingSegments.BuildInnerResponse(lineItem.LineNumber);

            segmentCount += line.ResponseElements.Count();

            line.ResponseElements.Add(new LineResponseElementEntity { ElementName = "PO1", Qualifier = "IN", Value = lineItem.CustomerPartNumber.SafeTrim() });

            _lines.Add(line);

            segmentCount += process_schedules(remainingSegments);

            lst.RemoveRange(0, segmentCount);

            _poParser.SegmentCount += segmentCount;

            return lineItem;
        }

        private int process_schedules(IEnumerable<Segment> remainingSegments)
        {
            IEnumerable<Segment> scheds = from s in remainingSegments.ToList()
                                          where s.Label == SegmentLabel.ScheduleLabel
                                          select s;
            return  scheds.Count();
        }

        public int SegmentCount { get { return _poParser.SegmentCount;  } }
    }
}