

namespace EDIDocsProcessing.Tests.UnitTests.Core.DocsIn
{
    using System.Linq;
    using Common;
    using EDIDocsProcessing.Core.DocsIn.impl;
    using NUnit.Framework;
    [TestFixture]
    public class Generic997ParserTester
    {
        private Generic997Parser _sut;
        [TestFixtureSetUp]
        public void SetUpForAllTests()
        {
            _sut = new Generic997Parser();
        }
        [SetUp]
        public void SetUpForEachTest()
        {

        }

        [Test]
        public void can_parse_997()
        {
            var splitter = new SegmentSplitter();
            var txt = "ISA*00*          *00*          *ZZ*SLRSCOREGTWY   *12*EEC5122516063  *120223*1057*U*00401*000000157*1*P*|~GS*PO*SLR010*EEC5122516063*20120223*1057*90*X*004010~ST*997*23432~AK1*SS*23432~AK2*810*30405~AK2*810*30508~AK3*REF*25*2**~AK3*DTM*20120203~AK4*2*373*8*A02343~SE*2*23432~GE*1*90~IEA*1*000000157|";
            var segs = splitter.Split(txt);
            var segList = segs.SegmentList.Skip(2).ToList();
            var acks = _sut.ProcessSegmentList(segList);
            acks.Acks.Count.ShouldEqual(2);
            var ack = acks.Acks.First();
            acks.ControlNumber.ShouldEqual("23432");
            ack.DocumentId.ShouldEqual(810);
            ack.ControlNumber.ShouldEqual("30405");
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
