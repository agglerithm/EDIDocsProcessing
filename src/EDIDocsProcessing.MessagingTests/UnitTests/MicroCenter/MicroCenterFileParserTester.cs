namespace EDIDocsProcessing.Tests.UnitTests.MicroCenter
{
    using System.Collections.Generic;
    using EDIDocsProcessing.Common;
    using EDIDocsProcessing.Common.EDIStructures;
    using EDIDocsProcessing.Core.DocsIn;
    using FedexEDIDocsIn;
    using global::EdiMessages;
    using MicroCenterEDIDocsIn;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class MicroCenterFileParserTester
    {
        private IFileParser _sut;
        private Mock<IDocumentParser> _documentParser;
        private Mock<ISegmentSplitter> _splitter;
        private Mock<IIncomingDocumentsRepository> _docsRepo;
        private Mock<IBusinessPartnerSpecificServiceResolver> _resolver;
        private Mock<IEdiFileReader> _reader;
        private Mock<IMessageGenerator> _generator;
        

        [TestFixtureSetUp]
        public void SetUp()
        {
            _splitter = new Mock<ISegmentSplitter>();
            _documentParser = new Mock<IDocumentParser>();
            _docsRepo = new Mock<IIncomingDocumentsRepository>();
            _resolver = new Mock<IBusinessPartnerSpecificServiceResolver>();
            _reader = new Mock<IEdiFileReader>();
            _generator = new Mock<IMessageGenerator>();
            _reader.Setup(r => r.Read(It.IsAny<EdiSegmentCollection>())).Returns(new EdiFileInfo(null));
            _sut = new MicroCenterFileParser(_splitter.Object, _reader.Object, _generator.Object);
        }

        [Test]
        public void can_split_file()
        {
            string contents = @"ISA*00*          *00*          *ZZ*SPSMICROELEC   *12*EEC5122516063  *120626*1504*U*00401*100000001*0*P*}~GS*PO*SPSMICROELEC*EEC5122516063*20120626*1504*1001*X*004010~ST*850*1001~BEG*00*SA*25151**20120625~REF*IA*25097~PER*IC*MICHELE WALTERS*TE*000000000000000~CSH*N~DTM*010*20120701~DTM*002*20120709~DTM*001*20120725~N9*L1**PO Notes~MSG*DON CHUTES 614-527-7853~MSG*DONCHUTES@AUSTINFOAM.COM~MSG*CELL - 614 204 3179~MSG*EDI~N1*ST*MEI/MICRO CENTER, INC.*92*033~N3*#033 IPSG FINISHED PRODUCTS*2701 CHARTER ST.  SUITE A~N4*COLUMBUS*OH*43228~PO1*001*1500*EA*2.03**VP*UNIV OUTER BOX*BP*0000060434~CTP**MSR*2.09~PID*F****POWERSPEC UNIV OUTER BOX~PO1*002*1500*EA*0.36**VP*UNIV ACCES BOX*BP*0000060467~CTP**MSR*0.37~PID*F****POWERSPEC UNIV ACCES BOX~PO1*003*1500*EA*4.12**VP*UNIV FOAM SET*BP*0000060483~CTP**MSR*4.24~PID*F****POWERSPEC UNIV FOAM SET~CTT*3~SE*27*1001~GE*1*1001~IEA*1*100000001~";

            var order = new DocumentRecordPackage(new OrderRequestReceivedMessage(),null,null);
            var splitter = new SegmentSplitter();
            var isa_lst_lst = new List<InterchangeContainer>();
 

            _documentParser.Setup(p => p.ProcessSegmentList(It.IsAny<List<Segment>>())).Returns(order);
             
            var segs = splitter.Split(contents);
            _sut.Parse(segs);


            _splitter.VerifyAll();
        }
    }
}