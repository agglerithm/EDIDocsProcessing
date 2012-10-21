using System;
using System.Collections.Generic;
using System.Linq;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core.DocsIn.impl;
using EdiMessages; 

namespace EDIDocsProcessing.Core.DocsIn
{
    using AFPST.Common.Services.Logging;

    public class MessageGenerator : IMessageGenerator
    {
        private readonly IIncomingDocumentsRepository _docsRepo;

        public MessageGenerator(IIncomingDocumentsRepository docsRepo)
        {
            _docsRepo = docsRepo;
        }

        public IEnumerable<IEdiMessage> GenerateMessages(EdiFileInfo fileInfo, BusinessPartner partner)
        {
            return buildMessagesFromFileInfo(fileInfo, partner);
        }

        private IEnumerable<IEdiMessage> buildMessagesFromFileInfo(EdiFileInfo fileInfo, BusinessPartner partner)
        {
            var docs = fileInfo.Documents;
            if (docs == null)
                throw new InvalidOperationException("File for partner " + partner.Text + " has no documents.");
            var msgs = new List<IEdiMessage>();
            docs.ForEach(d => msgs.Add(getMessageFrom(d, partner)));
            return msgs;
        }

        private IEdiMessage getMessageFrom(DocContainer docContainer, BusinessPartner partner)
        { 
            var parser = docContainer.ParserFor(partner);
            var package = parser.ProcessSegmentList(docContainer.InnerSegments.ToList());
            var docDto = create_document_entity(docContainer, partner); 
            add_value(docDto, package.ResponseValues);
            add_lines(docDto, package.Lines);
            _docsRepo.Save(docDto);
            return package.Message;
        }

        private void add_lines(DocumentInDTO doc, IEnumerable<DocumentLineItemEntity> lines)
        {
            var val = lines.Where(l => l.DocumentInDTO.ControlNumber == doc.ControlNumber);
            if (val != null)
            {
                val.ToList().ForEach(l => doc.AddLineItem(l.LineIdentifier, l.ResponseElements));
            }
        }

        private static void add_value(DocumentInDTO doc, IEnumerable<ResponseElementEntity> responseValues)
        {
            var val = responseValues.Where(v => v.DocumentInDTO.ControlNumber == doc.ControlNumber);
            if (val != null)
            {
                val.ToList().ForEach(v => doc.AddResponseElement(v.ElementName, v.Value, v.Qualifier));
            }
        }

        private DocumentInDTO create_document_entity(DocContainer document, BusinessPartner partner)
        {
            var group = document.ParentGroup;
            return new DocumentInDTO()
                       {
                           DocumentID = document.DocType.CastToInt(),
                           ControlNumber = document.ControlNumber,
                           ISAControlNumber = group.ControlNumber.CastToInt(),
                           DateSent = group.DateSent, 
                           GroupID = group.GroupId,
                           PartnerNumber = partner.Number
                       };
        }
 
    }
}