using System;
using AFPST.Common.Structures;
using EDIDocsIn.config;
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
    using EDIDocsProcessing.Common.Enumerations;
    using EDIDocsProcessing.Common.Extensions;

    [TestFixture]
    public class Initech855Tester
    {
        private ICreateEdiContentFrom<OrderRequestReceivedMessage> _sut;
        private IBusinessPartnerResolver<OrderRequestReceivedMessage> _resolver;

        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();

            ObjectFactory.Configure(x =>
            {
                x.For<IMessagePublisher>().Use<FakeMessagePublisher>();
            });

            _resolver = ServiceLocator.Current.GetInstance<IBusinessPartnerResolver<OrderRequestReceivedMessage>>();
            var docRepo = ServiceLocator.Current.GetInstance<IIncomingDocumentsRepository>();
            docRepo.Save(get_save_info());
        }

        private static DocumentInDTO get_save_info()
        {
            var doc = new DocumentInDTO
                          {
                              ISAControlNumber = 234,
                              ControlNumber = 23443,
                              DateSent = DateTime.Today,
                              PartnerNumber = BusinessPartner.Initech.Number
                          };
            doc.AddResponseElement("19", "23432", "19");
            return doc;
        }

        private static OrderRequestReceivedMessage GetOrder()
        {
            var order = new OrderRequestReceivedMessage
                            {
                                CustomerPO = "SRC009680",
                                BusinessPartnerCode = BusinessPartner.Initech.Code,
                                BusinessPartnerNumber = BusinessPartner.Initech.Number,
                                ControlNumber = "110001",
                                DocumentId = EdiDocumentTypes.PurchaseOrder.DocumentNumber,
                                GeographicLocation = "Tennessee",
                                SpecificLocationNumber = "4",
                                PhoneNumber = "901-263-6584",
                                RequestDate = "7/14/2009 12:00:00 AM",
                                CustomerBankDescription = "Initech",
                                Customer = new Customer
                                               {
                                                   CustomerIDs = new CustomerAliases() { LegacyCustomerID = "FEDE021" },
                                                   CustomerName = "Initech",
                                                   Terms = new TermsOfSale
                                                               {
                                                                   DiscountDays = 0,
                                                                   DiscountPercent = 0,
                                                                   NetDays = 30
                                                               }
                                               }

                            };
            order.AddAddress(new Address
                                          {
                                              AddressType = AddressTypeConstants.ShipTo,
                                              AddressName = "Initech SUSIE CARNEY",
                                              Address1 = "90 Initech PARKWAY",
                                              City = "Collierville",
                                              State = "TN",
                                              Zip = "38017",
                                              Country = "US"
                                          });
            order.Add(new CustomerOrderLine
                          {
                              CustomerPartNumber = "900104",
                              ItemDescription = "That thing",
                              RequestedPrice = 1.55m,
                              RequestedQuantity = 260,
                              LineNumber = 1
                          });
            order.Add(new CustomerOrderLine
                          {
                              CustomerPartNumber = "AAGES7",
                              ItemDescription = "BOX, MONITOR FLAT PANEL 17\"",
                              OrderMultiple = 0,
                              RequestedPrice = 1,
                              RequestedQuantity = 1,
                              LineNumber = 1,
                              TestMode = false, 
                          });
            return order;
        }

        [Test]
        public void can_create_855()
        {
            OrderRequestReceivedMessage orderRequestReceived = GetOrder();
            _sut = _resolver.ResolveFrom(orderRequestReceived);
            EDIXmlTransactionSet ediStr = _sut.BuildFromMessage(orderRequestReceived);
            Console.WriteLine(ediStr);
        }

        [Test]
        public void can_create_interchange_control_with_855()
        {
            OrderRequestReceivedMessage orderRequestReceived = GetOrder();
            _sut = _resolver.ResolveFrom(orderRequestReceived);
            var subscr = ServiceLocator.Current.GetInstance<Subscriber<OrderRequestReceivedMessage>>();
            subscr.Consume(orderRequestReceived);
        }

        [Test, ExpectedException(ExpectedMessage = "PO contains no line items!")]
        public void will_throw_exception_if_no_lines()
        {
            var order = new OrderRequestReceivedMessage(){BusinessPartnerCode = BusinessPartner.Initech.Code,BusinessPartnerNumber = BusinessPartner .Initech .Number};
            _sut = _resolver.ResolveFrom(order);
            _sut.BuildFromMessage(order);
        }
    }
}