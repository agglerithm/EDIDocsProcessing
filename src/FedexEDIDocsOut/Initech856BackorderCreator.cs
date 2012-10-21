using EDIDocsProcessing.Common;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;
using EdiMessages;

namespace InitechEDIDocsOut
{
    using EDIDocsProcessing.Common.EDIStructures;

    public class Initech856BackorderCreator : Initech856Creator, ICreateEdiContentFrom<OrderIsBackorderedMessage>
    {
        public Initech856BackorderCreator(IControlNumberRepository repo, 
                                        IIncomingDocumentsRepository docsRepo, 
                                        IBusinessPartnerSpecificServiceResolver serviceResolver) 
            : base(repo, docsRepo, serviceResolver) { }

        public EDIXmlTransactionSet BuildFromMessage(OrderIsBackorderedMessage message)
        {
            return CreateTransactionSet(message);
        }

        public new bool CanProcess(IEdiMessage msg)
        {
            return msg.GetType() == typeof(OrderIsBackorderedMessage) && msg.BusinessPartnerCode == BusinessPartner.Initech.Code;
        }
    }
}