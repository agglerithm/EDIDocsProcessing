using System.Collections.Generic;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Core.DocsIn.impl;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsIn
{
    using Common;

    public interface IGenericDocumentParser  
    {
        string ElementDelimiter { get; }
        string SegmentDelimiter { get; set; }
        bool CanProcess(Segment header);
        void Process(List<Segment> segList);
        IEnumerable<ResponseElementEntity> ResponseValues { get;}
        IEnumerable<DocumentLineItemEntity> Lines { get;  }
        IEnumerable<Segment> MemorizeOuterReferences(List<Segment> segList, string controlNumber, BusinessPartner partner);
        void MemorizeInnerReferences(IList<DocumentLineItemEntity> refs, string controlNumber, BusinessPartner partner);
        void SetElementDelimiterFromHeader(Segment header);
    }
}