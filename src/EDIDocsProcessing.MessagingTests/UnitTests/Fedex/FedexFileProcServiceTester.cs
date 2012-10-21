

namespace EDIDocsProcessing.Tests.UnitTests.Fedex
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AFPST.Common.Infrastructure;
    using AFPST.Common.IO;
    using AFPST.Common.Services;
    using Common;
    using global::EdiMessages;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class FedexFileProcServiceTester
    {
        private IEdiFileProcessingService _sut;
        private Mock<IFileUtilities> _fileUtilities;
        private Mock<IAFPSTConfiguration> _config;
        private Mock<IFileParser> _parser;
        private Mock<IBusinessPartnerSpecificServiceResolver> _resolver;
        private Mock<IAssignDocumentsToPartners> _assigner;

        [TestFixtureSetUp]
        public void SetUp()
        {
            TestRegistry.SetUpTestRegistry();
            var retList =   new List<OrderRequestReceivedMessage>();
            move_file();
            IList<FileEntity> files = get_file_list();
            _fileUtilities = new Mock<IFileUtilities>();
            _fileUtilities.Setup(
                fu => fu.GetListFromFolder(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns((List<FileEntity>)files); 
            _config = new Mock<IAFPSTConfiguration>();
            _resolver = new Mock<IBusinessPartnerSpecificServiceResolver>();
            _config.Setup(c => c.GetSettingBasedOnTestMode(It.IsAny<string>())).Returns(Directory.GetCurrentDirectory());
            _parser = new Mock<IFileParser>();
            _assigner = new Mock<IAssignDocumentsToPartners>();
            _resolver.Setup(r => r.GetFileParserFor(BusinessPartner.FedEx)).Returns(_parser.Object);
            _parser.Setup(p => p.Parse(It.IsAny<EdiSegmentCollection>())).Returns(retList.Select(r => (IEdiMessage) r)); 
            // _ackSrv.Setup(s => s.AcknowledgeAll(It.IsAny <List<CreateOrderMessage>>()));
            _sut = new EdiFileProcessingService(_fileUtilities.Object, _assigner .Object, null);
        }



        private static void move_file()
        {
            try
            {
                File.Move(@"..\..\TestFiles\Archive\Sample850X12.edi", @"..\..\TestFiles\Sample850X12.edi");
            }
            catch
            {
            }
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
        }

        private static IList<FileEntity> get_file_list()
        {
            var retVal = new List<FileEntity>();
            var dinf = new DirectoryInfo(@"..\..\TestFiles\");
            FileInfo[] lst = dinf.GetFiles("Sample*.edi");
            foreach (FileInfo f in  lst)
            {
                retVal.Add(new FileEntity {FullPath = f.Directory.FullName + @"\" + f.Name, Size = (int) f.Length});
            }
            return retVal;
        }

        [Test]
        public void can_process_file()
        {
            _sut.ProcessFiles(TransmissionPath.FedEx);
            _fileUtilities.VerifyAll();
 
        }
    }

    internal class TestFileParser : IFileParser 
    {
        public IEnumerable<IEdiMessage> Parse(string contents)
        {
            return new List<IEdiMessage>();
        }

        public bool CanParseSenderId(string senderId)
        {
            throw new NotImplementedException();
        }

        public bool CanParseFor(BusinessPartner partner)
        {
            return true;
        }

        public IEnumerable<IEdiMessage> Parse(EdiSegmentCollection segList)
        {
            return new List<IEdiMessage>();
        }
    }
}