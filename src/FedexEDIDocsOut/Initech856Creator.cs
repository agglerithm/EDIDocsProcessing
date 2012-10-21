using System;
using System.Collections.Generic;
using System.Linq;
using AFPST.Common.Services.Logging;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;
using EdiMessages;

namespace InitechEDIDocsOut
{
    public class Initech856Creator : ICreateEdiContentFrom<OrderHasBeenShippedMessage>
    {        
        private readonly IControlNumberRepository _repo;
        private readonly IIncomingDocumentsRepository _docsRepo; 
        private readonly EdiXmlBuildValues _ediXmlBuildValues;
        private readonly ISegmentFactory _segmentFactory;

        public Initech856Creator(IControlNumberRepository repo, 
            IIncomingDocumentsRepository docsRepo, 
            IBusinessPartnerSpecificServiceResolver serviceResolver)
        {
            _repo = repo;
            _docsRepo = docsRepo;
            _segmentFactory = serviceResolver.GetSegmentFactoryFor(BusinessPartner.Initech);
            _ediXmlBuildValues = _segmentFactory.BuildValues; 
        }

        public bool CanProcess(IEdiMessage msg)
        {
            return msg.GetType() == typeof(OrderHasBeenShippedMessage) && msg.BusinessPartnerCode == BusinessPartner.Initech.Code;
        }

        public EDIXmlTransactionSet BuildFromMessage(OrderHasBeenShippedMessage message)
        {
            return CreateTransactionSet(message);
        }

        public ISegmentFactory SegmentFactory
        {
            get { return _segmentFactory; }
        }

        public BusinessPartner GetBusinessPartner()
        {
            return BusinessPartner.Initech;
        }

        protected EDIXmlTransactionSet CreateTransactionSet(OrderShippingInfo message)
        {
            if(message.Lines.Count() == 0)
                throw new Exception("Shipped order contains no line items!");
 
            var originalDoc = _docsRepo.GetByDocumentControlNumberAndPartnerID(message.ControlNumber.CastToInt(), BusinessPartner.Initech.Number); 
            if (originalDoc == null)
                throw new Exception(string.Format("Control number {0} not found for business partner {1}.",
                    message.ControlNumber, message.BusinessPartnerNumber));
            var orderTypeElement = originalDoc.ResponseElements.Find(e => e.ElementName == "REF02" && (e.Qualifier == Qualifier.MutuallyAssignedCode.Value));
            var priorityElement = originalDoc.ResponseElements.Find(e => e.ElementName == "REF02" && (e.Qualifier == Qualifier.ServiceLevelNumber.Value));

            var doc = new EDIXmlTransactionSet(_segmentFactory) {ISA = _repo.GetNextISA(GroupTypeConstants.AdvanceShipNotice, BusinessPartner.Initech.Number)};


            var docDef = _repo.GetNextDocument(doc.ISA, 856);

            doc.SetHeader("856", docDef.ControlNumber);

            docDef.ERPID = message.BOL;
            _repo.Save(docDef.ISAEntity);

            doc.AddSegment(get_begin_segment(message) );

            if (message.MaxDateShipped() > DateTime.MinValue)
                doc.AddSegment(_segmentFactory.GetDateTimeSegment(EDIDateQualifiers.Shipped, message.MaxDateShipped()));

            var hl = HierarchicalLevelLoopWrapper.BuildWrapper("O", _segmentFactory, true); 
            hl.AddSegment(_segmentFactory.GetPurchaseOrderReference(message.CustomerPO.Trim()));

            if(orderTypeElement != null) 
                hl.AddSegment(_segmentFactory.GetReferenceIDSegment(orderTypeElement.Qualifier, orderTypeElement.Value)); 

            if(priorityElement != null)
                hl.AddSegment(_segmentFactory.GetReferenceIDSegment(priorityElement.Qualifier, priorityElement.Value)); 


            var addrs = new AddressLoop(_segmentFactory);

            if(message.ShipToAddress.AddressName.IsNullOrEmpty())
                message.ShipToAddress.AddressName = "FEDERAL EXPRESS";

            addrs.AddAddress(message.ShipToAddress, Qualifier.EmptyQualifier);

            addrs.AddAddress(message.ShipFromAddress, Qualifier.EmptyQualifier);

            hl.AddLoop(addrs);

            message.Lines.ForEach(l => add_line(hl,l,originalDoc.LineItems));

            hl.AddTo(doc);

            doc.AddSegment(_segmentFactory.GetTransactionTotal(message.Lines.Count()));

            doc.SetFooter();


            return doc;
        }

        private EDIXmlSegment get_begin_segment(OrderShippingInfo orderShippingInfo)
        {
            var begin = new EDIXmlSegment("BSN", _ediXmlBuildValues);
            begin.Add(new EDIXmlElement("BSN01", "00", _ediXmlBuildValues));
            begin.Add(new EDIXmlElement("BSN02", orderShippingInfo.BOL, _ediXmlBuildValues));
            begin.Add(new EDIXmlElement("BSN03", DateTime.Today.ToString("yyyyMMdd"), _ediXmlBuildValues));
            begin.Add(new EDIXmlElement("BSN04", DateTime.Now.ToString("HHmmss"), _ediXmlBuildValues));
            return begin;
        }

        private void add_line(HierarchicalLevelLoopWrapper hl, ShippedLine line, IEnumerable<DocumentLineItemEntity> linesFromOriginalOrder)
        {

            var lineFromOriginalOrder = linesFromOriginalOrder.Find(l => l.LineIdentifier == line.LineNumber);

            var parts = lineFromOriginalOrder.GetResponseElementsMatching("PO1"); 
            if (parts.Count > 0)
                line.CustomerPartNo = parts[0].Value;
            var itemLevel = hl.AddLevel("I");
            itemLevel.AddSegment(_segmentFactory.GetShipmentLineItem(line.LineNumber, line.CustomerPartNo.Trim(),
                line.ItemID));
            itemLevel.AddSegment(_segmentFactory.GetLineItemShipmentDetail(line.LineNumber, line.QtyShipped,
                line.QtyOrdered, line.QtyShippedToDate, line.GetStatus()));
            itemLevel.AddSegment(_segmentFactory.GetRoutingCarrierDetails("", "", "", "P", BusinessPartner.Initech.Code, ""));
            if (line.QtyShipped != line.TrackingNumbers.Count()) throw new InvalidOperationException(string.Format("Count of tracking numbers for line {0} did not match qty shipped", line.LineNumber));
            line.TrackingNumbers.ForEach(tn =>
                itemLevel.AddSegment(_segmentFactory.GetReferenceIDSegment(Qualifier.AirBillNumber.Value.Replace(" ", ""), tn)));
            lineFromOriginalOrder.GetResponseElementsMatching("REF02").ForEach(r => add_ref_segment(itemLevel, r));
            if (line.DateShipped == DateTime.MinValue)
                line.DateShipped = DateTime.Today.AddDays(1);
            itemLevel.AddSegment(_segmentFactory.GetDateTimeSegment(EDIDateQualifiers.Shipped, line.DateShipped));
        }

        private void add_ref_segment(EDIXmlMixedContainer hl, LineResponseElementEntity r)
        {
            hl.AddSegment(_segmentFactory.GetReferenceIDSegment(r.Qualifier, r.Value));
            Logger.Info(this, "Qualifier: " + r.Qualifier + "; Value: " + r.Value);
        }
    }
}
