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
    public class SubscriberTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _sut = CreateSUT();
            _resolver = new Mock<IBusinessPartnerSpecificServiceResolver>();
            _resolver.Setup(r => r.GetBuildValueFactoryFor(BusinessPartner.FedEx)).Returns(new FedExBuildValueFactory());
            _seg = new SegmentFactory(_resolver.Object);
        }

        #endregion

        private Mock<IBusinessPartnerResolver<OrderHasBeenShippedMessage>> _businessPartnerResolver;
        private Mock<ICreateEdiDocumentFrom<OrderHasBeenShippedMessage>> _ediDocumentCreator;
        private Mock<IEdiDocumentSaver> _ediDocumentSaver;
        private Mock<IAcceptMessages> _acceptMessages;
        private Mock<IExecutePostConsumeAction> _executePostConsumeAction;

        private Mock<IBusinessPartnerSpecificServiceResolver> _resolver;
        private Subscriber<OrderHasBeenShippedMessage> _sut;

        private SegmentFactory _seg;

        private Subscriber<OrderHasBeenShippedMessage> CreateSUT()
        {
            _businessPartnerResolver = new Mock<IBusinessPartnerResolver<OrderHasBeenShippedMessage>>();
            _ediDocumentCreator = new Mock<ICreateEdiDocumentFrom<OrderHasBeenShippedMessage>>();
            _ediDocumentSaver = new Mock<IEdiDocumentSaver>();
            _acceptMessages = new Mock<IAcceptMessages>();
            _executePostConsumeAction = new Mock<IExecutePostConsumeAction>();


            //return new Subscriber<OrderHasBeenShippedMessage>(_businessPartnerResolver.Object,  _ediDocumentSaver.Object);

            //return new ShippedOrderMessageSubscriber(_businessPartnerResolver.Object,  _ediDocumentSaver.Object);
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
            _businessPartnerResolver.Setup(x => x.ResolveFrom(message)).Returns(ediContentCreator.Object);
            var document = new EDIXmlInterchangeControl(_seg);
            var package = new EDITransmissionPackage(document, BusinessPartner.FedEx);
            _ediDocumentCreator.Setup(x => x.CreateDocumentWith(ediContentCreator.Object, message))
                .Returns(package);
            _ediDocumentSaver.Setup(x => x.Save(It.IsAny<EDITransmissionPackage>()));
            _acceptMessages.Setup(x => x.Accept(message)).Returns(true);

            //ACT
            _sut.Consume(message);

            //ASSERT
            _executePostConsumeAction.Verify(x => x.Execute(message));
            _ediDocumentSaver.VerifyAll();
            _ediDocumentCreator.VerifyAll();
            _businessPartnerResolver.VerifyAll();
        }
    }
}