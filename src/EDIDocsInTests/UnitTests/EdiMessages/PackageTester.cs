using AFPST.Common.Structures;
using EdiMessages;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.EdiMessages
{
    [TestFixture]
    public class PackageTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _sut = new Package();
        }

        #endregion

        private Package _sut;

        [Test]
        public void can_get_string_representation_of_PackageSpecification()
        {
            _sut.IndexNumber = 222;
            _sut.ToString().ShouldContainText(222.ToString());

            _sut.Description = "description";
            _sut.ToString().ShouldContainText("description");

            _sut.Specifications = new PackageSpecification();
            _sut.Specifications.Height = 500;
            _sut.Specifications.Length = 4;
            _sut.Specifications.Weight = 100;
            _sut.Specifications.Width = 200;

            _sut.ToString().ShouldContainText(4.ToString());
            _sut.ToString().ShouldContainText(500.ToString());
            _sut.ToString().ShouldContainText(100.ToString());
            _sut.ToString().ShouldContainText(200.ToString());

            PropertyTester.TestToStringBehavior(_sut);
        }

        [Test]
        public void test_properties()
        {
            PropertyTester.TestPropertyBehavior(_sut, new[] {typeof (PackageSpecification)});
        }
    }
}