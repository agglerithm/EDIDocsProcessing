using AFPST.Common.Structures;
using EdiMessages;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.EdiMessages
{
    [TestFixture]
    public class PackageSpecificationTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _sut = new PackageSpecification();
        }

        #endregion

        private PackageSpecification _sut;

        [Test]
        public void can_get_string_representation_of_PackageSpecification()
        {
            PropertyTester.TestToStringBehavior(_sut);
        }

        [Test]
        public void test_properties()
        {
            PropertyTester.TestPropertyBehavior(_sut, null);
        }
    }
}