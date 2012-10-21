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
using EDIDocsProcessing.Core.DocsIn.impl;
using EdiMessages;

namespace InitechEDIDocsIn
{
    public class Initech850Parser : IDocumentParser 
    {
        private string _contactName;
        const string LOCATION = "Tennessee";
        private const string CUSTOMER_ID = "FEDE210";
        private const string SYTELINE_CUSTOMER_ID = "FEDE001";
        private const string WAREHOUSE = "4";
        private readonly IAddressParser _addrParser;
        private readonly IInitech850LineParser _lineParser;
        private readonly IEDIResponseReferenceRecorder _recorder;
        private readonly IGeneric850Parser _genericParser;
        public Initech850Parser(IAddressParser addressParser, IEDIResponseReferenceRecorder recorder, IGeneric850Parser genericParser, IInitech850LineParser lineParser)
        {
            _addrParser = addressParser;
            _lineParser = lineParser;
            _recorder = recorder;
            _genericParser = genericParser;
        }


        public DocumentRecordPackage ProcessSegmentList(List<Segment> segList)
        {
            var orderRequestReceivedMessage = _genericParser.ProcessSegmentList(segList);

            orderRequestReceivedMessage.BusinessProcessName = "Initech Purchase Order Processing";

            orderRequestReceivedMessage.BusinessPartnerCode = BusinessPartner.Initech.Code;

            orderRequestReceivedMessage.CustomerBankDescription = BusinessPartner.Initech.Code;

            orderRequestReceivedMessage.BusinessPartnerNumber = BusinessPartner.Initech.Number;

            orderRequestReceivedMessage.GeographicLocation = LOCATION;

            orderRequestReceivedMessage.SpecificLocationNumber = WAREHOUSE;

            orderRequestReceivedMessage.Customer = 
                new Customer
                    {
                        CustomerName = "Initech",
                        CustomerIDs =
                            {
                                LegacyCustomerID = CUSTOMER_ID,
                                SytelineCustomerID = SYTELINE_CUSTOMER_ID
                            }
                    };
             

            load_beginning_segment(segList[1], orderRequestReceivedMessage);

            //remove Header And Beginning Segments

            segList.RemoveRange(0, 2);

            int segmentsProcessed = 2;

            var removeList = _recorder.MemorizeOuterReferences(segList, orderRequestReceivedMessage.ControlNumber, _genericParser.ElementDelimiter, BusinessPartner.Initech);

            segmentsProcessed += removeList.Count();    

            removeList.ForEach(s => segList.Remove(s)); 

 
            orderRequestReceivedMessage.LevelOfService = ServiceLevel.TwoDay; //Hard coded at request of Initech
 

            segmentsProcessed += process_admin_contact(segList, orderRequestReceivedMessage);

            segmentsProcessed += process_addresses(segList, orderRequestReceivedMessage);

            if (orderRequestReceivedMessage.ShipToAddress != null)
                orderRequestReceivedMessage.ShipToAddress.ContactName = _contactName;

            segmentsProcessed += process_line_items(segList, orderRequestReceivedMessage);

            segmentsProcessed += process_summary(segList, orderRequestReceivedMessage);

            process_footer(segList, orderRequestReceivedMessage, segmentsProcessed);

            return new DocumentRecordPackage(orderRequestReceivedMessage, _genericParser.ResponseValues, _genericParser.Lines);
        }

        public bool CanProcess(BusinessPartner partner, string docType)
        {
            return partner.Value == BusinessPartner.Initech.Value && docType == "850";
        }

 
        public IEnumerable<DocumentLineItemEntity> GetLines()
        {
            return _genericParser.Lines;
        }

 

        private void process_footer(List<Segment> segList, IEdiMessage order, int segmentsProcessed)
        {
            EDIUtilities.ProcessFooter(segList, order, _genericParser.ElementDelimiter, segmentsProcessed + 1);
        }


        private int process_summary(IEnumerable<Segment> segList, OrderRequestReceivedMessage orderRequestReceived)
        {
            Segment tempSeg = segList.FindSegmentByLabel(SegmentLabel.SummaryLabel);
            Console.WriteLine("Number of lines: " + orderRequestReceived.LineCount);
            if (tempSeg == null) return 0;
            if (orderRequestReceived.LineCount != tempSeg.GetElements(_genericParser.ElementDelimiter)[1].CastToInt())
                throw new Invalid850Exception("Line item count does not match number of lines");
            return 1;
        }

        private int process_line_items(List<Segment> segList, IEdiMessage order)
        {
            var refs = _lineParser.ProcessLines(segList, order);
            _recorder.MemorizeInnerReferences(refs, order.ControlNumber, BusinessPartner.Initech);
            return _lineParser.SegmentCount;
        }

        private int process_addresses(List<Segment> segList, IEdiMessageWithAddress order)
        {
            return _addrParser.ProcessAddresses(segList, order); 
        }

        private  int process_admin_contact(ICollection<Segment> segList, OrderRequestReceivedMessage msg)
        {
            Segment tempSeg = segList.FindSegmentByLabel(SegmentLabel.ContactLabel);
            if (tempSeg == null) return 0;
            var els = tempSeg.GetElements(ElementDelimiter);
            if(els.Count() > 4)
                msg.PhoneNumber = els[4];
            _contactName = els[2];
            segList.Remove(tempSeg);
            return 1;
        }

        public string ElementDelimiter { get { return _genericParser.ElementDelimiter; } }

//        private void process_inner_ref(Segment rf, CustomerOrderLine line)
//        { 
//            var arr = rf.GetElements(ElementDelimiter); 
//        }

        private void load_beginning_segment(Segment segment, OrderRequestReceivedMessage orderRequestReceived)
        {
           var arr = segment.GetElements(ElementDelimiter);
           if (arr[0] != SegmentLabel.POBegin.Text)
               throw new Invalid850Exception("BEG Segment is missing!");
            orderRequestReceived.CustomerPO = arr[3];
            orderRequestReceived.RequestDate =  DateTime.Now.ToString();
        }


 
    }

 
}