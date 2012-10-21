using System;
using System.Threading;
using AFPST.Common;
using AFPST.Common.Structures;
using EDIDocsProcessing.Common;
using EdiMessages;
using MassTransit;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace EDIDocsProcessing.MessagingTests
{
    [TestFixture]
    public class OrderRequestReceivedMessagePublisher
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        { 
        }

        [TearDown]
        public void TearDown()
        { 
        }

        #endregion
         

        [Test]
        public void publish_CreateOrderMessage()
        {
            var messaage = new OrderRequestReceivedMessage
                               {
                                   BusinessPartnerCode = BusinessPartner.Initech.Code,
                                   BusinessPartnerNumber = BusinessPartner.Initech.Number,
                                   ControlNumber = "1",
                                   CustomerPO = "PO-100",
                                   Customer = new Customer {CustomerIDs = new CustomerAliases(){LegacyCustomerID = "100"}, CustomerName = "test co."},
                                   BusinessProcessName = "business process name",
                                   RequestDate = SystemTime.Now().ToString()
                               };

            messaage.AddAddress(new Address
                                             {
                                                 Address1 = "test addr1",
                                                 Address2 = "addr2",
                                                 AddressCode = new AddressCode(){CustomerCode = "TEST" },
                                                 AddressName = "office",
                                                 AddressType = AddressTypeConstants.ShipTo,
                                                 City = "austin",
                                                 State = "TX",
                                                 Zip = "88888"
                                             });

            messaage.Add(new CustomerOrderLine
                             {
                                 CustomerIDs  = new CustomerAliases() { LegacyCustomerID = "100" },
                                 CustomerPartNumber = "222",
                                 CustomerPO = "333",
                                 ItemDescription = "desc",
                                 ItemId = "444",
                                 LineNumber = 1,
                                 Notes = "note",
                                 OrderMultiple = 1,
                                 OrderNumber = "555",
                                 RequestedPrice = 6.0m,
                                 RequestedQuantity = 2,
                                 RequestNumber = "1",
                                 TestMode = true
                             });
            messaage.LineItems.Add(new CustomerOrderLine
                                       {
                                           CustomerIDs = new CustomerAliases() { LegacyCustomerID = "100" },
                                           CustomerPartNumber = "222b",
                                           CustomerPO = "333b",
                                           ItemDescription = "descb",
                                           ItemId = "444b",
                                           LineNumber = 2,
                                           Notes = "noteb",
                                           OrderMultiple = 2,
                                           OrderNumber = "555b",
                                           RequestedPrice = 7.0m,
                                           RequestedQuantity = 3,
                                           RequestNumber = "1b",
                                           TestMode = true
                                       });

            Thread.Sleep(4000);

            Bus.Instance().Publish(messaage);
        }
    }

    [TestFixture]
    public class test
    {
        [Test]
        public void TEST_NAME()
        {
            var messaage = new OrderRequestReceivedMessage
                               {
                                   BusinessPartnerCode = BusinessPartner.Initech.Code,
                                   BusinessPartnerNumber = BusinessPartner.Initech.Number,
                                   ControlNumber = "1",
                                   CustomerPO = "PO-100",
                                   Customer = new Customer {  CustomerIDs = new CustomerAliases() { LegacyCustomerID = "100" }, CustomerName = "test co." },
                                   BusinessProcessName = "business process name",
                                   RequestDate = SystemTime.Now().ToString(),
                               };

            messaage.Add(new CustomerOrderLine
                             {
                                 CustomerIDs = new CustomerAliases() { LegacyCustomerID = "100" },
                                 CustomerPartNumber = "200",
                                 CustomerPO = "90909",
                                 ItemDescription = "description",
                                 TestMode = true
                             });

            messaage.Add(new CustomerOrderLine
                             {
                                 CustomerIDs = new CustomerAliases() { LegacyCustomerID = "200" },
                                 CustomerPartNumber = "300",
                                 CustomerPO = "40909",
                                 ItemDescription = "description2",
                                 TestMode = true
                             });

            messaage.ShipToAddress = new Address
                                    {
                                        Address1 = "addr1",
                                        Address2 = "addr2",
                                        AddressCode = new AddressCode{CustomerCode = "ship to"},
                                        AddressName = "Ship To",
                                        AddressType = "ST",
                                        City = "city",
                                        Country = "USA",
                                        State = "State",
                                        Zip = "ddd"
                                    } ;
  
            Console.WriteLine(messaage);
        }
    }
}