using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests
{
    using Common;
    using Common.EDIStructures;
    using EDIDocsOut.config; 
    using EDIDocsProcessing.Core.impl;
    using global::EdiMessages;

    [TestFixture]
    public class HierarchicalLevelLoopTester
    {
        private IBusinessPartnerSpecificServiceResolver _resolver;
        [TestFixtureSetUp]
        public void SetUpForAllTests()
        {
            StructureMapBootstrapper.Execute();
            _resolver = new BusinessPartnerSpecificServiceResolver(); 
        }
        [SetUp]
        public void SetUpForEachTest()
        {

        }

        [Test]
        public void can_add_levels()
        {
            var hll =  HierarchicalLevelLoopWrapper.BuildWrapper("O",_resolver.GetSegmentFactoryFor(BusinessPartner.VandalayIndustries), false); 
            var child = hll.AddLevel("I");
            var grandchild = hll.AddLevel("X", child);
            var grandchild2 = hll.AddLevel("X", child);

            hll.GetId().ShouldEqual("1");
            hll.GetParent().ShouldEqual("");

            child.GetId().ShouldEqual("2");
            child.GetParent().ShouldEqual("1");

            grandchild.GetId().ShouldEqual("3");
            grandchild.GetParent().ShouldEqual("2");
            grandchild2.GetId().ShouldEqual("4");

            hll.Value.ShouldEqual("HL~1~~O\nHL~2~1~I\nHL~3~2~X\nHL~4~2~X\n");
        }

        [Test]
        public void can_add_segments()
        {
            var hll = HierarchicalLevelLoopWrapper.BuildWrapper("O", _resolver.GetSegmentFactoryFor(BusinessPartner.Initech), false);  
            var child = hll.AddLevel("I");
            var grandchild = hll.AddLevel("X", child);

            hll.SegmentCount.ShouldEqual(3);

            child.Count(EdiStructureNameConstants.Segment).ShouldEqual(2);

            grandchild.Count(EdiStructureNameConstants.Segment).ShouldEqual(1); 

            grandchild.AddSegment(new EDIXmlSegment("",new EdiXmlBuildValues()));

            hll.SegmentCount.ShouldEqual(4);
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
