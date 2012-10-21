using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Common.EDIStructures
{
    public class EDITransmissionPackage
    {
        private readonly EDIXmlInterchangeControl _ic;
        private readonly BusinessPartner _bp;

        public EDITransmissionPackage(EDIXmlInterchangeControl ic, BusinessPartner bp)
        {
            _ic = ic;
            _bp = bp;
        }

        public EDIXmlInterchangeControl GetInterchangeControl()
        {
            return _ic;
        }

        public BusinessPartner GetBusinessPartner()
        {
            return _bp;
        }

    }
}