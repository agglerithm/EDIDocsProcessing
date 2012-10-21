using System;
using System.Collections.Generic;
using AFPST.Common.Infrastructure;
using EDIDocsOut;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Common.Infrastructure;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DataAccess.DTOs;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsOut;
using EdiMessages;
using FlexEDIDocsOut;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests
{
    [TestFixture]
    public class CrossPartnerTests
    {
 
        private Subscriber<InvoicedOrderMessage> _subscr;

        [TestFixtureSetUp]
        public void SetUp()
        {
            EDIContainer.Reset();
            ApplicationEnvironment.Setup(); 
            var docRepo = EDIContainer.Resolve<IIncomingDocumentsRepository>();
            docRepo.Save(get_fedex_save_info());
            docRepo.Save(get_flex_save_info());

            _subscr =
                new Subscriber<InvoicedOrderMessage>(
                    EDIContainer.Resolve<IBusinessPartnerResolver<InvoicedOrderMessage>>(),
                    EDIContainer.Resolve<ICreateEdiDocumentFrom<InvoicedOrderMessage>>(),
                    EDIContainer.Resolve<IEdiDocumentSaver>());
        }

        private DocumentInDTO get_flex_save_info()
        {            var doc = new DocumentInDTO
            {
                ISAControlNumber = 234,
                ControlNumber = 23444,
                DateSent = DateTime.Today,
                PartnerNumber = BusinessPartner.Flextronics.Number,

            };
            doc.AddResponseElement("REF02", "23432", "ZZ");
            doc.AddLineItem("1", get_element_list());
            doc.AddLineItem("2", get_element_list());
            return doc;
        }

        private static DocumentInDTO get_fedex_save_info()
        {
            var doc = new DocumentInDTO
            {
                ISAControlNumber = 234,
                ControlNumber = 23443,
                DateSent = DateTime.Today,
                PartnerNumber = BusinessPartner.FedEx.Number,

            };
            doc.AddResponseElement("REF02", "23432", "ZZ");
            doc.AddLineItem("1", get_element_list());
            doc.AddLineItem("2", get_element_list());
            return doc;
        }
        [Test]
        public void can_process_two_partners()
        {
            InvoicedOrderMessage order = GetFedexOrder();
            _subscr.Consume(order);
            var flexOrder = GetFlexOrder();
            _subscr.Consume(flexOrder);
            _subscr.Consume(order);
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
        [TestFixtureTearDown]
        public void TearDown()
        {
            Container.Reset();
        }

        private static InvoicedOrderMessage GetFedexOrder()
        {
            var order = new InvoicedOrderMessage
            {
                Customer =
                    new Customer { CustomerIDs = new CustomerAliases() { LegacyCustomerID = BusinessPartner.FedEx.Code }, CustomerName = "Fedex" },
                CustomerPO = "SRC009680",
                Location = "Austin",
                OrderType = "O",
                BusinessPartnerCode = BusinessPartner.FedEx.Code,
                BusinessPartnerNumber = BusinessPartner.FedEx.Number,
                ControlNumber = "23443",
                InvoiceNumber = "2345",
                InvoiceDate = "6/10/2009".CastToDateTime(),
                SalesTax = (decimal)2.50
            };
            order.Add(new InvoicedOrderLine
            {
                ItemID = "FIN23430I0",
                LineNumber = 1,
                CustomerPartNumber = "94001",
                ItemDescription = "Box, 10X6X6",
                Price = (decimal)1.1,
                Quantity = 120,
                CustomerPO = "997439"
            });
            order.Add(new InvoicedOrderLine
            {
                ItemID = "FIN66430I0",
                LineNumber = 2,
                CustomerPartNumber = "94001",
                ItemDescription = "Box 12X6X6",
                Price = (decimal)1.78,
                Quantity = 190,
                CustomerPO = "997439"
            });
            order.AddAddress(new Address
            {
                Address1 = "2343 Mockingbird Ln.",
                AddressCode = new AddressCode { CustomerCode = "adfd" },
                AddressName = "Fedex",
                AddressType = AddressTypeConstants.ShipTo,
                City = "Boise",
                State = "ID"
            });
            return order;
        }

        private static InvoicedOrderMessage GetFlexOrder()
        {
            var order = new InvoicedOrderMessage
            {
                Customer =
                    new Customer { CustomerIDs = new CustomerAliases() { LegacyCustomerID = BusinessPartner.Flextronics.Code }, CustomerName = "Flextronix" },
                CustomerPO = "997439",
                Location = "Austin",
                OrderType = "O",
                BusinessPartnerCode = BusinessPartner.Flextronics.Code,
                BusinessPartnerNumber = BusinessPartner.Flextronics.Number,
                ControlNumber = "23443",
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
                Price = (decimal)1.1,
                Quantity = 120,
                CustomerPO = "997439"
            });
            order.Add(new InvoicedOrderLine
            {
                ItemID = "FIN66430I0",
                LineNumber = 2,
                CustomerPartNumber = "XR545",
                ItemDescription = "Box 12X6X6",
                Price = (decimal)1.78,
                Quantity = 190,
                CustomerPO = "997439"
            });
            order.AddAddress(new Address
            {
                Address1 = "2343 Mockingbird Ln.",
                AddressCode = new AddressCode { CustomerCode = "0000001" },
                AddressName = "Flex",
                AddressType = AddressTypeConstants.ShipTo,
                City = "Boise",
                State = "ID"
            });
            return order;
        }

    }
}
