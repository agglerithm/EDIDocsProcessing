using System.Collections.Generic;
using EDIDocsProcessing.Common; 
using EDIDocsProcessing.Common.Infrastructure;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsIn.impl;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests
{
    [TestFixture]
    public class GroupSplitterTester
    {
        private List<Segment> _lst;
        private ISplitter _sut;

        [SetUp]
        public void SetUp()
        {
            Container.Reset();
            _lst = new List<Segment>();
            Container.Register<ISplitter, Splitter>();
            _sut = Container.Resolve<ISplitter>();
        }

        [Test]
        public void can_split_file_contents()
        {
            _lst.Add(new Segment { Contents = "ISA*00*          *00*          *ZZ*SLRSCOREGTWY   *12*EEC5122516063  *071031*0925*U*00401*000000007*0*P*~~", Label = EDIConstants.InterchangeLabel });
            _lst.Add(new Segment { Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~", Label = EDIConstants.GroupLabel });
            _lst.Add(new Segment { Contents = "GE*1*7~", Label = EDIConstants.GroupClose });
            _lst.Add(new Segment { Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~", Label = EDIConstants.GroupLabel });
            _lst.Add(new Segment { Contents = "GE*1*7~", Label = EDIConstants.GroupClose });
            _lst.Add(new Segment { Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~", Label = EDIConstants.GroupLabel });
            _lst.Add(new Segment { Contents = "GE*1*7~", Label = EDIConstants.GroupClose });
            _lst.Add(new Segment { Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~", Label = EDIConstants.GroupLabel });
            _lst.Add(new Segment { Contents = "GE*1*7~", Label = EDIConstants.GroupClose });
            _lst.Add(new Segment { Contents = "IEA*1*000000010~", Label = EDIConstants.InterchangeClose });
            var result = _sut.SplitByGroup(_lst);
            Assert.That(result.Count == 4);
        }

        [Test, ExpectedException]
        public void will_fail_with_badly_formed_file_contents()
        {
            _lst.Add(new Segment { Contents = "ISA*00*          *00*          *ZZ*SLRSCOREGTWY   *12*EEC5122516063  *071031*0925*U*00401*000000007*0*P*~~", Label = EDIConstants.InterchangeLabel });
            _lst.Add(new Segment { Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~", Label = EDIConstants.GroupLabel });
            _lst.Add(new Segment { Contents = "GE*1*7~", Label = EDIConstants.GroupClose });
            _lst.Add(new Segment { Contents = "IEA*1*000000007~", Label = EDIConstants.InterchangeClose });
            _lst.Add(new Segment { Contents = "ISA*00*          *00*          *ZZ*SLRSCOREGTWY   *12*EEC5122516063  *071031*0925*U*00401*000000008*0*P*~~", Label = EDIConstants.InterchangeLabel });
            _lst.Add(new Segment { Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~", Label = EDIConstants.GroupLabel });
            _lst.Add(new Segment { Contents = "GE*1*7~", Label = EDIConstants.GroupClose });
            _lst.Add(new Segment { Contents = "IEA*1*000000008~", Label = EDIConstants.InterchangeClose });
            _lst.Add(new Segment { Contents = "ISA*00*          *00*          *ZZ*SLRSCOREGTWY   *12*EEC5122516063  *071031*0925*U*00401*000000009*0*P*~~", Label = EDIConstants.InterchangeLabel });
            _lst.Add(new Segment { Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~", Label = EDIConstants.GroupLabel });
            _lst.Add(new Segment { Contents = "GE*1*7~", Label = EDIConstants.GroupClose });
            _lst.Add(new Segment { Contents = "IEA*1*000000009~", Label = EDIConstants.InterchangeClose });
            _lst.Add(new Segment { Contents = "ISA*00*          *00*          *ZZ*SLRSCOREGTWY   *12*EEC5122516063  *071031*0925*U*00401*000000010*0*P*~~", Label = EDIConstants.InterchangeLabel });
            _lst.Add(new Segment { Contents = "GS*PO*SLR010*EEC5122516063*20071031*0925*7*X*004010~", Label = EDIConstants.GroupLabel });
            _lst.Add(new Segment { Contents = "IEA*1*000000010~", Label = EDIConstants.InterchangeClose });
            _sut.SplitByGroup(_lst);
        }
        [TestFixtureTearDown]
        public void TearDown()
        {
            Container.Reset();
        }
    }
}