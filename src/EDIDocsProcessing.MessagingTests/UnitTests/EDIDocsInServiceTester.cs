using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AFPST.Common.Infrastructure;
using AFPST.Common.Services.Logging;
using Castle.Core.Logging;
using EDIDocsIn;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.Publishers;
using EDIDocsProcessing.Tests.UnitTests.Fedex;
using EdiMessages;
using FedexEDIDocsIn;
using Microsoft.Practices.ServiceLocation;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace EDIDocsProcessing.Tests.UnitTests
{
    [TestFixture]
    public class EDIDocsInServiceTester
    {
        private Mock<IEdiFileProcessingService> _fileProcessingService;
        private Mock<IEdiMessagePublisher> _createOrderMessagePublisher;


        private EdiDocsInWorker _sut;

        [TestFixtureSetUp]
        public void SetUp()
        {
            TestRegistry.SetUpTestRegistry();
            _fileProcessingService = new Mock<IEdiFileProcessingService>();
            _createOrderMessagePublisher = new Mock<IEdiMessagePublisher>();
            _sut = new EdiDocsInWorker(_createOrderMessagePublisher.Object, _fileProcessingService.Object);
        }

        [TestFixtureTearDown]
        public void TearDown()
        { 
        }

        [Test]
        public void can_do_work()
        {
            IEnumerable<IEdiMessage> createOrderMessages =
                new List<IEdiMessage> { new OrderRequestReceivedMessage() };
            _fileProcessingService.Setup(x => x.ProcessFiles(TransmissionPath.FedEx)).Returns(createOrderMessages);
            _fileProcessingService.Setup(x => x.ProcessFiles(TransmissionPath.Edict)).Returns(createOrderMessages);
            _sut.DoWork(null);  
            _fileProcessingService.VerifyAll();
        }
    }

    internal class TestPublisher:IEdiMessagePublisher
    {
        public void PublishMessages(IEnumerable<IEdiMessage> customerOrders)
        { 
        }

        public bool CanPublish(IEdiMessage msg)
        {
            return true;
        }

        public void PublishMessage(IEdiMessage ediMessage)
        { 
        }
    }
}