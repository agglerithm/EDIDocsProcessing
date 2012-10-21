using System;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace InitechEDIDocsOut
{
    public class InitechBuildValueFactory : IBuildValueFactory
    {
        private EdiXmlBuildValues _buildValues;
        public EdiXmlBuildValues GetValues()
        {
            if(_buildValues == null)
                _buildValues = new EdiXmlBuildValues()
                                   {
                                       ElementDelimiter = "~",
                                       SegmentDelimiter = "\n",
                                       FunctionGroupReceiverID = "055001924VA",
                                       InterchangeReceiverQualifier = "ZZ",
                                       InterchangeReceiverID = "055001924VA",
                                       InterchangeSenderQualifier = "12",
                                       InterchangeSenderID = "EEC5122516063",
                                       Transport = TransportAgent.Initech
                                   };
            return _buildValues;
        }

        public bool For(BusinessPartner partner)
        {
            return partner == BusinessPartner.Initech;
        }
    }
}