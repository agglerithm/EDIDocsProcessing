using System.Collections.Generic;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Core.DocsIn.impl;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsIn
{
    public interface IAddressParser
    {
        int ProcessAddresses(List<Segment> segments, IEdiMessageWithAddress ediMessage);
        int ProcessAddresses(List<Segment> segList, ChangeOrderMessage ediMessage);
    }
}