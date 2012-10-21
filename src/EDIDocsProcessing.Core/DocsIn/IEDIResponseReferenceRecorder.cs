using System;
using System.Collections.Generic;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Core.DocsIn.impl;

namespace EDIDocsProcessing.Core.DocsIn
{
    using Common;

    public interface IEDIResponseReferenceRecorder
    {
        IEnumerable<Segment> MemorizeOuterReferences(List<Segment> segList, string controlNumber,
            string elDelimiter, BusinessPartner partner);
        void MemorizeInnerReferences(IList<DocumentLineItemEntity> refs, string controlNumber, BusinessPartner partner);
         
        IEnumerable<ResponseElementEntity> GetResponseValues();
        IEnumerable<DocumentLineItemEntity> GetLines();
        void Clear();
    }
}