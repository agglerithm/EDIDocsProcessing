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
using EdiMessages;
using MassTransit;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using StructureMap;

namespace EDIDocsProcessing.Tests.IntegrationTests.Flextronics
{
    using EDIDocsProcessing.Common.EDIStructures;

    [TestFixture]
    public class Flextronics810Tester
    {
        private ICreateEdiContentFrom<InvoicedOrderMessage> _sut;
        private IBusinessPartnerResolver<InvoicedOrderMessage> _resolver;

        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();

            ObjectFactory.Configure(x =>
            {
                x.For<IServiceBus>().Use<FakeServiceBus>();
            });


            _resolver = ServiceLocator.Current.GetInstance<IBusinessPartnerResolver<InvoicedOrderMessage>>();
            var docRepo = ServiceLocator.Current.GetInstance<IIncomingDocumentsRepository>();
            docRepo.Save(get_save_info());
        }

        private static DocumentInDTO get_save_info()
        {
            var doc = new DocumentInDTO
                          {
                              ControlNumber = 23443,
                              ISAControlNumber = 234,
                              DateSent = DateTime.Today,
                              PartnerNumber = 1004
                          };
            doc.AddResponseElement("REF02", "23432", "ZZ");
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
                                  {ElementName = "REF02", Qualifier = "BF", Value = "444444444"}
                          };
            return lst;
        }

        private static InvoicedOrderMessage GetOrder()
        {
            var order = new InvoicedOrderMessage
                            {
                                Customer =
                                    new Customer { CustomerIDs = new CustomerAliases() { LegacyCustomerID = BusinessPartner.Flextronics.Code }, CustomerName = "Flextronix" },
                                CustomerPO = "4110213145",
                                Location = "Austin",
                                OrderType = "O",
                                BusinessPartnerCode = BusinessPartner.Flextronics.Code,
                                BusinessPartnerNumber = BusinessPartner.Flextronics.Number,
                                ControlNumber = "23444",
                                InvoiceNumber = "461191",
                                InvoiceDate = "6/10/2011".CastToDateTime(),
                                PODate = "6/1/2011".CastToDateTime(),
                                Notes = "101435"
                            };
            order.Add(new InvoicedOrderLine
                          {
                              ItemID = "FIN23430I0",
                              LineNumber = 10,
                              CustomerPartNumber = "14000181",
                              ItemDescription = "Box, 10X6X6",
                              Price = (decimal) 419,
                              Quantity = 250,
                              CustomerPO = "4110213145"
                          });
            order.AddAddress(new Address
                                 {
                                     Address1 = "2343 Mockingbird Ln.",
                                     AddressCode =  new AddressCode { CustomerCode = "0000001"},
                                     AddressName = "Flex",
                                     AddressType = AddressTypeConstants.ShipTo,
                                     City = "Boise",
                                     State = "ID"
                                 });
            return order;
        }

        [Test]
        public void can_create_810()
        {
            InvoicedOrderMessage order = GetOrder();
            _sut = _resolver.ResolveFrom(order);
            EDIXmlTransactionSet ediStr = _sut.BuildFromMessage(order);
            Console.WriteLine(ediStr.Value);
        }

        [Test]
        public void can_create_interchange_control_with_810()
        {
            InvoicedOrderMessage order = GetOrder();
            _sut = _resolver.ResolveFrom(order);
            var subscr = ServiceLocator.Current.GetInstance<Subscriber<InvoicedOrderMessage>>();
            subscr.Consume(order);
        }

        [Test, ExpectedException(ExpectedMessage = "Invoiced order contains no line items!")]
        public void will_throw_exception_if_no_lines()
        {
            var order = new InvoicedOrderMessage { Customer = new Customer(), ControlNumber = "23443", BusinessPartnerCode = BusinessPartner .Flextronics .Code, BusinessPartnerNumber = BusinessPartner .Flextronics .Number };
            _sut = _resolver.ResolveFrom(order);
            _sut.BuildFromMessage(order);
        }
    }
}