using System.Linq;
using AFPST.Common.Infrastructure;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;
using EDIDocsProcessing.Core.DocsOut.Impl;
using FedexEDIDocsOut;
using Moq;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.Core.DocsOut
{
    [TestFixture]
    public class CreateEdiDocumentFromTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _sut = CreateSUT();
            _resolver = new Mock<IBusinessPartnerSpecificServiceResolver>();
            _resolver.Setup(r => r.GetBuildValueFactoryFor(It.IsAny<BusinessPartner>())).Returns(
                new FedExBuildValueFactory());
            _seg = new SegmentFactory(_resolver.Object);
            _seg.SetBuildValues(BusinessPartner.FedEx);
        }

        #endregion

        private testMessage _message;
        private Mock<IAFPSTConfiguration> _configuration;
        private SegmentFactory _seg;
        private ICreateEdiDocumentFrom<testMessage> _sut;
        private Mock<IBusinessPartnerSpecificServiceResolver> _resolver;

        private ICreateEdiDocumentFrom<testMessage> CreateSUT()
        {
            _message = new testMessage();
            _configuration = new Mock<IAFPSTConfiguration>();
            return new CreateEdiDocumentFrom<testMessage>(_configuration.Object);
        }


        [Test]
        public void can_create_document()
        {
            //ARRANGE
            var testMessageDocumentCreator = new Mock<ICreateEdiContentFrom<testMessage>>();
            //testMessageDocumentCreator.Setup(x => x.ReceiverId).Returns("ReceiverId"); 
            var transactionSet = new EDIXmlTransactionSet(_seg);
            transactionSet.ISA = new ISAEntity {ControlNumber = 1, GroupID = "IN"};
            testMessageDocumentCreator.Setup(x => x.BuildFromMessage(_message)).Returns(transactionSet);
            testMessageDocumentCreator.Setup(x => x.SegmentFactory).Returns(_seg);
            _configuration.Setup(x => x.TestMode).Returns(true);
            _resolver.Setup(r => r.GetSegmentFactoryFor(It.IsAny<BusinessPartner>())).Returns(_seg);
            //ACT
            EDIXmlInterchangeControl expectedDocument =
                _sut.CreateDocumentWith(testMessageDocumentCreator.Object, _message).GetInterchangeControl();

            //ASSERT
            testMessageDocumentCreator.VerifyAll();
            _configuration.VerifyAll();
            Assert.That(expectedDocument.EDIFunctionGroups()
                            .First().EDITransactions()
                            .First().Value
                        == transactionSet.Value);
        }
    }

    public class testMessage
    {
    }
}