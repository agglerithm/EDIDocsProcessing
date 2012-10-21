using System;
using AFPST.Common.Services.Logging;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Core.DataAccess;
using FluentNHibernate;

namespace EDIDocsProcessing.Core.DocsOut.Impl
{
    public class ControlNumberRepository : Repository, IControlNumberRepository 
    {
        private IReceiptAcknowledgementRepository _ackRepo;
        public ControlNumberRepository(ISessionSource sessionSource, IReceiptAcknowledgementRepository ackRepo)
            : base(sessionSource)
        {
            _ackRepo = ackRepo;
        }

        public ISAEntity GetNextISA(string groupId, int partnerId)
        {
            var isa = new ISAEntity{GroupID = groupId, PartnerNumber = partnerId };

            try
            { 
                Save(isa);
            }
            catch (Exception ex)
            {
                Logger.Error(this, "Error creating control number." , ex);
                throw;
            }

            return isa;
        }

        public  DocumentEntity GetNextDocument(ISAEntity isa, int docType)
        {
            var doc = new DocumentEntity {ISAEntity = isa, DocumentID = docType };

            Save(doc);
            createAckRecord(doc);
            return doc;

        }

        private void createAckRecord(DocumentEntity doc)
        {
            try
            {
                var ack = new ReceiptAcknowledgement();
                ack.DocumentControlNumber = doc.ControlNumber;
                ack.DocumentType = doc.DocumentID;
                ack.DocumentSendDate = DateTime.Now;
                _ackRepo.SetPlaceholder(ack);

            }
            catch (Exception ex)
            {
                Logger.Error(this, "Could not create acknowledgement record for " + doc.ControlNumber + ".", ex);
            }
        }

        public DocumentEntity GetNextDocument(ISAEntity isa, int docType, string bol)
        {
            var doc = new DocumentEntity { ISAEntity = isa, DocumentID = docType, ERPID = bol };

            Save(doc);
            createAckRecord(doc);
            return doc;
        }

        public void Save(ISAEntity isa)
        {
            base.Save(isa);
        }
    }
}