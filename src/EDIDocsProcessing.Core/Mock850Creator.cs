using System;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core.DataAccess.DTOs;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;
using EdiMessages;

namespace EDIDocsProcessing.Core
{
    public class Mock850Creator : ICreateEdiContentFrom<CreateOrderMessage>
    {
//        private readonly IControlNumberRepository _repo;
//        private readonly ISAEntity _isa;
        private readonly ISegmentFactory _segmentFactory;

        public Mock850Creator( )
        {
            _segmentFactory = new SegmentFactory(new MockBuildValuesFactory());

        }


        public ISegmentFactory SegmentFactory
        {
            get { return _segmentFactory; }
        }

 

        public string ReceiverId
        {
            get { return "055001924VA"; }
        }

 

        public IEDIXmlTransactionSet BuildFromMessage(CreateOrderMessage orderMessage)
        {
            if (orderMessage.LineItems == null) 
                throw new ApplicationException("PO contains no line items!");
            Console.WriteLine("Order control number: " + orderMessage.ControlNumber);
            orderMessage.LineItems.ForEach(display_line);
            Console.WriteLine("Customer ID: " + orderMessage.CustomerID);
            Console.WriteLine("Customer PO: " + orderMessage.CustomerPO);
            Console.WriteLine("Ship-to-address: " + orderMessage.GetShipToAddress().ToString());
            Console.WriteLine("Request date: " + orderMessage.RequestDate);
            var ts = new EDIXmlTransactionSet(_segmentFactory).SetHeader("850",23430) ;
            ts.ISA = new ISAEntity() {ControlNumber = 1, GroupID = "PO"};
            return ts;
        }

        private static void display_line(CustomerOrderLine line)
        {
            Console.WriteLine("\tLine item #" + line.LineNumber);
            Console.WriteLine("\tCustomer part number: " + line.CustomerPartNumber);
            Console.WriteLine("\tQuantity requested: " + line.RequestedQuantity);
            Console.WriteLine("\tPrice requested: " + line.RequestedPrice);
        }
    }

    public class MockBuildValuesFactory : IBuildValueFactory
    {
        public EdiXmlBuildValues GetValues()
        {
            return new EdiXmlBuildValues()
                       {
                           ElementDelimiter = "~",
                           SegmentDelimiter = @"\",
                           FunctionGroupReceiverID = "234323",
                           InterchangeQualifier = "12",
                           InterchangeReceiverID = "234323",
                           Transport = TransportAgent.Fedex
                       };
        }
    }
}