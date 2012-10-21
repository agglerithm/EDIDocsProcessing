using System.Collections.Generic;
using EDIDocsProcessing.Core.DocsIn.impl;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsIn
{
    public interface IPOLineParser
    {
        int SegmentCount { get; }
        void ProcessLines(List<Segment> lst, IEdiMessage doc);
    }
}