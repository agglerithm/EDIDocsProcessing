using System;
using AFPST.Common.Structures;
using EDIDocsOut.config;
using EDIDocsProcessing.Common;

using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;
using EdiMessages;
using MassTransit;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using StructureMap;

namespace EDIDocsProcessing.Tests.IntegrationTests.Initech
{
    using EDIDocsProcessing.Common.EDIStructures;
    using EDIDocsProcessing.Common.Extensions;

    [TestFixture]
    public class Initech850Tester
    {
        private ICreateEdiContentFrom<OrderRequestReceivedMessage> _sut;

        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();

            ObjectFactory.Configure(x =>
                                        {
                                            x.For<IServiceBus>().Use<FakeServiceBus>();
                                        });


            var lst = ServiceLocator.Current.GetAllInstances<ICreateEdiContentFrom<OrderRequestReceivedMessage>>();
            _sut = lst.Find(c => c.CanProcess(GetOrder()));
        }

        private OrderRequestReceivedMessage GetOrder()
        {
            var order = new OrderRequestReceivedMessage
                            {
                                ControlNumber = "0005456",
                                CustomerPO = "C04134707",
                                RequestDate = "20090318",
                                BusinessPartnerCode = BusinessPartner.Initech.Code,
                                BusinessPartnerNumber = BusinessPartner.Initech.Number,
                                BusinessProcessName = "Initech Purchase Order Processing",
                                Customer =
                                    new Customer { CustomerIDs = new CustomerAliases() { LegacyCustomerID = BusinessPartner.Initech.Code }, CustomerName = "Initech" }
                            };
            order.AddAddress(new Address
                                 {
                                     Address1 = "1900 Mockingbird Ln",
                                     AddressType = AddressTypeConstants.ShipTo,
                                     AddressCode = new AddressCode { CustomerCode = "AEGW" } 
                                     ,
                                     AddressName = "Billy Bob",
                                     City = "Hackensack",
                                     State = "New Jersey",
                                     Zip = "01234"
                                 });
            order.Add(new CustomerOrderLine
                          {
                              CustomerPartNumber = "146525",
                              ItemId = "FIN23430I0",
                              LineNumber = 1,
                              RequestedQuantity = 4,
                              RequestedPrice = 1,
                              ItemDescription = ""
                          });
            order.Add(new CustomerOrderLine
                          {
                              CustomerPartNumber = "CAD33",
                              ItemId = "FIN23430I044",
                              LineNumber = 2,
                              RequestedQuantity = 4,
                              RequestedPrice = 1,
                              ItemDescription = ""
                          });
            return order;
        }

        [Test, Explicit("We are not creating 850s yet.  Once we do this test becomes valid")]
        public void can_create_850()
        {
            OrderRequestReceivedMessage orderRequestReceived = GetOrder();

            EDIXmlTransactionSet ediStr = _sut.BuildFromMessage(orderRequestReceived);

            Assert.That(ediStr.Value, Text.Contains(@"~"));
            Assert.That(ediStr.Value, Text.Contains("\n"));

            Console.WriteLine(ediStr.Value);
        }

        [Test, Explicit("We are not creating 850s yet.  Once we do this test becomes valid")]
        public void can_create_interchange_control_with_850()
        {
            OrderRequestReceivedMessage orderRequestReceived = GetOrder();
            var subscr = ServiceLocator.Current.GetInstance<Subscriber<OrderRequestReceivedMessage>>();
            subscr.Consume(orderRequestReceived);
        }

        [Test, ExpectedException(ExpectedMessage = "PO contains no line items!"), Explicit("We are not creating 850s yet.  Once we do this test becomes valid")]
        public void will_throw_exception_if_no_lines()
        {
            var order = new OrderRequestReceivedMessage();
            _sut.BuildFromMessage(order);
        }
    }
}