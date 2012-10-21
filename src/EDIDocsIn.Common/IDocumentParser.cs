using System.Collections.Generic;
using EDIDocsProcessing.Common.EDIStructures;

namespace EDIDocsProcessing.Common
{
    public interface IDocumentParser 
    {
        DocumentRecordPackage ProcessSegmentList(List<Segment> segList);
        bool CanProcess(BusinessPartner partner, string docType);
    }
}