using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDIDocsIn.config;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.Extensions;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests.NatlInstruments
{
    using Microsoft.Practices.ServiceLocation;

    [TestFixture]
    public class Ni860Tester
    {
 
        private IFileParser _sut;

        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();
            _sut = BusinessPartner.NationalInstruments.Parser(); 

        }
        [SetUp]
        public void SetUpForEachTest()
        {

        }

        [Test]
        public void can_read_860()
        {
            var txt =
                @"ISA>00>          >00>          >01>088005265      >12>EEC5122516063  >091016>1401>U>00200>000000133>0>P>|~GS>PC>088005265>EEC5122516063>20091016>1401>133>X>004010~ST>860>1330001~BCH>05>SA>349592>>2>20091016~CUR>BY>USD~FOB>CC>DE~ITD>>>>>>>>>>>>2/10 Net 30~N1>SU>Austin Foam Plastics Inc>92>28449~N3>2933 A W Grimes Blvd~N4>Pflugerville>TX>78660-5292~PER>AG>Jimie Salas>TE>512 251 6063x266~N1>ST>Manufacturing facility at MoPac>92>NI-MoPac~N3>11500 N. MoPac Expressway~N4>Austin>TX>78759~N1>BT>National Instruments billing address~N3>ATTENTION: Accounts Payable>11500 N. MoPac Expressway~N3>Bldg. A~N4>Austin>TX>78759~N1>BY>National Instruments>92>NI-MoPac~PER>BD>Gerard Netek~POC>1>CA>100>100>EA>4.06>>EC>1>BP>800624A-01~PID>F>>>>FOAM, CUT, POLYURETHANE~SCH>100>EA>>>002>20091022~AMT>1>406~CTT>1~SE>24>1330001~GE>1>133~IEA>1>000000133~";

            var splitter = ServiceLocator.Current.GetInstance<ISegmentSplitter>();
            var segs = splitter.Split(txt);
            var customerOrders = _sut.Parse(segs);


            customerOrders.Count().ShouldEqual(1);
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
