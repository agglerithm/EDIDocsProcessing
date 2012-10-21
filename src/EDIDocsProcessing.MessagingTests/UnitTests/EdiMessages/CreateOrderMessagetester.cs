using AFPST.Common.Structures;
using EdiMessages;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.EdiMessages
{
    [TestFixture]
    public class CreateOrderMessagetester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _sut = CreateSUT();
        }

        #endregion

        private OrderRequestReceivedMessage _sut;

        private OrderRequestReceivedMessage CreateSUT()
        {
            return new OrderRequestReceivedMessage();
        }

        [Test]
        public void can_add_addresses()
        {
            _sut.Customer = new Customer();
            var billToAddress = new Address{AddressType = AddressTypeConstants.BillTo};
            _sut.AddAddress(billToAddress);
            var shipToAddress = new Address{AddressType = AddressTypeConstants.ShipTo};
            _sut.AddAddress(shipToAddress);

            _sut.GetBillToAddress().ShouldBeSameAs(billToAddress);
            _sut.ShipToAddress.ShouldBeSameAs(shipToAddress);
        }


        [Test]
        public void can_get_customer_id()
        {
            var customer = new Customer {CustomerIDs = new CustomerAliases(){LegacyCustomerID = "100"}};
            _sut.Customer = customer;
            _sut.CustomerIDs.LegacyCustomerID.ShouldEqual(customer.CustomerIDs.LegacyCustomerID);
        }

        [Test]
        public void can_get_number_of_lines()
        {
            _sut.Add(new CustomerOrderLine());
            _sut.Add(new CustomerOrderLine());
            _sut.Add(new CustomerOrderLine());

            _sut.LineCount.ShouldEqual(3);
        }

        [Test]
        public void can_get_string_representation_of_message()
        {
            _sut.ToString().ShouldContainText("ControlNumber");
            _sut.ControlNumber = "1234";
            _sut.ToString().ShouldContainText("1234");
            _sut.Customer = new Customer {CustomerName = "testCustomer"};
            _sut.ToString().ShouldContainText(_sut.Customer.CustomerName);
            _sut.AddAddress(new Address {City = "Austin", AddressType = AddressTypeConstants.ShipTo});
            _sut.ToString().ShouldContainText("Austin");
        }

        [Test]
        public void get_customer_id_returns_null_if_no_customer_is_contained_in_the_message()
        {
            _sut.CustomerIDs.LegacyCustomerID.ShouldEqual(null);
        }
    }
}