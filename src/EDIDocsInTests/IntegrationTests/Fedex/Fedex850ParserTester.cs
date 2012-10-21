using System.Linq;
using EDIDocsIn.config;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core.DocsIn;
using EdiMessages;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests.Initech
{
    [TestFixture]
    public class Initech850ParserTester
    {
        private const string C850 = "ISA~00~          ~00~          ~ZZ~055001924VA    ~12~EEC5122516063  ~090810~1551~U~00401~000000009~0~T~^GS~PO~055001924VA~EEC5122516063~20090810~1551~9~X~004010ST~850~90001BEG~00~NE~SRC009677~~20090714REF~I5~ORREF~19~20001REF~ZZ~CAD32REF~XE~1PER~DC~MARY PEEK~TE~901-263-6584N1~ST~Initech SUSIE CARNEYN3~90 Initech PARKWAYN4~COLLIERVILLE~TN~38017~USPO1~1~1~EA~1~~IN~900104~~~PD~BOX, MONITOR FLAT PANEL 17\"~TP~NREF~CO~249040291943404REF~BF~249040REF~P4~9002087SCH~1~EA~~~002~20090810SE~16~90001ST~850~90002BEG~00~NE~SRC009678~~20090714REF~I5~ORREF~19~20001REF~ZZ~CAD32REF~XE~1PER~DC~MARY PEEK~TE~901-263-6584N1~ST~Initech SUSIE CARNEYN3~90 Initech PARKWAYN4~COLLIERVILLE~TN~38017~USPO1~1~1~EA~1~~IN~900104~~~PD~BOX, MONITOR FLAT PANEL 17\"~TP~NREF~CO~249040291943403REF~BF~249040REF~P4~9002087SCH~1~EA~~~002~20090806SE~16~90002GE~2~9IEA~1~000000009";
        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();
        }

        [Test]
        public void can_consume_850()
        {
            var splitter = ServiceLocator.Current.GetInstance<ISegmentSplitter>();
            var segs = splitter.Split(C850);
            var parser = BusinessPartner.Initech.Parser();
            var lst = parser.Parse(segs);
            var docsRepo = ServiceLocator.Current.GetInstance<IIncomingDocumentsRepository>();
            var originalDoc = docsRepo.GetByDocumentControlNumberAndPartnerID(90001, BusinessPartner.Initech.Number); 
            Assert.IsNotNull(originalDoc);
            originalDoc.LineItems[0].ResponseElements.Count.ShouldEqual(4);
            originalDoc = docsRepo.GetByDocumentControlNumberAndPartnerID(90002, BusinessPartner.Initech.Number);
            Assert.IsNotNull(originalDoc);
            originalDoc.LineItems[0].ResponseElements.Count.ShouldEqual(4); 
        }

        [Test]
        public void can_consume_international_850()
        {
            var intlC850 = "ISA~00~          ~00~          ~ZZ~055001924VA    ~12~EEC5122516063  ~100804~1250~U~00401~000000498~0~P~^GS~PO~055001924VA~EEC5122516063~20100804~1250~481~X~004010ST~850~4810001BEG~00~NE~AFP000634~~20100804REF~I5~ORREF~19~20001REF~ZZ~CAD32REF~XE~1PER~DC~JOHN ISAAC~TE~4162979544N1~ST~FED EXN3~80 NUGGET AVENUEN4~SCARBOROUGH~ON~M1S 3A7~CAPO1~1~6~EA~13.84~~IN~910008~VN~910008~PD~Box, HP 2035 Laser Printer~TP~NREF~CO~103010202165093REF~BF~001000REF~P4~9002087SCH~6~EA~~~002~20100804SE~16~4810001GE~1~481IEA~1~000000498";

            var splitter = ServiceLocator.Current.GetInstance<ISegmentSplitter>();
            var segs = splitter.Split(intlC850);
            var parser = BusinessPartner.Initech.Parser();
            var lst = parser.Parse(segs);
            var ord = (OrderRequestReceivedMessage)lst.First();
            var addr = ord.ShipToAddress;
            addr.Country.ShouldEqual("CA");
        }
    }
}
