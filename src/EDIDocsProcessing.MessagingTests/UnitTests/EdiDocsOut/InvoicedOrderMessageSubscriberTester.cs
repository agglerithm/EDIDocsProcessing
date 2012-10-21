using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;
using EdiMessages;
using FedexEDIDocsOut;
using Moq;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.EdiDocsOut
{
    [TestFixture]
    public class InvoicedOrderMessageSubscriberTester
    {

        [SetUp]
        public void SetUp()
        {
            _sut = CreateSUT();
            _resolver = new Mock<IBusinessPartnerSpecificServiceResolver>();
            _resolver.Setup(r => r.GetBuildValueFactoryFor(It.IsAny<BusinessPartner>())).Returns(
                new FedExBuildValueFactory());
            _seg = new SegmentFactory(_resolver.Object);
        }


        private Mock<IBusinessPartnerResolver<InvoicedOrderMessage>> _businessPartnerResolver;
        private Mock<ICreateEdiDocumentFrom<InvoicedOrderMessage>> _ediDocumentCreator;
        private Mock<IEdiDocumentSaver> _ediDocumentSaver;
        private Subscriber<InvoicedOrderMessage> _sut;
        private SegmentFactory _seg;
        private Mock<IBusinessPartnerSpecificServiceResolver> _resolver;
        private Mock<IAcceptMessages> _acceptMessages;
        private Mock<IExecutePostConsumeAction> _executePostConsumeAction;

        private Subscriber<InvoicedOrderMessage> CreateSUT()
        {
            _businessPartnerResolver = new Mock<IBusinessPartnerResolver<InvoicedOrderMessage>>();
            _ediDocumentCreator = new Mock<ICreateEdiDocumentFrom<InvoicedOrderMessage>>();
            _ediDocumentSaver = new Mock<IEdiDocumentSaver>();
            _acceptMessages = new Mock<IAcceptMessages>();
            _executePostConsumeAction = new Mock<IExecutePostConsumeAction>();

            return new Subscriber<InvoicedOrderMessage>(_businessPartnerResolver.Object, _ediDocumentCreator.Object,
                                                        _ediDocumentSaver.Object, _acceptMessages.Object, _executePostConsumeAction.Object);
        }

        [Test]
        public void can_consume_shipped_order_message()
        {
            //ARRANGE
            var message = new InvoicedOrderMessage();
            var ediContentCreator = new Mock<ICreateEdiContentFrom<InvoicedOrderMessage>>();
            _businessPartnerResolver.Setup(x => x.ResolveFrom(message)).Returns(ediContentCreator.Object);
            var package = new EDITransmissionPackage(new EDIXmlInterchangeControl(_seg), BusinessPartner.FedEx);
            _ediDocumentCreator.Setup(x => x.CreateDocumentWith(ediContentCreator.Object, message))
                .Returns(package);
            _acceptMessages.Setup(x => x.Accept(message)).Returns(true);
            _ediDocumentSaver.Setup(x => x.Save(package));

            //ACT
            _sut.Consume(message);

            //ASSERT
            _ediDocumentSaver.VerifyAll();
            _ediDocumentCreator.VerifyAll();
            _businessPartnerResolver.VerifyAll();
        }
    }
}