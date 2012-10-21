using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests
{
    using Common;
    using EDIDocsOut.config;
    using EDIDocsProcessing.Core.impl;

    [TestFixture]
    public class EdiSegmentTester
    {
        private BusinessPartnerSpecificServiceResolver _resolver;
        private ISegmentFactory _factory;

        [TestFixtureSetUp]
        public void SetUpForAllTests()
        {
            StructureMapBootstrapper.Execute();
            _resolver = new BusinessPartnerSpecificServiceResolver();
            _factory = _resolver.GetSegmentFactoryFor(BusinessPartner.Initech);

        }
        [SetUp]
        public void SetUpForEachTest()
        {

        }

        [Test]
        public void can_get_list_of_edi_elements()
        {
            var seg = _factory.GetAddressLine("123 Fake St.", "Suite 100");
            var els = seg.EdiElements;
            els.Count().ShouldEqual(2);
        }

        [Test]
        public void can_trim_trailing_empties()
        {
            var seg = _factory.GetAddressLine("123 Fake St.", "");
            seg.StripEmptyTrailingElements();
            var els = seg.EdiElements;
            els.Count().ShouldEqual(1);
            seg.Value.ShouldEqual("N3~123 Fake St.\n");
        }
        [TearDown]
        public void TearDownForEachTest()
        {

        }

        [TestFixtureTearDown]
        public void TearDownAfterAllTests()
        {

        }
    }


    [TestFixture]
    public class EdiTransactionSetTester
    {
        private BusinessPartnerSpecificServiceResolver _resolver;
        private ISegmentFactory _factory;

        [TestFixtureSetUp]
        public void SetUpForAllTests()
        {
            StructureMapBootstrapper.Execute();
            _resolver = new BusinessPartnerSpecificServiceResolver();
            _factory = _resolver.GetSegmentFactoryFor(BusinessPartner.VandalayIndustries);

        }
        [SetUp]
        public void SetUpForEachTest()
        {

        }

        [Test]
        public void can_get_list_of_edi_segments()
        {
            var seg = _factory.GetAddressLine("123 Fake St.", "Suite 100");
            var els = seg.EdiElements;
            els.Count().ShouldEqual(2);
        }

        [Test]
        public void can_trim_trailing_empties()
        {
            var seg = _factory.GetAddressLine("123 Fake St.", "");
            seg.StripEmptyTrailingElements();
            var els = seg.EdiElements;
            els.Count().ShouldEqual(1);
            seg.Value.ShouldEqual("N3~123 Fake St.\n");
        }
        [TearDown]
        public void TearDownForEachTest()
        {

        }

        [TestFixtureTearDown]
        public void TearDownAfterAllTests()
        {

        }
    }
}
