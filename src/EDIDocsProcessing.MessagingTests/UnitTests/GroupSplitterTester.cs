using System.Collections.Generic;
using System.Linq;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsIn.impl;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests
{
    [TestFixture]
    public class GroupSplitterTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _lst = new List<Segment>();
            _sut = new HierarchySplitter();
        }

        #endregion

        private List<Segment> _lst;
        private IHierarchySplitter _sut;

        [Test]
        public void can_split_file_contents()
        {
            _lst.Add(new Segment
                         {
                             Contents =
                                 "ISA*00*          *00*          *ZZ*SLRSCOREGTWY   *12*EEC5122516063  *071031*0925*U*00401*000000007*0*P*~~",
                             Label = SegmentLabel.InterchangeLabel
                         });
            _lst.Add(new Segment
                         {
                             Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~",
                             Label = SegmentLabel.GroupLabel
                         });
            _lst.Add(new Segment {Contents = "GE*1*7~", Label = SegmentLabel.GroupClose});
            _lst.Add(new Segment
                         {
                             Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~",
                             Label = SegmentLabel.GroupLabel
                         });
            _lst.Add(new Segment {Contents = "GE*1*7~", Label = SegmentLabel.GroupClose});
            _lst.Add(new Segment
                         {
                             Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~",
                             Label = SegmentLabel.GroupLabel
                         });
            _lst.Add(new Segment {Contents = "GE*1*7~", Label = SegmentLabel.GroupClose});
            _lst.Add(new Segment
                         {
                             Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~",
                             Label = SegmentLabel.GroupLabel
                         });
            _lst.Add(new Segment {Contents = "GE*1*7~", Label = SegmentLabel.GroupClose});
            _lst.Add(new Segment { Contents = "IEA*1*000000010~", Label = SegmentLabel.InterchangeClose });
            var segs = new EdiSegmentCollection(_lst, "*");
            var result = _sut.SplitByGroup(segs, new InterchangeContainer(segs));
            Assert.That(result.Count() == 4);
        }

        [Test, ExpectedException]
        public void will_fail_with_badly_formed_file_contents()
        {
            _lst.Add(new Segment
                         {
                             Contents =
                                 "ISA*00*          *00*          *ZZ*SLRSCOREGTWY   *12*EEC5122516063  *071031*0925*U*00401*000000007*0*P*~~",
                             Label = SegmentLabel.InterchangeLabel
                         });
            _lst.Add(new Segment
                         {
                             Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~",
                             Label = SegmentLabel.GroupLabel
                         });
            _lst.Add(new Segment {Contents = "GE*1*7~", Label = SegmentLabel.GroupClose});
            _lst.Add(new Segment {Contents = "IEA*1*000000007~", Label = SegmentLabel.InterchangeClose});
            _lst.Add(new Segment
                         {
                             Contents =
                                 "ISA*00*          *00*          *ZZ*SLRSCOREGTWY   *12*EEC5122516063  *071031*0925*U*00401*000000008*0*P*~~",
                             Label = SegmentLabel.InterchangeLabel
                         });
            _lst.Add(new Segment
                         {
                             Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~",
                             Label = SegmentLabel.GroupLabel
                         });
            _lst.Add(new Segment {Contents = "GE*1*7~", Label = SegmentLabel.GroupClose});
            _lst.Add(new Segment {Contents = "IEA*1*000000008~", Label = SegmentLabel.InterchangeClose});
            _lst.Add(new Segment
                         {
                             Contents =
                                 "ISA*00*          *00*          *ZZ*SLRSCOREGTWY   *12*EEC5122516063  *071031*0925*U*00401*000000009*0*P*~~",
                             Label = SegmentLabel.InterchangeLabel
                         });
            _lst.Add(new Segment
                         {
                             Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~",
                             Label = SegmentLabel.GroupLabel
                         });
            _lst.Add(new Segment {Contents = "GE*1*7~", Label = SegmentLabel.GroupClose});
            _lst.Add(new Segment {Contents = "IEA*1*000000009~", Label = SegmentLabel.InterchangeClose});
            _lst.Add(new Segment
                         {
                             Contents =
                                 "ISA*00*          *00*          *ZZ*SLRSCOREGTWY   *12*EEC5122516063  *071031*0925*U*00401*000000010*0*P*~~",
                             Label = SegmentLabel.InterchangeLabel
                         });
            _lst.Add(new Segment
                         {
                             Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~",
                             Label = SegmentLabel.GroupLabel
                         });
            _lst.Add(new Segment { Contents = "IEA*1*000000010~", Label = SegmentLabel.InterchangeClose });
            var segs = new EdiSegmentCollection(_lst, "*");
            _sut.SplitByGroup(segs, new InterchangeContainer(segs));
        }
    }
}