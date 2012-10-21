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
    public class ShippedOrderMessageSubscriberTester
    {
        [SetUp]
        public void SetUp()
        {
            _sut = CreateSUT();
            _resolver = new Mock<IBusinessPartnerSpecificServiceResolver>();
            _seg = new SegmentFactory(_resolver.Object);
        }

        private Mock<IBusinessPartnerResolver<OrderHasBeenShippedMessage>> _businessPartnerResolver;
        private Mock<ICreateEdiDocumentFrom<OrderHasBeenShippedMessage>> _ediDocumentCreator;
        private Mock<IEdiDocumentSaver> _ediDocumentSaver;
        private Mock<IAcceptMessages> _acceptMessages;
        private Subscriber<OrderHasBeenShippedMessage> _sut;
        private SegmentFactory _seg;
        private Mock<IBusinessPartnerSpecificServiceResolver> _resolver;
        private Mock<IExecutePostConsumeAction> _executePostConsumeAction;

        private Subscriber<OrderHasBeenShippedMessage> CreateSUT()
        {
            _businessPartnerResolver = new Mock<IBusinessPartnerResolver<OrderHasBeenShippedMessage>>();
            _ediDocumentCreator = new Mock<ICreateEdiDocumentFrom<OrderHasBeenShippedMessage>>();
            _ediDocumentSaver = new Mock<IEdiDocumentSaver>();
            _acceptMessages = new Mock<IAcceptMessages>();
            _executePostConsumeAction = new Mock<IExecutePostConsumeAction>();

            return new Subscriber<OrderHasBeenShippedMessage>(_businessPartnerResolver.Object,
                                                       _ediDocumentCreator.Object, 
                                                       _ediDocumentSaver.Object,
                                                       _acceptMessages.Object, _executePostConsumeAction.Object);
        }

        [Test]
        public void can_consume_shipped_order_message()
        {
            //ARRANGE
            var message = new OrderHasBeenShippedMessage();
            message.Add(new ShippedLine());
            var ediContentCreator = new Mock<ICreateEdiContentFrom<OrderHasBeenShippedMessage>>();
            var buildValues = new EdiXmlBuildValues {ElementDelimiter = "~", SegmentDelimiter = "\n"};
//            ediContentCreator.Setup(c => c.ReceiverId).Returns("xxxi");
//            ediContentCreator.Setup(c => c.ControlNumber).Returns(62);
//            ediContentCreator.Setup(c => c.EdiXmlBuildValues).Returns(buildValues);
//            ediContentCreator.Setup(c => c.FunctionalId).Returns("PO");

            _resolver.Setup(r => r.GetBuildValueFactoryFor(BusinessPartner.FedEx)).Returns(new FedExBuildValueFactory());
            _businessPartnerResolver.Setup(x => x.ResolveFrom(message)).Returns(ediContentCreator.Object);
            var document = new EDIXmlInterchangeControl(_seg);
            _ediDocumentCreator.Setup(x => x.CreateDocumentWith(ediContentCreator.Object, message))
                .Returns(new EDITransmissionPackage(document, BusinessPartner.FedEx));
            _ediDocumentSaver.Setup(x => x.Save(It.IsAny<EDITransmissionPackage>()));
            _acceptMessages.Setup(x => x.Accept(message)).Returns(true);

            //ACT
            _sut.Consume(message);

            //ASSERT
            _ediDocumentSaver.VerifyAll();
            _ediDocumentCreator.VerifyAll();
            _businessPartnerResolver.VerifyAll();
        }

    }
}