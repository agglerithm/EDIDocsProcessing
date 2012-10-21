using EDIDocsOut;
using EDIDocsProcessing.Core.DocsOut;
using EdiMessages;
using NUnit.Framework;
using Container=EDIDocsProcessing.Common.Infrastructure.Container;

namespace EDIDocsProcessing.Tests.IntegrationTests.Fedex
{
    [TestFixture]
    public class FedexOrderAckServiceTester
    {
        private IOrderAckService _sut; 

        [TestFixtureSetUp]
        public void SetUp()
        {
            Container.Reset();
            Environment.Setup();

            _sut = Container.Resolve<IOrderAckService>();
        }

        [Test]
        public void can_acknowledge_an_order()
        {
            _sut.Acknowledge(get_message()); 
        }

        private static CreateOrderMessage get_message()
        {
            var line = new CreateOrderMessage() {ControlNumber = "23432", CustomerPO = "23432"};
            line.Add(new CustomerOrderLine() {LineNumber = 1});
            return line;
        }
 
    }
}