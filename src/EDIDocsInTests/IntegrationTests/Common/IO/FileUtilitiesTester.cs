using System.IO;
using AFPST.Common.IO;
using AFPST.Common.Services.imp;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests.Common.IO
{
    [TestFixture]
    public class FileUtilitiesTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _sut = CreateSUT();
        }

        #endregion

        private FileUtilities _sut;


        private FileUtilities CreateSUT()
        {
            return new FileUtilities();
        }


        [Test]
        public void can_move_files_with_override()
        {
            //ARRANGE
            if (File.Exists(@".\testto\test.xml")) File.Delete(@".\testto\test.xml");
            Directory.CreateDirectory("testfrom");
            Directory.CreateDirectory("testto");
            File.WriteAllText(@".\testfrom\test.xml", "1");
            //ACT
            _sut.MoveFileWithoutOverwrite(@".\testfrom\test.xml", @".\testto\test.xml");

            //ASSERT
            Assert.That(File.ReadAllText(@".\testto\test.xml"), Is.EqualTo("1"));

            //ACT
            File.WriteAllText(@".\testfrom\test2.xml", "2");

            _sut.MoveFileWithOverwrite(@".\testfrom\test2.xml", @".\testto\test.xml");
            Assert.That(File.ReadAllText(@".\testto\test.xml"), Is.EqualTo("2"));

            Directory.Delete("testfrom", true);
            Directory.Delete("testto", true);
        }

        [Test]
        public void can_move_files_without_override()
        {
            //ARRANGE
            if (File.Exists(@".\testto\test.xml")) File.Delete(@".\testto\test.xml");
            Directory.CreateDirectory("testfrom");
            Directory.CreateDirectory("testto");
            File.WriteAllText(@".\testfrom\test.xml", "1");
            //ACT
            _sut.MoveFileWithoutOverwrite(@".\testfrom\test.xml", @".\testto\test.xml");

            //ASSERT
            Assert.That(File.ReadAllText(@".\testto\test.xml"), Is.EqualTo("1"));

            //ACT
            File.WriteAllText(@".\testfrom\test2.xml", "2");

            _sut.MoveFileWithoutOverwrite(@".\testfrom\test2.xml", @".\testto\test.xml");
            Assert.That(File.ReadAllText(@".\testto\test.xml"), Is.EqualTo("1"));
            Assert.That(File.ReadAllText(@".\testto\testx.xml"), Is.EqualTo("2"));

            Directory.Delete("testfrom", true);
            Directory.Delete("testto", true);
        }
    }
}