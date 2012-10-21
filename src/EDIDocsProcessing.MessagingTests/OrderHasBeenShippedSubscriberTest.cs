using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AFPST.Common;
using AFPST.Common.Structures;
using EDIDocsOut.config;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core.DocsIn;
using EdiMessages;
using MassTransit;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework; 

namespace EDIDocsProcessing.MessagingTests
{
    [TestFixture]
    public class OrderHasBeenShippedSubscriberTest
    { 

        [SetUp]
        public void SetUp()
        {

            MassTransitEdiDocsOutBootstrapper.Execute();
            StructureMapBootstrapper.Execute();



            var docRepo = ServiceLocator.Current.GetInstance<IIncomingDocumentsRepository>();
            // docRepo.Delete(get_save_info());
            
            get_save_info().ForEach(docRepo.Save);

        }


        private static IEnumerable<DocumentInDTO> get_save_info()
        {
            var doc1 = new DocumentInDTO
            {
                ISAControlNumber = 234,
                ControlNumber = 60001,
                DateSent = DateTime.Today,
                PartnerNumber = BusinessPartner.Initech.Number
            };
            doc1.AddResponseElement("19", "23432", "19");
            doc1.AddResponseElement("REF02", "CAD36", "ZZ");
            doc1.AddResponseElement("REF02", "001", "19");
            doc1.AddResponseElement("REF02", "3", "XE");
            doc1.AddLineItem("1", get_element_list());


            var doc2 = new DocumentInDTO
            {
                ControlNumber = 23443,
                ISAControlNumber = 234,
                DateSent = DateTime.Today,
                PartnerNumber = 1004
            };
            doc2.AddResponseElement("REF02", "23432", "ZZ");
            doc2.AddLineItem("1", get_element_list());


            return new List<DocumentInDTO> {doc1, doc2};

        }

        private static IList<LineResponseElementEntity> get_element_list()
        {
            var lst = new List<LineResponseElementEntity>
                          {
                              new LineResponseElementEntity
                                  {ElementName = "REF02", Qualifier = "CO", Value = "33333333"},
                              new LineResponseElementEntity
                                  {ElementName = "REF02", Qualifier = "BF", Value = "444444444"},
                                  new LineResponseElementEntity()
                                  {ElementName = "PO1", Qualifier = "IN", Value = "900104"}
                          };
            return lst;
        }


        private InvoicedOrderMessage CreateFlex_InvoicedOrderMessage()
        {
            var order = new InvoicedOrderMessage
                            {
                                Customer =
                                    new Customer
                                    {
                                        CustomerName = "Initech",
                                        CustomerIDs = new CustomerAliases()
                                                                                                    {
                                                                                                        LegacyCustomerID = BusinessPartner.Initech.Code 
                                                                                                    } },

                                CustomerPO = "997439",
                                Location = "Austin",
                                OrderType = "O",
                                BusinessPartnerCode = BusinessPartner.Initech.Code,
                                BusinessPartnerNumber = BusinessPartner.Initech.Number,
                                ControlNumber = "70001",
                                InvoiceNumber = "2345",
                                InvoiceDate = "6/10/2009".CastToDateTime(),
                                PODate = "6/1/2009".CastToDateTime()
                            };
            order.Add(new InvoicedOrderLine
                          {
                              ItemID = "FIN23430I0",
                              LineNumber = 1,
                              CustomerPartNumber = "WR545",
                              ItemDescription = "Box, 10X6X6",
                              Price = (decimal) 1.1,
                              Quantity = 120,
                              CustomerPO = "997439"
                          });
            order.Add(new InvoicedOrderLine
                          {
                              ItemID = "FIN66430I0",
                              LineNumber = 2,
                              CustomerPartNumber = "XR545",
                              ItemDescription = "Box 12X6X6",
                              Price = (decimal) 1.78,
                              Quantity = 190,
                              CustomerPO = "997439"
                          });
            order.AddAddress(new Address
                                 {
                                     Address1 = "2343 Mockingbird Ln.",
                                     AddressCode = new AddressCode {CustomerCode = "0000001"} ,
                                     AddressName = "Flex",
                                     AddressType = AddressTypeConstants.ShipTo,
                                     City = "Boise",
                                     State = "ID"
                                 });
            return order;
        }

