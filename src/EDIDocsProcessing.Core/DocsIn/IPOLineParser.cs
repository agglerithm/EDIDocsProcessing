using System;
using System.Collections.Generic;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Core.DocsIn.impl;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsIn
{
    public interface IPOLineParser
    {
        int SegmentCount { get; set; }
        IList<DocumentLineItemEntity> ProcessLines(List<Segment> lst, IEdiMessage doc);
        CustomerOrderLine CreateLine(Segment lineSeg);
    }
}