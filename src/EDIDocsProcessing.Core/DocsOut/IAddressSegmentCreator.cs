using AFPST.Common.Structures;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsOut
{
    public interface IAddressSegmentCreator
    {
        bool CanProcess(string addressType);
        void AddAddressSegmentsTo(EDIXmlMixedContainer container, ISegmentFactory factory, Address addr);
        BusinessPartner Partner { get;  }
    }
}