using System;
using System.Collections.Generic;
using System.Linq;
using AFPST.Common.Structures;
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
    public class 
        Initech810Creator : ICreateEdiContentFrom<InvoicedOrderMessage>
    {
        private readonly IControlNumberRepository _repo;
        private readonly IIncomingDocumentsRepository _docsRepo; 
        private readonly ISegmentFactory _segmentFactory;
        private readonly EdiXmlBuildValues _buildValues;

        public Initech810Creator(IControlNumberRepository repo,
            IIncomingDocumentsRepository docsRepo, IBusinessPartnerSpecificServiceResolver serviceResolver)
        {
            _repo = repo;
            _docsRepo = docsRepo;
            _segmentFactory = serviceResolver.GetSegmentFactoryFor(BusinessPartner.Initech);
            _buildValues = _segmentFactory.BuildValues;
        }

        public ISegmentFactory SegmentFactory
        {
            get { return _segmentFactory; }
        }

        public BusinessPartner GetBusinessPartner()
        {
            return BusinessPartner.Initech;
        }

        public bool CanProcess(IEdiMessage msg)
        {
            return msg.DocumentId == EdiDocumentTypes.Invoice.DocumentNumber && msg.BusinessPartnerCode == BusinessPartner.Initech.Code;
        }

        public EDIXmlTransactionSet BuildFromMessage(InvoicedOrderMessage orderMessage)
        {
            return create_transaction_set(orderMessage);
        }

        private EDIXmlTransactionSet create_transaction_set(InvoicedOrderMessage orderMessage)
        {
            if (orderMessage.Customer == null)
                throw new ApplicationException("There is no customer associated with invoiced order message "
                    + orderMessage.ControlNumber + ".  Cannot build 810.");  

 
            var orderDoc = _docsRepo.GetByDocumentControlNumberAndPartnerID(orderMessage.ControlNumber.CastToInt(), BusinessPartner.Initech.Number);
            if(orderDoc == null) 
                throw new Exception(string.Format("Control number {0} not found for business partner {1}.",
                    orderMessage.ControlNumber,orderMessage.BusinessPartnerNumber));
            var responseElements = orderDoc.ResponseElements.Where(e => e.ElementName == "REF02");

            var isa = _repo.GetNextISA(GroupTypeConstants.Invoice, BusinessPartner.Initech.Number); 
            if(orderMessage.LineCount() == 0)
                throw new Exception("Invoiced order contains no line items!");

            var doc = new EDIXmlTransactionSet(_segmentFactory) {ISA = isa};

            var docDef = _repo.GetNextDocument(isa, 810);

            doc.SetHeader("810", docDef.ControlNumber);

            docDef.ERPID = orderMessage.BOL;

            _repo.Save(docDef.ISAEntity);

            doc.AddSegment(get_begin_segment(orderMessage));

            //doc.AddSegment(_segmentFactory.GetCurrencySegment("II",orderMessage.CurrencyCode));

            if(responseElements != null)
                responseElements.ForEach(r => doc.AddSegment(_segmentFactory.GetReferenceIDSegment(r.Qualifier,
                    r.Value)));

            doc.AddLoop(get_address_loop(orderMessage));

           // doc.AddSegment(get_terms(orderMessage));

            doc.AddLoop(get_detail_loop(orderMessage,orderDoc.LineItems));

            doc.AddSegment(_segmentFactory.GetTotalMonetaryValue(orderMessage.GetTotal()));
            
            doc.AddSegment(_segmentFactory.GetTaxInformationSegment("SU",orderMessage.SalesTax));

            doc.AddSegment(_segmentFactory.GetCarrierDetail("P","Initech"));

            doc.AddSegment(_segmentFactory.GetTransactionTotal(orderMessage.LineCount()));


            doc.SetFooter();

            return doc;
        }

//        private EDIXmlSegment get_terms(InvoicedOrderMessage message)
//        {
//            TermsOfSale terms = message.Customer.Terms;
//            return _segmentFactory.GetTerms("14", terms.DiscountPercent, 
//                DateTime.Today.AddDays(terms.DiscountDays), DateTime.Today.AddDays(terms.NetDays));
//        }

        private   EDIXmlMixedContainer get_detail_loop(InvoicedOrderMessage orderMessage, IList<DocumentLineItemEntity> lines)
        { 
            var loop = new EDIXmlMixedContainer("IT1");
            orderMessage.LineItems.ForEach(l => add_line(loop, l, lines));
            return loop;
        }

        private void add_line(EDIXmlMixedContainer container, InvoicedOrderLine line, 
            IList<DocumentLineItemEntity> lines)
        {
            var orderLine = lines.Find(l => l.LineIdentifier == line.LineNumber.ToString());
            IDictionary<Qualifier, string> values = new Dictionary<Qualifier, string>
                                                     {
                                                         {Qualifier.InvoiceVendorPart, line.ItemID},
                                                         {Qualifier.PartDescription, line.ItemDescription},
                                                         {Qualifier.PONumber, line.CustomerPO},
                                                         {Qualifier.EmptyQualifier, ""} ,
                                                         {Qualifier.POLineNumber, get_formatted_po_line(line.LineNumber)}
                                                         
                                                     };
            container.AddSegment(_segmentFactory.GetLineItemInvoiceDetail(line.LineNumber.ToString(), 
                line.Quantity, line.Price,   values));
            if (orderLine == null) return;
            orderLine.ResponseElements.ForEach(r => 
                container.AddSegment(_segmentFactory.GetReferenceIDSegment(r.Qualifier,r.Value)));
        }

        private string get_formatted_po_line(int number)
        {
            return string.Format("{0}001", number.ToString("0#"));
        }

        private   EDIXmlMixedContainer get_address_loop(InvoicedOrderMessage orderMessage)
        {
            var loop = new EDIXmlMixedContainer("N1");
            if (orderMessage.ShipToAddress == null && orderMessage.GetBillToAddress() == null)
                throw new ApplicationException("There are no bill to address associated with customer "
                    + orderMessage.GetCustomerId() + ".  Cannot build 810.");
            add_address(loop, orderMessage.ShipFromAddress, Qualifier.AssignedByBuyer.Value,
                        BusinessPartner.Initech.VendorIdOfAfp);
            add_address(loop, orderMessage.ShipToAddress,"","");
            add_address(loop, orderMessage.GetBillToAddress(), "", ""); 

            return loop;
        }

//        private EDIXmlSegment get_vendor_id()
//        {
//            return _segmentFactory.GetAddressName("Austin Foam Plastics", "VN", Qualifier.AssignedByBuyer.Value, BusinessPartner.Initech.VendorIdOfAfp);
//        }

        private EDIXmlSegment get_begin_segment(InvoicedOrderMessage message)
        {
            var begin = new EDIXmlSegment("BIG", _buildValues);
            begin.Add(new EDIXmlElement("BIG01", message.InvoiceDate.ToString("yyyyMMdd"), _buildValues));
            begin.Add(new EDIXmlElement("BIG02", message.InvoiceNumber, _buildValues));
            begin.Add(new EDIXmlElement("BIG03", message.PODate.ToString("yyyyMMdd"), _buildValues));
            begin.Add(new EDIXmlElement("BIG04", message.CustomerPO, _buildValues));
            begin.Add(new EDIXmlElement("BIG05", message.TransactionType, _buildValues));
            begin.Add(new EDIXmlElement("BIG06","",_buildValues));
            begin.Add(new EDIXmlElement("BIG07", "OR", _buildValues));
            return begin;
        }

        private void add_address(EDIXmlMixedContainer container, Address address, string qualifier,string code)
        {
            if (address == null) return;

            container.AddSegment(_segmentFactory.GetAddressName(address.AddressName, address.AddressType, qualifier, code));
            container.AddSegment(_segmentFactory.GetAddressLine(address.Address1, address.Address2));
            container.AddSegment(_segmentFactory.GetGeographicInfo(address.City, address.State, address.Zip, "US"));
        }
    }
}
