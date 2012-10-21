using System.Collections.Generic;
using EDIDocsProcessing.Core.DocsIn.impl;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsIn
{
    public interface IDocumentParser
    {
        string ElementDelimiter { get; }
        string SegmentDelimiter { get; set; }
        bool CanProcess(Segment header);
        T Process<T>(List<Segment> seg_list) where T : IEdiMessage;
    }
}