using System.Collections.Generic;
using System.IO;
using System.Linq;
using AFPST.Common.Infrastructure;
using EDIDocsIn.config;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.Extensions;
using EdiMessages;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests.Initech
{
    [TestFixture]
    public class InitechFileProcServiceTester
    {
        private IEdiFileProcessingService  _sut;

        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();
            _sut = ServiceLocator.Current.GetInstance<IEdiFileProcessingService >();

            ensureFilesAreReadyToProcess();
        }

        private void ensureFilesAreReadyToProcess()
        {
            string downloadFolder = EdiConfig.GetDownloadFolderFor(BusinessPartner.Initech.Code);
            bool thereAreFilesReadyToProcess = Directory.GetFiles(downloadFolder).Length > 0;
            if (thereAreFilesReadyToProcess == false) moveFilesFromArchive(downloadFolder);
        }

        private void moveFilesFromArchive(string downloadFolder)
        {
            List<string> files = Directory.GetFiles(downloadFolder + @"Archive\").ToList();
            files.ForEach(f =>
                              {
                                  string fileName = new FileInfo(f).Name;
                                  File.Move(f, downloadFolder + fileName);
                              });
        }


        [TestFixtureTearDown]
        public void TearDown()
        {
            ensureFilesAreReadyToProcess();
        }

        [Test]
        public void can_process_file()
        {
            var msgs =   _sut.ProcessFiles(TransmissionPath.Initech);
            Assert.That(msgs != null);
        }
    }
}