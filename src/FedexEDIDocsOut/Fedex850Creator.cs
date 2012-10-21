using System;
using AFPST.Common.Extensions;
using AFPST.Common.Structures;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;
using EdiMessages;

namespace FedexEDIDocsOut
{
    public class Fedex850Creator : ICreateEdiContentFrom<OrderRequestReceivedMessage>
    {
        private readonly IControlNumberRepository _repo; 
        private readonly ISegmentFactory _segmentFactory;
        private readonly IBuildValueFactory _buildValueFactory;

        public Fedex850Creator(IControlNumberRepository repo, IBusinessPartnerSpecificServiceResolver serviceResolver)
        {
            _repo = repo; 
            _segmentFactory = serviceResolver.GetSegmentFactoryFor(BusinessPartner.FedEx);
            _buildValueFactory = serviceResolver.GetBuildValueFactoryFor(BusinessPartner.FedEx);
        }
    
        public ISegmentFactory SegmentFactory
        {
            get { return _segmentFactory; }
        }

        public BusinessPartner GetBusinessPartner()
        {
            return BusinessPartner.FedEx;
        }

        public bool CanProcess(IEdiMessage msg)
        {
            return msg.GetType() == typeof(OrderRequestReceivedMessage) && msg.BusinessPartnerNumber == BusinessPartner.FedEx.Number;
        }

//
//        public string ReceiverId
//        {
//            get { return "055001924VA    "; }
//        }
//
//        public string SenderId
//        {
//            get { return "EEC5122516063  "; }
//        }


        public IEDIXmlTransactionSet BuildFromMessage(OrderRequestReceivedMessage orderRequestReceivedMessage)
        {
            return create_transaction_set(orderRequestReceivedMessage);
        }



        private EDIXmlTransactionSet create_transaction_set(OrderRequestReceivedMessage orderRequestReceivedMessage)
        {
            if (orderRequestReceivedMessage.LineCount == 0)
                throw new Exception("PO contains no line items!");
            var doc = new EDIXmlTransactionSet(_segmentFactory)
                          {
                              ISA = _repo.GetNextISA(GroupTypeConstants.PurchaseOrder, BusinessPartner.FedEx.Number)
                          };

            var docDef = _repo.GetNextDocument(doc.ISA, EdiDocumentTypes.PurchaseOrder.DocumentNumber);
            doc.SetHeader(EdiDocumentTypes.PurchaseOrder.DocumentNumber.ToString(), docDef.ControlNumber);

            doc.AddSegment(get_begin_segment(orderRequestReceivedMessage));

//            doc.AddSegment(_segmentFactory.GetCurrencySegment("II", orderMessage.CurrencyCode));
//
//            doc.AddSegment(_segmentFactory.GetReferenceIDSegment(BOLQualifiers.VendorOrderNumber,
//                orderMessage.BOL));

            doc.AddLoop(get_address_loop(orderRequestReceivedMessage));

            doc.AddLoop(get_detail_loop(orderRequestReceivedMessage));

//            doc.AddSegment(_segmentFactory.GetTotalMonetaryValue(orderMessage.GetTotal()));

            doc.AddSegment(_segmentFactory.GetTransactionTotal(orderRequestReceivedMessage.LineCount));
            return doc;
        }

        private EDIXmlMixedContainer get_detail_loop(OrderRequestReceivedMessage orderRequestReceivedMessage)
        {
            var loop = new EDIXmlMixedContainer("PO1");
            orderRequestReceivedMessage.LineItems.ForEach(l => add_line(loop, l));
            return loop;
        }

        private void add_line(EDIXmlMixedContainer container, CustomerOrderLine line)
        {
            container.AddSegment(_segmentFactory.GetPOLine(line.LineNumber.ToString(), line.RequestedQuantity,
                line.RequestedPrice, line.CustomerPartNumber, line.ItemId, line.ItemDescription));
        }

        private EDIXmlMixedContainer get_address_loop(OrderRequestReceivedMessage orderRequestReceivedMessage)
        {
            var loop = new EDIXmlMixedContainer("N1");
            if (orderRequestReceivedMessage.ShipToAddress == null)
                throw new ApplicationException("There are no ship to address associated with order");

            add_address(loop, orderRequestReceivedMessage.ShipToAddress);
            add_address(loop, orderRequestReceivedMessage.GetBillToAddress());

            return loop;
        }

        private void add_address(EDIXmlMixedContainer container, Address address)
        {
            container.AddSegment(_segmentFactory.GetAddressName(address.AddressName, address.AddressType, "",""));
            container.AddSegment(_segmentFactory.GetAddressLine(address.Address1, address.Address2));
            container.AddSegment(_segmentFactory.GetGeographicInfo(address.City, address.State, address.Zip, "US"));
        }

        private EDIXmlSegment get_begin_segment(OrderRequestReceivedMessage message)
        {
            var begin = new EDIXmlSegment("BEG", _buildValueFactory.GetValues());
            begin.Add(new EDIXmlElement("BEG01", "00", _buildValueFactory.GetValues()));
            begin.Add(new EDIXmlElement("BEG02", "NE", _buildValueFactory.GetValues()));
            begin.Add(new EDIXmlElement("BEG03", message.CustomerPO, _buildValueFactory.GetValues()));
            begin.Add(new EDIXmlElement("BEG04", "0", _buildValueFactory.GetValues()));
            begin.Add(new EDIXmlElement("BEG05", message.RequestDate, _buildValueFactory.GetValues()));
            return begin;
        }
    }
}

