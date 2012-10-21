using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;
using EdiMessages;
using FedexEDIDocsOut;
using Moq;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.Fedex
{
    [TestFixture]
    public class FedexOrderAckServiceTester
    {
        private IOrderAckService _sut; 
        private Mock<ICreateEdiContentFrom<CreateOrderMessage>> _builder;
        private Mock<IFileCreationService> _fileCreator;
        private Mock<IEDIXmlTransactionSet> _trans;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _trans = new Mock<IEDIXmlTransactionSet>();
            _builder = new Mock<ICreateEdiContentFrom<CreateOrderMessage>>();
            _fileCreator = new Mock<IFileCreationService>();
            _builder.Setup(b => b.BuildFromMessage(It.IsAny<CreateOrderMessage>())).Returns(_trans.Object);
            _sut = new FedexOrderAckService(_builder.Object, _fileCreator.Object);
        }

        [Test]
        public void can_acknowledge_an_order()
        {
            _sut.Acknowledge(get_message()); 
            _builder.VerifyAll();
            _fileCreator.VerifyAll();
        }

        private static CreateOrderMessage get_message()
        {
            return new CreateOrderMessage() {ControlNumber = "23432", CustomerPO = "23432"};
        }
 
    }
}