using System.Collections.Generic;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.EDIStructures;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsIn.impl
{
    using Common;

    public interface IGeneric860Parser
    {
        bool CanProcess(Segment header);
        IEnumerable<ResponseElementEntity> ResponseValues { get; }
        IEnumerable<DocumentLineItemEntity> Lines { get; }
        string ElementDelimiter { get; }
        string SegmentDelimiter { get; }
        OrderChangeRequestReceivedMessage ProcessSegmentList(List<Segment> segList);
        IEnumerable<Segment> MemorizeOuterReferences(List<Segment> segList, string controlNumber, BusinessPartner partner);
        void MemorizeInnerReferences(IList<DocumentLineItemEntity> refs, string controlNumber, BusinessPartner partner);
    }
}