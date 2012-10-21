using System;
using System.Collections.Generic;
using AFPST.Common.Structures;
using EDIDocsOut.config;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.Extensions;

using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsIn;
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

    [TestFixture]
    public class Initech810Tester
    {
        private ICreateEdiContentFrom<InvoicedOrderMessage> _sut;

        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();
            ObjectFactory.Configure(x =>
                                        {
                                            x.For<IServiceBus>().Use<FakeServiceBus>();
                                        });


            var lst = ServiceLocator.Current.GetAllInstances<ICreateEdiContentFrom<InvoicedOrderMessage>>();
            _sut = lst.Find(p => p.CanProcess(GetOrder()));
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
                              PartnerNumber = BusinessPartner.Initech.Number,
                              
                          };
            doc.AddResponseElement("REF02", "23432", "ZZ");
            doc.AddLineItem("1", get_element_list());
            doc.AddLineItem("2", get_element_list());
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
                                  new LineResponseElementEntity
                                  {ElementName = "PO1", Qualifier = "IN", Value = "94001"}
                          };
            return lst;
        }

        private static InvoicedOrderMessage GetOrder()
        {
            var order = new InvoicedOrderMessage
                            {
                                Customer =
                                    new Customer { CustomerIDs = new CustomerAliases() { LegacyCustomerID = BusinessPartner.Initech.Code }, CustomerName = "Initech" },
                                CustomerPO = "AFP009680",
                                Location = "Austin",
                                OrderType = "O",
                                BusinessPartnerCode = BusinessPartner.Initech.Code,
                                BusinessPartnerNumber = BusinessPartner.Initech.Number,
                                ControlNumber = "23443",
                                InvoiceNumber = "2345",
                                InvoiceDate = "6/10/2009".CastToDateTime(),
                                SalesTax = 0m
                            };
            order.Add(new InvoicedOrderLine
                          {
                              ItemID = "FIN23430I0",
                              LineNumber = 1,
                              CustomerPartNumber = "94001",
                              ItemDescription = "Box, 10X6X6",
                              Price = (decimal) 1.1,
                              Quantity = 120,
                              CustomerPO = "997439"
                          });
            order.Add(new InvoicedOrderLine
                          {
                              ItemID = "FIN66430I0",
                              LineNumber = 2,
                              CustomerPartNumber = "94001",
                              ItemDescription = "Box 12X6X6",
                              Price = (decimal) 1.78,
                              Quantity = 190,
                              CustomerPO = "997439"
                          });
            order.AddAddress(new Address
                                 {
                                     Address1 = "2343 Mockingbird Ln.",
                                     AddressCode = new AddressCode { CustomerCode = "adfd" },
                                     AddressName = "Initech",
                                     AddressType = AddressTypeConstants.ShipTo,
                                     City = "Boise",
                                     State = "ID"
                                 }); 
            
            order.AddAddress(new Address
                                 {
                                     Address1 = "1253 Heil Quaker Blvd.",
                                     AddressCode = new AddressCode { CustomerCode = "adfd" },
                                     AddressName = "Austin Foam Plastics",
                                     AddressType = AddressTypeConstants.Vendor,
                                     City = "La Vergne",
                                     State = "TN",
                                     Zip = "39234"
                                 });
            return order;
        }

        [Test]
        public void can_create_810()
        {
            InvoicedOrderMessage order = GetOrder();

            EDIXmlTransactionSet ediStr = _sut.BuildFromMessage(order);
            Console.WriteLine(ediStr.Value);
        }

        [Test]
        public void can_create_interchange_control_with_810()
        {
            InvoicedOrderMessage order = GetOrder();
            var subscr = ServiceLocator.Current.GetInstance<Subscriber<InvoicedOrderMessage>>();

            subscr.Consume(order);
        }

        [Test, ExpectedException(ExpectedMessage = "Invoiced order contains no line items!")]
        public void will_throw_exception_if_no_lines()
        {
            var order = new InvoicedOrderMessage {Customer = new Customer(), ControlNumber = "23443"};
            _sut.BuildFromMessage(order);
        }
    }
}