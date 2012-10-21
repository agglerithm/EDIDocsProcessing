using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsIn.impl;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.Core.DocsIn
{
    [TestFixture]
    public class SegmentExtractorTester
    {
        private ISegmentExtractor _sut;
        [TestFixtureSetUp]
        public void SetUpForAllTests()
        {
            _sut = new SegmentExtractor();
        }
        [SetUp]
        public void SetUpForEachTest()
        {

        }

        [Test]
        public void can_extract_segments()
        {
            var segList = get_list();
            var cnt = segList.Count;
            _sut.RegisterSegmentList(segList);
            var beginList = _sut.ExtractSegment(s => s.Label == SegmentLabel.POBegin,
                                    s => s.Label == SegmentLabel.DocumentClose);
            var refList = _sut.ExtractSegment(s => s.Label == SegmentLabel.ReferenceLabel,
                                              s => s.Label == SegmentLabel.PurchaseOrderChange);
            var addressList =
                _sut.ExtractSegment(
                    s => s.Label == SegmentLabel.AddressNameLabel || s.Label == SegmentLabel.AddressLineLabel
                         || s.Label == SegmentLabel.GeographicLabel,
                    s => s.Label == SegmentLabel.PurchaseOrderChange);

            var lineList = _sut.ExtractSegment(
                    s => s.Label == SegmentLabel.ReferenceLabel || s.Label == SegmentLabel.PurchaseOrderChange
                         || s.Label == SegmentLabel.PricingInformation || s.Label == SegmentLabel.ProductItemDescription,
                    s => s.Label == SegmentLabel.DocumentClose);

            var nothingList = _sut.ExtractSegment(
                s => s.Label == SegmentLabel.ScheduleLabel, s => s.Label == SegmentLabel.DocumentClose);

            var noEndList = _sut.ExtractSegment(
                s => s.Label == SegmentLabel.DocumentClose, s => s.Label == SegmentLabel.POBegin);

            segList.Count.ShouldEqual(cnt);
            _sut.ExtractedCount.ShouldEqual(cnt - 4);
            beginList.Count.ShouldEqual(1);
            refList.Count.ShouldEqual(2);
            addressList.Count.ShouldEqual(6);
            lineList.Count.ShouldEqual(12);
            nothingList.Count.ShouldEqual(0);
            noEndList.Count.ShouldEqual(1);
        }

        [Test, ExpectedException(typeof(InvalidEDIDocumentException))]
        public void will_fail_if_segment_list_not_validated()
        {
            var segList = get_list();
            _sut.RegisterSegmentList(segList);
            List<Segment> lst = _sut.ExtractSegment(
                s => s.Label == SegmentLabel.AddressNameLabel || s.Label == SegmentLabel.AddressLineLabel
                     || s.Label == SegmentLabel.GeographicLabel,
                s => s.Label == SegmentLabel.PurchaseOrderChange);
            _sut.ValidateSegmentList();
        }

        [Test, ExpectedException(typeof(InvalidEDIDocumentException))]
        public void will_fail_if_segment_list_not_registered()
        {
            var segList = get_list(); 
            List<Segment> lst = _sut.ExtractSegment(
                s => s.Label == SegmentLabel.AddressNameLabel || s.Label == SegmentLabel.AddressLineLabel
                     || s.Label == SegmentLabel.GeographicLabel,
                s => s.Label == SegmentLabel.PurchaseOrderChange);
            _sut.ValidateSegmentList();
        }

        private List<Segment> get_list()
        {
            return new List<Segment>
                              {
                                  new Segment {Label = SegmentLabel.DocumentLabel},
                                  new Segment {Label = SegmentLabel.POBegin},
                                  new Segment {Label = SegmentLabel.ReferenceLabel},
                                  new Segment {Label = SegmentLabel.DateTimeReference},
                                  new Segment {Label = SegmentLabel.ReferenceLabel},
                                  new Segment {Label = SegmentLabel.ContactLabel},
                                  new Segment() {Label = SegmentLabel.AddressNameLabel},
                                  new Segment() {Label = SegmentLabel.AddressLineLabel},
                                  new Segment() {Label = SegmentLabel.GeographicLabel},
                                  new Segment() {Label = SegmentLabel.AddressNameLabel},
                                  new Segment() {Label = SegmentLabel.AddressLineLabel},
                                  new Segment() {Label = SegmentLabel.GeographicLabel},
                                  new Segment {Label = SegmentLabel.PurchaseOrderChange},
                                  new Segment {Label = SegmentLabel.ReferenceLabel},
                                  new Segment {Label = SegmentLabel.PricingInformation},
                                  new Segment {Label = SegmentLabel.ProductItemDescription},
                                  new Segment {Label = SegmentLabel.PurchaseOrderChange},
                                  new Segment {Label = SegmentLabel.ReferenceLabel},
                                  new Segment {Label = SegmentLabel.PricingInformation},
                                  new Segment {Label = SegmentLabel.ProductItemDescription},
                                  new Segment {Label = SegmentLabel.PurchaseOrderChange},
                                  new Segment {Label = SegmentLabel.ReferenceLabel},
                                  new Segment {Label = SegmentLabel.PricingInformation},
                                  new Segment {Label = SegmentLabel.ProductItemDescription},
                                  new Segment {Label = SegmentLabel.SummaryLabel},
                                  new Segment {Label = SegmentLabel.DocumentClose}
                              };
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