        private static OrderHasBeenShippedMessage CreateInitechOrderHasBeenShippedMessageWithTwoLines()
        {
            var order = new OrderHasBeenShippedMessage
            {
                BOL = "0005456",
                CustomerPO = "SRC009670",
                BusinessPartnerCode = BusinessPartner.Initech.Code,
                BusinessPartnerNumber = BusinessPartner.Initech.Number,
                ControlNumber = "60001"
            };
            order.Add(new ShippedLine
            {
                CustomerPartNo = "900104",
                DateShipped = SystemTime.Now(),
                ItemID = "FIN23430I044",
                LineNumber = "1",
                QtyOrdered = 3,
                QtyShipped = 3,
                UOM = "EA"
            });
            order.AddAddress(new Address
            {
                AddressType = AddressTypeConstants.ShipFrom,
                Address1 = "2343 Grimes",
                City = "Austin",
                State = "TX",
                Zip = "78987",
                AddressName = "Austin Foam Plastics"
            });
            order.AddAddress(new Address
            {
                AddressType = AddressTypeConstants.ShipTo,
                Address1 = "90 Initech PARKWAY",
                City = "COLLIERVILLE",
                State = "TN",
                Zip = "38017",
                AddressName = "Initech SUSIE CARNEY"
            });
            order.Lines.First().TrackingNumbers.Add("33532343");
            order.Lines.First().TrackingNumbers.Add("33577773");
            return order;
        }


        private static OrderHasBeenShippedMessage CreateInitechOrderHasBeenShippedMessageWithThreeLines()
        {
            var order = new OrderHasBeenShippedMessage
            {
                BOL = "00999998",
                CustomerPO = "AFC000001",
                BusinessPartnerCode = BusinessPartner.Initech.Code,
                BusinessPartnerNumber = BusinessPartner.Initech.Number,
                ControlNumber = "50001"
            };
            order.Add(new ShippedLine
            {
                CustomerPartNo = "910021",
                DateShipped = SystemTime.Now(),
                ItemID = "910021",
                LineNumber = "1",
                QtyOrdered = 1,
                QtyShipped = 1,
                UOM = "EA"
            });
            order.Add(new ShippedLine
            {
                CustomerPartNo = "910022",
                DateShipped = SystemTime.Now(),
                ItemID = "910022",
                LineNumber = "2",
                QtyOrdered = 2,
                QtyShipped = 2,
                UOM = "EA"
            });
            order.AddAddress(new Address
            {
                AddressType = AddressTypeConstants.ShipFrom,
                Address1 = "2343 Grimes",
                City = "Austin",
                State = "TX",
                Zip = "78987",
                AddressName = "Austin Foam Plastics"
            });
            order.AddAddress(new Address
            {
                AddressType = AddressTypeConstants.ShipTo,
                Address1 = " 590 BROADWAY",
                City = "MENANDS",
                State = "NY",
                Zip = "12204",
                AddressName = "FEDERAL EXPRESS"
            });
            order.Lines.Where(l => l.LineNumber == "1").First().TrackingNumbers.Add("99999994");
            order.Lines.Where(l => l.LineNumber == "2").First().TrackingNumbers.Add("99999995");
            order.Lines.Where(l => l.LineNumber == "2").First().TrackingNumbers.Add("99999996");
            return order;
        }

        [Test]
        public void publish_ShippedOrderMessage()
        {

            var messaage = CreateInitechOrderHasBeenShippedMessageWithThreeLines();
            
            Thread.Sleep(2000);

            Bus.Instance().Publish(messaage);
        }
        

        [Test]
        public void publish_two_Messages()
        {
            var messaage1 = CreateFlex_InvoicedOrderMessage();
            var messaage2 = CreateInitechOrderHasBeenShippedMessageWithTwoLines();

            Thread.Sleep(2000);

            Bus.Instance().Publish(messaage1);

            Bus.Instance().Publish(messaage2);

        }
    }
}