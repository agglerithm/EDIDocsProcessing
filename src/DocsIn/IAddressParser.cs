using System.Collections.Generic;
using EDIDocsProcessing.Core.DocsIn.impl;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsIn
{
    public interface IAddressParser
    {
        void ProcessAddresses(List<Segment> segments, IEdiMessage ediMessage);
        int SegmentCount { get;   }
    }
}