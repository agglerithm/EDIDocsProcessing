using AFPST.Common;
using AFPST.Common.Enumerations;
using AFPST.Common.Structures;
using EdiMessages;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.EdiMessages
{
    [TestFixture]
    public class AddressTester
    {
        [Test]
        public void can_convert_Edi_AddressTypes_to_AutomationAddressTypes()
        {
            //ARRANGE
            var shipToAddress = new Address {AddressType = "ST"};
            var billToAddress = new Address {AddressType = "BT"};
            var shipFromAddress = new Address {AddressType = "SF"};

            //ACT
            string automationShipToAddressType = shipToAddress.GetAutomationAddressType();
            string automationBillToAddressType = billToAddress.GetAutomationAddressType();
            string automationShipFromAddressType = shipFromAddress.GetAutomationAddressType();

            //ASSERT
            automationShipToAddressType.ShouldEqual(AddressType.ShipTo.Text);
            automationBillToAddressType.ShouldEqual("billTo");
            automationShipFromAddressType.ShouldEqual("shipFrom");
        }
    }
}