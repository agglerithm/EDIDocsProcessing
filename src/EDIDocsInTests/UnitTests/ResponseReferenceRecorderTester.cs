using System.Collections.Generic;
using System.Linq;
using AFPST.Common.Extensions;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsIn.impl;
using NUnit.Framework;
using FormattingExtensions = EDIDocsProcessing.Common.Extensions.FormattingExtensions;

namespace EDIDocsProcessing.Tests.UnitTests
{
    [TestFixture]
    public class ResponseReferenceRecorderTester
    {
        private IEDIResponseReferenceRecorder _recorder;
        private string _controlNumber = "1234323"; 

        [TestFixtureSetUp]
        public void SetUpForAllTests()
        {
            _recorder = new EDIResponseReferenceRecorder();
        }

        [SetUp]
        public void SetUpForEachTest()
        {

        }

        [Test]
        public void can_save_outer_refs()
        {
            List<Segment> segList = get_seg_list();
            var cnt = segList.Count();
            var lst = _recorder.MemorizeOuterReferences(segList, _controlNumber,"~",BusinessPartner.Initech);
            lst.Count().ShouldEqual(4); 
            _recorder.GetResponseValues().Count().ShouldEqual(4);
        }

        private List<Segment> get_seg_list()
        {
            return new List<Segment>()
                       {
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("ISA"),Contents = "ISA~00~          ~00~          ~ZZ~055001924VA    ~12~EEC5122516063  ~100423~1516~U~00401~000000138~0~P~^".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("GS"),Contents = "GS~PO~055001924VA~EEC5122516063~20100423~1516~121~X~004010".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("ST"),Contents = "ST~850~1210001BEG~00~NE~AFP000156~~20100423".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("REF"),Contents = "REF~I5~OR".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("REF"),Contents = "REF~19~20001".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("REF"),Contents = "REF~ZZ~CAD32".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("REF"),Contents = "REF~XE~1".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("PER"),Contents = "PER~DC~BRIAN CARLSON~TE~800-334-6325".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("N1"),Contents = "N1~ST~FED EX".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("N3"),Contents = "N3~6322 ATLANTIS DRIVEN4~APPLETON~WI~54915~US".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("PO1"),Contents = "PO1~1~3~EA~8.51~~IN~910002~VN~910002~PD~Box, Monitor 15\" Universal~TP~N".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("REF"),Contents = "REF~CO~249040201133865".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("REF"),Contents = "REF~BF~249040".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("REF"),Contents = "REF~P4~9002087".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("SCH"),Contents = "SCH~3~EA~~~002~20100423".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("SE"),Contents = "SE~16~1210001".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("GE"),Contents = "GE~1~121".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("IEA"),Contents = "IEA~1~000000138".Trim()} 
                       };
        }

        [Test]
        public void can_build_inner_responses()
        {
            var entity = get_inner_seg_list().BuildInnerResponse(1);
            Assert.That(entity != null);
            entity.LineIdentifier.ShouldEqual("1");
            entity.ResponseElements.Count.ShouldEqual(3);
            _recorder.GetLines().Count().ShouldEqual(0);

        }

        private IEnumerable<Segment> get_inner_seg_list()
        {
            return new List<Segment>()
                       {
 
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("ISA"),Contents = "ISA~00~          ~00~          ~ZZ~055001924VA    ~12~EEC5122516063  ~100423~1516~U~00401~000000138~0~P~^".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("PO1"),Contents = "PO1~1~3~EA~8.51~~IN~910002~VN~910002~PD~Box, Monitor 15\" Universal~TP~N"},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("REF"),Contents = "REF~CO~249040201133865".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("REF"),Contents = "REF~BF~249040".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("REF"),Contents = "REF~P4~9002087".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("SCH"),Contents = "SCH~3~EA~~~002~20100423".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("SE"),Contents = "SE~16~1210001".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("GE"),Contents = "GE~1~121".Trim()},
                           new Segment{Label = FormattingExtensions.GetSegmentLabel("IEA"),Contents = "IEA~1~000000138".Trim()} 
                       };
        }

        [Test]
        public void can_save_inner_refs()
        {
            IList<DocumentLineItemEntity> refs = get_inner_refs(); 
            _recorder.MemorizeInnerReferences(refs, _controlNumber,BusinessPartner.Initech);
            _recorder.GetLines().Count().ShouldEqual(3);
        }
 
        private IList<DocumentLineItemEntity> get_inner_refs()
        {
            return new List<DocumentLineItemEntity>
                       {
                           new DocumentLineItemEntity{DocumentInDTO = new DocumentInDTO{ControlNumber = _controlNumber.CastToInt()},LineIdentifier = "1",
                           ResponseElements = get_response_elements()},
                           new DocumentLineItemEntity{DocumentInDTO = new DocumentInDTO{ControlNumber = _controlNumber.CastToInt()},LineIdentifier = "2",
                           ResponseElements = get_response_elements()},
                           new DocumentLineItemEntity{DocumentInDTO = new DocumentInDTO{ControlNumber = _controlNumber.CastToInt()},LineIdentifier = "3",
                           ResponseElements = get_response_elements()},
                       };
        }

        private IList<LineResponseElementEntity> get_response_elements()
        {
            return new List<LineResponseElementEntity>
                       {
                          
                           new LineResponseElementEntity(){ElementName = "REF01" }
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
