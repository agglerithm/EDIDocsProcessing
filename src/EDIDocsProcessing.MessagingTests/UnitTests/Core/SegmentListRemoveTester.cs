using System.Collections.Generic;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsIn.impl;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.Core
{
    [TestFixture]
    public class SegmentListRemoveTester
    {
        private List<Segment> segList = new List<Segment>();
        [TestFixtureSetUp]
        public void SetUpForAllTests()
        {
            segList.Add(new Segment{Label = SegmentLabel.DocumentLabel});
            segList.Add(new Segment { Label = SegmentLabel.POBegin });
            segList.Add(new Segment { Label = SegmentLabel.ReferenceLabel });
            segList.Add(new Segment { Label = SegmentLabel.DateTimeReference });
            segList.Add(new Segment { Label = SegmentLabel.ReferenceLabel });
            segList.Add(new Segment { Label = SegmentLabel.ContactLabel });
            segList.Add(new Segment() { Label = SegmentLabel.AddressNameLabel });
            segList.Add(new Segment() { Label = SegmentLabel.AddressLineLabel });
            segList.Add(new Segment() { Label = SegmentLabel.GeographicLabel });
            segList.Add(new Segment() { Label = SegmentLabel.AddressNameLabel });
            segList.Add(new Segment() { Label = SegmentLabel.AddressLineLabel });
            segList.Add(new Segment() { Label = SegmentLabel.GeographicLabel });
            segList.Add(new Segment { Label = SegmentLabel.PurchaseOrderChange });
            segList.Add(new Segment { Label = SegmentLabel.ReferenceLabel });
            segList.Add(new Segment { Label = SegmentLabel.PricingInformation });
            segList.Add(new Segment { Label = SegmentLabel.ProductItemDescription });
            segList.Add(new Segment { Label = SegmentLabel.PurchaseOrderChange });
            segList.Add(new Segment { Label = SegmentLabel.ReferenceLabel });
            segList.Add(new Segment { Label = SegmentLabel.PricingInformation });
            segList.Add(new Segment { Label = SegmentLabel.ProductItemDescription });
            segList.Add(new Segment { Label = SegmentLabel.PurchaseOrderChange });
            segList.Add(new Segment { Label = SegmentLabel.ReferenceLabel });
            segList.Add(new Segment { Label = SegmentLabel.PricingInformation });
            segList.Add(new Segment { Label = SegmentLabel.ProductItemDescription });
            segList.Add(new Segment { Label = SegmentLabel.SummaryLabel });
            segList.Add(new Segment { Label = SegmentLabel.DocumentClose });
        }
        [SetUp]
        public void SetUpForEachTest()
        {

        }

        [Test]
        public void can_get_groups_of_segments()
        {
            var beginList = segList.RemoveGroup(s => s.Label == SegmentLabel.POBegin,
                                                s => s.Label == SegmentLabel.DocumentClose);
            var refList = segList.RemoveGroup(s => s.Label == SegmentLabel.ReferenceLabel,
                                              s => s.Label == SegmentLabel.PurchaseOrderChange);
            var addressList =
                segList.RemoveGroup(
                    s => s.Label == SegmentLabel.AddressNameLabel || s.Label == SegmentLabel.AddressLineLabel
                         || s.Label == SegmentLabel.GeographicLabel,
                    s => s.Label == SegmentLabel.PurchaseOrderChange);

            var lineList = segList.RemoveGroup(
                    s => s.Label == SegmentLabel.ReferenceLabel || s.Label == SegmentLabel.PurchaseOrderChange
                         || s.Label == SegmentLabel.PricingInformation || s.Label == SegmentLabel.ProductItemDescription,
                    s => s.Label == SegmentLabel.DocumentClose);

            var nothingList = segList.RemoveGroup(
                s => s.Label == SegmentLabel.ScheduleLabel, s => s.Label == SegmentLabel.DocumentClose);

            var noEndList = segList.RemoveGroup(
                s => s.Label == SegmentLabel.DocumentClose, s => s.Label == SegmentLabel.POBegin);

            segList.Count.ShouldEqual(4);
            beginList.Count.ShouldEqual(1);
            refList.Count.ShouldEqual(2);
            addressList.Count.ShouldEqual(6);
            lineList.Count.ShouldEqual(12);
            nothingList.Count.ShouldEqual(0);
            noEndList.Count.ShouldEqual(1);
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
