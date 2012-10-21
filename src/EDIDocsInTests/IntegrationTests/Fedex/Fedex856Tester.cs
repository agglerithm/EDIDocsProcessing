using System;
using System.Collections.Generic;
using System.Linq;
using AFPST.Common;
using AFPST.Common.Structures;
using EDIDocsOut.config;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;
using EdiMessages;
using MassTransitContrib;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using StructureMap;

namespace EDIDocsProcessing.Tests.IntegrationTests.Initech
{
    using EDIDocsProcessing.Common.EDIStructures;
    using EDIDocsProcessing.Common.Extensions;

    [TestFixture]
    public class Initech856Tester
    {
        private ICreateEdiContentFrom<OrderHasBeenShippedMessage> _sut;

        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();

            ObjectFactory.Configure(x =>
            {
                x.For<IMessagePublisher>().Use<FakeMessagePublisher>();
            });

            var lst = ServiceLocator.Current.GetAllInstances<ICreateEdiContentFrom<OrderHasBeenShippedMessage>>();
            _sut = lst.Find(c => c.CanProcess(GetOrder())); 
            var docRepo = ServiceLocator.Current.GetInstance<IIncomingDocumentsRepository>();
            docRepo.Save(get_save_info());
        }

        private static DocumentInDTO get_save_info()
        {
            var doc = new DocumentInDTO
                          {
                              ISAControlNumber = 234,
                              ControlNumber = 60001,
                              DateSent = DateTime.Today,
                              PartnerNumber = BusinessPartner.Initech.Number
                          };
            doc.AddResponseElement("19", "23432", "19");
            doc.AddResponseElement("REF02", "CAD36", "ZZ");
            doc.AddResponseElement("REF02", "001", "19");
            doc.AddResponseElement("REF02", "3", "XE");
            doc.AddLineItem("1", get_element_list());
            return doc;
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

        private OrderHasBeenShippedMessage GetOrder()
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
            order.Lines.First().TrackingNumbers.Add("33577776");
            return order;
        }

        [Test]
        public void can_create_856()
        {
            OrderHasBeenShippedMessage order = GetOrder();

            EDIXmlTransactionSet ediStr = _sut.BuildFromMessage(order);

            Assert.That(ediStr.Value, Text.Contains("~"));
            Assert.That(ediStr.Value, Text.Contains("\n"));

            Console.WriteLine(ediStr.Value);
        }

        [Test]
        public void     can_create_interchange_control_with_856()
        {
            OrderHasBeenShippedMessage order = GetOrder();
            var subscr = ServiceLocator.Current.GetInstance<Subscriber<OrderHasBeenShippedMessage>>();
            subscr.Consume(order);
        }

        [Test, ExpectedException(ExpectedMessage = "Shipped order contains no line items!")]
        public void will_throw_exception_if_no_lines()
        {
            var order = new OrderHasBeenShippedMessage();
            _sut.BuildFromMessage(order);
        }
    }
}