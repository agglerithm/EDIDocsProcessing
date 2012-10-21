using EDIDocsProcessing.Core;
using EdiMessages;

namespace EDIDocsOut
{
    public class FedexSelector:ISelector
    {
        public bool Accept(IEdiMessage message)
        {
            return (message.BusinessPartnerCode == "FEDEX") ;
            
        }
    }
}