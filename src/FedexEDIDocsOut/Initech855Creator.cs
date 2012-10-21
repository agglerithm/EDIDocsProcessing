using System; 
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core.DocsOut;
using EdiMessages;

namespace InitechEDIDocsOut
{
    public class Initech855Creator : ICreateEdiContentFrom<OrderRequestReceivedMessage>
    {
        private readonly IControlNumberRepository _repo; 
        private readonly EdiXmlBuildValues _ediXmlBuildValues;
        private readonly ISegmentFactory _segmentFactory;

        public Initech855Creator(IControlNumberRepository repo, IBusinessPartnerSpecificServiceResolver serviceResolver)
        {
            _repo = repo;
            _segmentFactory = serviceResolver.GetSegmentFactoryFor(BusinessPartner.Initech);
            _ediXmlBuildValues = _segmentFactory.BuildValues;
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
            return msg.DocumentId == EdiDocumentTypes.PurchaseOrder.DocumentNumber && msg.BusinessPartnerCode == BusinessPartner.Initech.Code;
        }


//        public string ReceiverId
//        {
//            get { return "055001924VA    "; }
//        }
//
//        public string SenderId
//        {
//            get { return "EEC5122516063  ";  }
//        }

        public EDIXmlTransactionSet BuildFromMessage(OrderRequestReceivedMessage orderRequestReceived)
        {
            return create_transaction_set(orderRequestReceived);
        }

        private EDIXmlTransactionSet create_transaction_set(OrderRequestReceivedMessage ackMessage)
        { 
            if (ackMessage.LineCount == 0)
                    throw new Exception("PO contains no line items!");
            var doc = new EDIXmlTransactionSet(_segmentFactory) {ISA = _repo.GetNextISA(GroupTypeConstants.POAcknowledgement, BusinessPartner.Initech.Number)};

            var docDef = _repo.GetNextDocument(doc.ISA, 855);
            doc.SetHeader("855", docDef.ControlNumber);

            doc.AddSegment(get_begin_segment(ackMessage));

            doc.AddSegment(_segmentFactory.GetAddressName("Austin Foam Plastics",
                NameCodeConstants.Vendor,"",""));

            var lines = new EDIXmlMixedContainer("PO1");  
            ackMessage.LineItems.ForEach(l => add_line(lines, l));

            doc.AddLoop(lines);

            doc.AddSegment(_segmentFactory.GetTransactionTotal(ackMessage.LineCount));

            doc.SetFooter();

            return doc;

        }

        private EDIXmlSegment get_begin_segment(OrderRequestReceivedMessage ackMessage)
        {
            var begin = new EDIXmlSegment("BAK", _ediXmlBuildValues);
            begin.Add(new EDIXmlElement("BAK01", "00", _ediXmlBuildValues));
            begin.Add(new EDIXmlElement("BAK02", "AK", _ediXmlBuildValues));
            begin.Add(new EDIXmlElement("BAK03", ackMessage.CustomerPO, _ediXmlBuildValues));
            begin.Add(new EDIXmlElement("BAK04", DateTime.Now.ToString("yyyyMMdd"), _ediXmlBuildValues));
            return begin;
        }

        private  void add_line(EDIXmlMixedContainer lines, CustomerOrderLine l)
        {
           lines.AddSegment(_segmentFactory.GetPOLine(l.LineNumber.ToString(),  l.RequestedQuantity,
               l.RequestedPrice, l.CustomerPartNumber, "", l.ItemDescription));
           lines.AddSegment(_segmentFactory.GetAckLine("IA", l.RequestedQuantity,
                get_delivery_date().ToString("yyyyMMdd"),
                l.CustomerPartNumber, "", l.ItemDescription));
        }

        private static DateTime get_delivery_date()
        {
            if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
                return DateTime.Today.AddDays(3);
            if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday)
                return DateTime.Today.AddDays(2);
            return DateTime.Today.AddDays(1);
        }
    }

 
}
