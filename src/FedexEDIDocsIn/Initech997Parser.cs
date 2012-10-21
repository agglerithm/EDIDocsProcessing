namespace InitechEDIDocsIn
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AFPST.Common.Extensions;
    using AFPST.Common.Services.Logging;
    using EDIDocsProcessing.Common;
    using EDIDocsProcessing.Common.DTOs;
    using EDIDocsProcessing.Common.EDIStructures;
    using EDIDocsProcessing.Core.DocsIn.impl;
    using EDIDocsProcessing.Core.DocsOut;

    public class Initech997Parser : IDocumentParser
    {
        private readonly IReceiptAcknowledgementRepository _ackRepo;
        private readonly IGeneric997Parser _parser;

        public Initech997Parser(IReceiptAcknowledgementRepository ackRepo, IGeneric997Parser parser)
        {
            _ackRepo = ackRepo;
            _parser = parser;
        }

        public DocumentRecordPackage ProcessSegmentList(List<Segment> segList)
        {
            var acks = _parser.ProcessSegmentList(segList);
            acks.BusinessPartnerNumber = BusinessPartner.Initech.Number;
            acks.BusinessPartnerCode = BusinessPartner.Initech.Code;
            acks.BusinessPurpose = "Initech Acknowledgement";
            _ackRepo.SaveAcks(acks);

            return new DocumentRecordPackage(acks, null, null);
        }

        public bool CanProcess(BusinessPartner partner, string docType)
        {
            return partner == BusinessPartner.Initech && docType == "997";
        }
    }
}