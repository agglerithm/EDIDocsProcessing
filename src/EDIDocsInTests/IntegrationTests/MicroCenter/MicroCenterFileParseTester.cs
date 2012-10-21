using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests.MicroCenter
{
    using EDIDocsIn.config;
    using EDIDocsProcessing.Common;
    using EDIDocsProcessing.Common.Extensions;
    using Microsoft.Practices.ServiceLocation;

    [TestFixture]
    public class MicroCenterFileParseTester
    {
        private IFileParser _sut;

        [TestFixtureSetUp]
        public void SetUpForAllTests()
        {

        }
        [SetUp]
        public void SetUpForEachTest()
        {
            StructureMapBootstrapper.Execute();
            _sut = BusinessPartner.MicroCenter.Parser();

        }

        [Test]
        public void can_read_complete_edi_file()
        {
            //Arrange
            var text = @"ISA*00*          *00*          *ZZ*SLRSCOREGTWY   *12*EEC5122516063  *120223*1057*U*00401*000000157*1*P*|~GS*PO*SLR010*EEC5122516063*20120223*1057*90*X*004010~ST*850*1111~BEG*00*SA*PO2343**20120322~REF*IA*23433~PER*IC*Billy Madison*TE*512-555-2343~CSH*N~DTM*001*20120313~DTM*002*20120313~DTM*010*20120313~TD5*****Ship it in there~N9*L1**Shipping instructions
                           ~MSG*Ship it now~N1*ST*Micro Center*92*001~N3*123 Fake St*Suite A~N4*Springfield*MO*21323~PO1*1*200*EA*2.12**VP*FIN12343MC001*BP*12343MC*UP*1243234~CTP**MSR*2.12~PID*F****Cubic zirconium box~PO1*2*200*EA*0.12**VP*FIN12345MC001*BP*12345MC*UP*1243234~CTP**MSR*0.12~PID*F****bag~CTT*2~SE*22*1111~GE*1*90~IEA*1*000000157~";

            //Act

            var splitter = ServiceLocator.Current.GetInstance<ISegmentSplitter>();
            var segs = splitter.Split(text);
            var msgs = _sut.Parse(segs);

            //Assert

            msgs.Count().ShouldEqual(1);

            var msg = msgs.First();

            msg.ControlNumber.ShouldEqual("1111");

        }

        [Test]
        public void can_read_edi_file_minus_optional_segments()
        {
            //Arrange
            var text = @"ISA*00*          *00*          *ZZ*SLRSCOREGTWY   *12*EEC5122516063  *120223*1057*U*00401*000000157*1*P*|~GS*PO*SLR010*EEC5122516063*20120223*1057*90*X*004010~ST*850*1111~BEG*00*SA*PO2343**20120322~REF*IA*23433~DTM*001*20120313~DTM*002*20120313~TD5*****Ship it in there~N9*L1**Shipping instructions~MSG*Ship it now~N1*ST*Micro Center*92*001~N3*123 Fake St*Suite A~N4*Springfield*MO*21323~PO1*1*200*EA*2.12**VP*FIN12343MC001*BP*12343MC*UP*1243234~PID*F****Cubic zirconium box~PO1*2*200*EA*0.12**VP*FIN12345MC001*BP*12345MC*UP*1243234~PID*F****bag~CTT*2~SE*17*1111~GE*1*90~IEA*1*000000157~";

            //Act
            var splitter = ServiceLocator.Current.GetInstance<ISegmentSplitter>();
            var segs = splitter.Split(text);
            var msgs = _sut.Parse(segs);

            //Assert
            msgs.Count().ShouldEqual(1);

            var msg = msgs.First();

            msg.ControlNumber.ShouldEqual("1111");

        }

        [Test]
        public void can_read_edi_860_file()
        {
            //Arrange
            var text = @"ISA*00*          *00*          *ZZ*TSTMICROCTRNB  *12*EEC5122516063  *120413*0812*U*00401*000001003*0*T*}~GS*PC*TSTMICROCTRNB*EEC5122516063*20120413*0812*1003*X*004010~ST*860*10030001~BCH*04*CP*193845***20120409*****20120413~PER*IC*ContactName*TE*6125551212~CSH*Y~DTM*001*20091225~DTM*002*20091215~DTM*010*20091201~TD5*****Roadway~N9*L1**PO Notes~MSG*Purchase Order Notes~POC*1*DI*10*0*EA*0.36**VP*DCB090701MEIPL0*BP*000277277*UP*111111111111~CTP**MSR*19.99~PID*F****9x7.625x1.75 32ect B~CTT*1~SE*15*10030001~GE*1*1003~IEA*1*000001003~";

            //Act
            var splitter = ServiceLocator.Current.GetInstance<ISegmentSplitter>();
            var segs = splitter.Split(text);
            var msgs = _sut.Parse(segs);

            //Assert
            msgs.Count().ShouldEqual(1);

            var msg = msgs.First();

            msg.ControlNumber.ShouldEqual("10030001");
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
