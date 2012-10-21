using System;
using AFPST.Common.Structures;
using EDIDocsProcessing.Common;
using EdiMessages;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests
{
    [TestFixture]
    public class CustomerEntityTester
    {

        [SetUp]
        public void SetUp()
        {
            _sut = new Customer { CustomerIDs = new CustomerAliases()
                                                    {
                                                        LegacyCustomerID = BusinessPartner.Initech.Code
                                                    }, CustomerName = "Initech" };
            _billToAddress = new Address
                         {
                             AddressName = "Initech Headquarters",
                             Address1 = "21 Initech Way",
                             AddressCode = new AddressCode { CustomerCode = "BillTo" },
                             AddressType = AddressTypeConstants.BillTo
                         };
            _shipToAddress = new Address
                         {
                             AddressName = "Initech Airport",
                             Address1 = "5454 Mockingbird Ln",
                             AddressType = AddressTypeConstants.ShipTo
                         };
            _nonSpecificAddress = new Address
                        {
                            AddressName = "Initech Airport",
                            Address1 = "5454 Mockingbird Ln"
                        };
        }


        private Customer _sut;
        private Address _billToAddress;
        private Address _shipToAddress;
        private Address _nonSpecificAddress;

        [Test]
        public void can_set_bill_to_address()
        {
            _sut.BillToAddress = _billToAddress;
        }

        [Test,ExpectedException(typeof(Exception))]
        public void will_prevent_setting_ship_to_address_as_the_bill_to_address()
        {
            _sut.BillToAddress = _shipToAddress;
        }

        [Test, ExpectedException(typeof(Exception))]
        public void will_prevent_setting_non_bill_to_address_as_the_bill_to_address()
        {
            _sut.BillToAddress = _nonSpecificAddress;
        }
    }
}