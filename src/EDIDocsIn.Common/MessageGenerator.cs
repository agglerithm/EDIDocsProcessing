using System.Collections.Generic;
using System.Linq;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.Extensions;
using EdiMessages;
using FlexEDIDocsIn;

namespace EDIDocsProcessing.Common
{
    public class MessageGenerator : IMessageGenerator
    {
        private readonly IIncomingDocumentsRepository _docsRepo; 

        public IEnumerable<IEdiMessage> GenerateMessages(EdiFileInfo fileInfo, BusinessPartner partner)
        {
            return buildMessagesFromFileInfo(fileInfo, partner);
        }

        private IEnumerable<IEdiMessage> buildMessagesFromFileInfo(EdiFileInfo fileInfo, BusinessPartner partner)
        {
            var docs = fileInfo.Documents;
            var msgs = new List<IEdiMessage>();
            docs.ForEach(d => msgs.Add(getMessageFrom(d, partner)));
            return msgs;
        }

        private IEdiMessage getMessageFrom(DocContainer docContainer, BusinessPartner partner)
        {
            var parser = docContainer.ParserFor(partner);
            var package = parser.ProcessSegmentList(docContainer.InnerSegments.ToList());
            var docDto = create_document_entity(docContainer);
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

        private DocumentInDTO create_document_entity(DocContainer document)
        {
            var group = document.ParentGroup;
            return new DocumentInDTO()
                       {
                           DocumentID = document.DocType.CastToInt(),
                           ControlNumber = document.ControlNumber,
                           ISAControlNumber = group.ControlNumber.CastToInt(),
                           DateSent = group.DateSent,
                           PartnerNumber = BusinessPartner.FedEx.Number,
                           GroupID = group.GroupId
                       };
        }
 
    }
}