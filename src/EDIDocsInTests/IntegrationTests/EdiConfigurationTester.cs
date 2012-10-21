using AFPST.Common.Infrastructure;
using AFPST.Common.Infrastructure.impl;
using EDIDocsProcessing.Common.Extensions;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests
{
    [TestFixture]
    public class AFPSTConfigurationTester
    {
        private IAFPSTConfiguration _sut;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _sut = new AFPSTConfiguration();
        }

        [Test]
        public void can_get_folders()
        {
            Assert.That(_sut.WorkingFolder() == @"..\..\TestFiles\Working\");
            Assert.That(_sut.GetUploadFolderFor("Initech") == @"\\automation\d$\Upload\Initech\Test\");
            Assert.That(_sut.GetUploadFolderFor("Edict") == @"\\automation\d$\Upload\Test\");
            Assert.That(_sut.DownloadFolder(), Is.StringContaining(@"..\..\TestFiles\Download\"
));
        }

        [Test]
        public void can_get_test_mode()
        {
            Assert.That(_sut.TestMode);
        }
    }
}