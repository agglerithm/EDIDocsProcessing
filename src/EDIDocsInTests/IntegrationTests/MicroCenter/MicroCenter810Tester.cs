namespace EDIDocsProcessing.Tests.IntegrationTests.MicroCenter
{
    using System;
    using System.Collections.Generic;
    using AFPST.Common.Structures;
    using EDIDocsOut.config;
    using EDIDocsProcessing.Common;
    using EDIDocsProcessing.Common.DTOs;
    using EDIDocsProcessing.Common.EDIStructures;
    using EDIDocsProcessing.Common.Extensions;
    using Core;
    using Core.DocsIn;
    using Core.DocsOut;
    using Core.DocsOut.EDIStructures;
    using EdiMessages;
    using global::StructureMap;
    using MassTransit;
    using Microsoft.Practices.ServiceLocation;
    using NUnit.Framework;

    [TestFixture]
    public class MicroCenter810Tester
    {
        private IBusinessPartnerResolver<InvoicedOrderMessage> _resolver;
        private ICreateEdiContentFrom<InvoicedOrderMessage> _sut;

        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();

            ObjectFactory.Configure(x =>
            {
                x.For<IServiceBus>().Use<FakeServiceBus>();
            });

             
            var docRepo = ServiceLocator.Current.GetInstance<IIncomingDocumentsRepository>();
            docRepo.Save(get_save_info());
            _resolver = ServiceLocator.Current.GetInstance<IBusinessPartnerResolver<InvoicedOrderMessage>>();
        }

        private static DocumentInDTO get_save_info()
        {
            var doc = new DocumentInDTO
                          {
                              ControlNumber = 23444,
                              ISAControlNumber = 2300,
                              DateSent = DateTime.Today,
                              PartnerNumber = BusinessPartner.MicroCenter.Number
                          };
            doc.AddResponseElement("REF02", "23444", "ZZ");
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
                                    new Customer { CustomerIDs = new CustomerAliases() { LegacyCustomerID = BusinessPartner.MicroCenter.Code }, CustomerName = "MicroCenter" },
                                CustomerPO = "4110213145",
                                Location = "Austin",
                                OrderType = "O",
                                BusinessPartnerCode = BusinessPartner.MicroCenter.Code,
                                BusinessPartnerNumber = BusinessPartner.MicroCenter.Number,
                                ControlNumber = "23444",
                                InvoiceNumber = "461191",
                                InvoiceDate = "6/10/2011".CastToDateTime(),
                                PODate = "6/1/2011".CastToDateTime(),
                                Notes = "101435"
                            };
            order.Add(new InvoicedOrderLine
                          {
                              ItemID = "FIN23430I0",
                              LineNumber = 1,
                              CustomerPartNumber = "14000181",
                              ItemDescription = "Box, 10X6X6",
                              Price = (decimal) 419,
                              Quantity = 250,
                              CustomerPO = "4110213145"
                          });
            order.Add(new InvoicedOrderLine()
            {
                ItemID = "FIN23430SS0",
                LineNumber = 2,
                CustomerPartNumber = "14000182",
                ItemDescription = "Box, 10X8X6",
                Price = (decimal)119,
                Quantity = 50,
                CustomerPO = "4110213145"
                              
                          });
            order.AddAddress(new Address
                                 {
                                     Address1 = "2343 Mockingbird Ln.",
                                     AddressCode =  new AddressCode { CustomerCode = "0000001"},
                                     AddressName = "MicroCenter",
                                     AddressType = AddressTypeConstants.ShipTo,
                                     City = "Boise",
                                     State = "ID"
                                 });
            order.AddAddress(new Address
            {
                Address1 = "1111 1st St.",
                AddressCode = new AddressCode { CustomerCode = "0000001" },
                AddressName = "AFP",
                AddressType = AddressTypeConstants.ShipFrom,
                City = "Columbus",
                State = "OH"
            });
            return order;
        }

        [Test]
        public void can_create_810()
        {
            //Arrange
            InvoicedOrderMessage order = GetOrder();
            //Act
            _sut = _resolver.ResolveFrom(order);
            EDIXmlTransactionSet ediStr = _sut.BuildFromMessage(order);
            var arr = ediStr.Value.Split("\n".ToCharArray());
            //Assert
            arr[1].ShouldEqual("BIG~20110610~461191~20110601~4110213145~~~DI");
            arr[2].ShouldEqual("REF~IA~25097");
            arr[arr.Length - 3].ShouldEqual("CTT~2");
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
            var order = new InvoicedOrderMessage { Customer = new Customer(), ControlNumber = "23443", BusinessPartnerCode = BusinessPartner.MicroCenter.Code, DocumentId = 810 };
            _sut = _resolver.ResolveFrom(order);
            _sut.BuildFromMessage(order);
        }
    }
}