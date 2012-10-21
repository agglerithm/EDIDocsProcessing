 
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsOut
{
    using Common.EDIStructures;

    public interface ICreateEdiContentFrom<T>
    {
        EDIXmlTransactionSet BuildFromMessage(T order); 
        ISegmentFactory SegmentFactory { get; }
        BusinessPartner GetBusinessPartner();
        bool CanProcess(IEdiMessage msg);
    }
}