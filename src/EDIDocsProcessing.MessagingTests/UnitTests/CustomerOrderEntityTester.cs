using AFPST.Common.Structures;
using EdiMessages;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests
{
    [TestFixture]
    public class CustomerOrderEntityTester
    {
        private OrderRequestReceivedMessage _sut;


        [Test]
        public void can_add_address()
        {
            _sut = new OrderRequestReceivedMessage { Customer = new Customer { CustomerIDs = new CustomerAliases() { LegacyCustomerID = "BLORG" } } };
            _sut.AddAddress(new Address
                                {
                                    AddressType = AddressTypeConstants.ShipTo,
                                    AddressName = "Yop"
                                });

            Address shipTo = _sut.ShipToAddress;
            Assert.That(shipTo.AddressName == "Yop");
        }

        [Test]
        public void can_add_line()
        {
            _sut = new OrderRequestReceivedMessage();
            _sut.Add(new CustomerOrderLine
                         {
                             CustomerPartNumber = "A0897-A00",
                             Notes = "Hello",
                             RequestedQuantity = 15,
                             RequestedPrice = 5.66m,
                             LineNumber = 1
                         });
            _sut.Add(new CustomerOrderLine
                         {
                             CustomerPartNumber = "B0897-A00",
                             Notes = "Hello",
                             RequestedQuantity = 15,
                             RequestedPrice = 5.96m,
                             LineNumber = 2
                         });

            Assert.That(_sut.LineCount == 2);
            Assert.That(_sut.LineCount == _sut.LineItems.Count);
        }

        [Test]
        public void can_get_customer_id()
        {
            _sut = new OrderRequestReceivedMessage();

            string custid = _sut.CustomerIDs.LegacyCustomerID;

            Assert.That(custid == null);

            _sut.Customer = new Customer { CustomerIDs = new CustomerAliases() { LegacyCustomerID = "WALL01" }, CustomerName = "WALL-E" };

            Assert.That(_sut.CustomerIDs.LegacyCustomerID == "WALL01");
        }
    }
}