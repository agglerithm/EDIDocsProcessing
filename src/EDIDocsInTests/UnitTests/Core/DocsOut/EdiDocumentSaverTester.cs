using AFPST.Common.Infrastructure;
using AFPST.Common.Services;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.Impl;
using Moq;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.Core.DocsOut
{
    [TestFixture]
    public class EdiDocumentSaverTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _sut = CreateSUT();
        }

        #endregion

        private Mock<IAFPSTConfiguration> _configuration;
        private Mock<IFileUtilities> _utilities;
        private Mock<ISegmentFactory> _factory;
        private IEdiDocumentSaver _sut;

        private IEdiDocumentSaver CreateSUT()
        {
            _configuration = new Mock<IAFPSTConfiguration>();
            _utilities = new Mock<IFileUtilities>();
            _factory = new Mock<ISegmentFactory>();
            return new EdiDocumentSaver(_configuration.Object, _utilities.Object);
        }


        [Test]
        public void can_save_edi_documents()
        {
            //ARRANGE
            var document = new Mock<EDIXmlInterchangeControl>(_factory.Object);
            var package = new EDITransmissionPackage(document.Object, BusinessPartner.Initech);

//            _configuration.Setup(x => x.WorkingFolder).Returns("WorkingFolder");
//            _configuration.Setup(x => x.UploadFolder).Returns("UploadFolder");
            document.Setup(x => x.ControlNumber).Returns("1");
            document.SetupGet(x => x.FunctionalID).Returns("SH"); 

            //ACT
            _sut.Save(package);

            //ASSERT
//            _configuration.VerifyAll();
            document.VerifyAll(); 
        }
    }
}