
namespace EDIDocsProcessing.Core.DocsOut.Impl
{
    using AFPST.Common.Infrastructure;
    using Common.EDIStructures;

    public class CreateEdiDocumentFrom<T> : ICreateEdiDocumentFrom<T>
    {
        private readonly IAFPSTConfiguration _configuration;

        public CreateEdiDocumentFrom(IAFPSTConfiguration configuration)
        {
            _configuration = configuration;
        }

        public EDITransmissionPackage CreateDocumentWith(ICreateEdiContentFrom<T> creator, T message)
        {

            var ediXmlInterchangeControl = new EDIXmlInterchangeControl(creator.SegmentFactory);

            EDIXmlTransactionSet ediTransactionSet = creator.BuildFromMessage(message);
             
            var isaControlNumber = ediTransactionSet.ISA.ControlNumber;
            var functionalID = ediTransactionSet.ISA.GroupID;

            ediXmlInterchangeControl.SetHeader(isaControlNumber, functionalID, _configuration.TestMode);

            ediXmlInterchangeControl.AddContent(ediTransactionSet);

            ediXmlInterchangeControl.SetFooter(isaControlNumber, 1);

            return new EDITransmissionPackage(ediXmlInterchangeControl,  creator.GetBusinessPartner());
        }
 
    }
}