using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests.Common
{
    [TestFixture]
    public class EdiFileReaderTester
    {
        private IEdiFileReader _fileReader;
        private ISegmentSplitter _segSplitter;

        [TestFixtureSetUp]
        public void SetUpForAllTests()
        {
            _fileReader = new EdiFileReader(new HierarchySplitter());
            _segSplitter = new SegmentSplitter();
        }
        [SetUp]
        public void SetUpForEachTest()
        {

             
        }

        [Test]
        public void can_read_file()
        {
            string contents = @"ISA~00~          ~00~          ~ZZ~InitechCADS      ~01~1109518        ~"
              + @"050620~1031~U~00401~000064170~0~P~^\"
              + @"GS~PO~CADS~1109518C~20050620~1031~55323~X~004010\ST~850~553230001\"
              + @"BEG~00~NE~C04134707~~20050620\"
              + @"REF~I5~OR\REF~19~001\REF~ZZ~CAD33\REF~XE~5\"
              + @"PER~DC~JENNIFER WILLIAMS\"
              + @"N1~ST~EPS SETTLEMENTS GROUP~92~SUSA\N3~12825 FLUSHING MEADOWS DR FL 2~STE 280\"
              + @"N4~SAINT LOUIS~MO~63131~US\"
              +
              @"PO1~1~4~PK~0~~IN~146525~VN~146525~PD~LABEL, Initech SHIP F223 100-PK, 8PK-BX    112BX-P~TP~U\"
              + @"REF~CO~256070251719847\REF~BF~256070\"
              + @"SCH~4~PK~~~002~20050620\"
              + @"PO1~2~1~EA~6.75~~IN~CAD33~VN~CAD33~PD~FEE, SERVICE FOR CUSTOMER SUPPLY ORDER~TP~S\"
              + @"REF~CO~256070251719847\REF~BF~256070\"
              + @"SCH~1~EA~~~002~20050620\SE~19~553230001\GE~1~55323\IEA~1~000064170~";
            var segs = _segSplitter.Split(contents);
            var info = _fileReader.Read(segs);

            info.SenderId.ShouldEqual("InitechCADS");
            info.Documents.Count().ShouldEqual(1);
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
