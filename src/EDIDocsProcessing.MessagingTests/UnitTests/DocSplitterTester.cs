using System.Collections.Generic;
using System.Linq;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsIn.impl;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests
{
    [TestFixture]
    public class DocSplitterTester
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

        //TAX*174-27734961***********3~FOB*DF*OR~N1*SE**92*0013000188~N1*BT*Solectron Texas, L.P.*92*12T1~N4*EL PASO*TX*79998-1366*US~N1*ST**92*W108~PO1*00010*768*EA*17.96000*PE*BP*100000021961*EC*00*VP*FIN191019SOLPK0*PN*PK99109250001~MSG*RoHS5/6~SCH*768.000*EA***002*20070130~PKG*F****Bulk packaging~CTT*1~
        [Test]
        public void can_split_file_contents()
        {
            _lst.Add(new Segment
                         {
                             Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~",
                             Label = SegmentLabel.GroupLabel
                         });
            _lst.Add(new Segment {Contents = "ST*850*0001~", Label = SegmentLabel.DocumentLabel});
            _lst.Add(new Segment { Contents = "BEG*00*SA*4110076497**20071031~CUR*BY*USD~", Label = "BEG".GetSegmentLabel() });
            _lst.Add(new Segment { Contents = "PER*BD*Brenda Fritz*TE*512-425-6314~", Label = "PER".GetSegmentLabel() });
            _lst.Add(new Segment {Contents = "SE*16*0001~", Label = SegmentLabel.DocumentClose});
            _lst.Add(new Segment {Contents = "ST*850*0001~", Label = SegmentLabel.DocumentLabel});
            _lst.Add(new Segment { Contents = "BEG*00*SA*4110076497**20071031~CUR*BY*USD~", Label = "BEG".GetSegmentLabel() });
            _lst.Add(new Segment { Contents = "PER*BD*Brenda Fritz*TE*512-425-6314~", Label = "PER".GetSegmentLabel() });
            _lst.Add(new Segment {Contents = "SE*16*0001~", Label = SegmentLabel.DocumentClose});
            _lst.Add(new Segment {Contents = "ST*850*0001~", Label = SegmentLabel.DocumentLabel});
            _lst.Add(new Segment { Contents = "BEG*00*SA*4110076497**20071031~CUR*BY*USD~", Label = "BEG".GetSegmentLabel() });
            _lst.Add(new Segment { Contents = "PER*BD*Brenda Fritz*TE*512-425-6314~", Label = "PER".GetSegmentLabel() });
            _lst.Add(new Segment {Contents = "SE*16*0001~", Label = SegmentLabel.DocumentClose});
            _lst.Add(new Segment {Contents = "ST*850*0001~", Label = SegmentLabel.DocumentLabel});
            _lst.Add(new Segment { Contents = "BEG*00*SA*4110076497**20071031~CUR*BY*USD~", Label = "BEG".GetSegmentLabel() });
            _lst.Add(new Segment { Contents = "PER*BD*Brenda Fritz*TE*512-425-6314~", Label = "PER".GetSegmentLabel() });
            _lst.Add(new Segment {Contents = "SE*16*0001~", Label = SegmentLabel.DocumentClose});
            _lst.Add(new Segment {Contents = "GE*1*7~", Label = SegmentLabel.GroupClose});
            var segs = new EdiSegmentCollection(_lst, "*");
            var result = _sut.SplitByDocument(segs, new GroupContainer(segs));
            Assert.That(result.Count() == 4);
        }

        [Test, ExpectedException]
        public void will_fail_with_badly_formed_file_contents()
        {
            _lst.Add(new Segment
                         {
                             Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~",
                             Label = SegmentLabel.GroupLabel
                         });
            _lst.Add(new Segment {Contents = "ST*850*0001~", Label = SegmentLabel.DocumentLabel});
            _lst.Add(new Segment { Contents = "BEG*00*SA*4110076497**20071031~CUR*BY*USD~", Label = "BEG".GetSegmentLabel() });
            _lst.Add(new Segment { Contents = "PER*BD*Brenda Fritz*TE*512-425-6314~", Label = "PER".GetSegmentLabel() });
            _lst.Add(new Segment {Contents = "SE*16*0001~", Label = SegmentLabel.DocumentClose});
            _lst.Add(new Segment {Contents = "ST*850*0001~", Label = SegmentLabel.DocumentLabel});
            _lst.Add(new Segment { Contents = "BEG*00*SA*4110076497**20071031~CUR*BY*USD~", Label = "BEG".GetSegmentLabel() });
            _lst.Add(new Segment { Contents = "PER*BD*Brenda Fritz*TE*512-425-6314~", Label = "PER".GetSegmentLabel() });
            _lst.Add(new Segment {Contents = "SE*16*0001~", Label = SegmentLabel.DocumentClose});
            _lst.Add(new Segment {Contents = "ST*850*0001~", Label = SegmentLabel.DocumentLabel});
            _lst.Add(new Segment { Contents = "BEG*00*SA*4110076497**20071031~CUR*BY*USD~", Label = "BEG".GetSegmentLabel() });
            _lst.Add(new Segment { Contents = "PER*BD*Brenda Fritz*TE*512-425-6314~", Label = "PER".GetSegmentLabel() });
            _lst.Add(new Segment {Contents = "GE*1*7~", Label = SegmentLabel.GroupClose});

            var segs = new EdiSegmentCollection(_lst, "*");
            _sut.SplitByDocument(segs, new GroupContainer(segs));
        }
    }
}