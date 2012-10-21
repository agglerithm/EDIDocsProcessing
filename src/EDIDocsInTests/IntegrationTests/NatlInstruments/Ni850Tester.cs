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
    public class Ni850Tester
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
        public void can_read_850()
        {
            var txt =
                @"ISA>00>          >00>          >01>088005265      >12>EEC5122516063  >091222>2100>U>00200>000000141>0>P>|~GS>PO>088005265>EEC5122516063>20091222>2100>141>X>004010~ST>850>1410001~BEG>00>SA>351945>>20091222~CUR>BY>USD~FOB>CC>DE~ITD>>>>>>>>>>>>2/10 Net 30~N9>PO>351945~MSG>ordered per Dean Arnold~N1>SU>Austin Foam Plastics Inc>92>28449~N3>2933 A W Grimes Blvd~N4>Pflugerville>TX>78660-5292~N1>ST>Manufacturing facility at MoPac>92>NI-MoPac~N3>11500 N. MoPac Expressway~N4>Austin>TX>78759~N1>BT>National Instruments billing address>92>NIC Billing~N3>ATTENTION: Accounts Payable>11500 N. MoPac Expressway~N3>Bldg. A~N4>Austin>TX>78759~N1>BY>National Instruments>92>NI-MoPac~PER>BD>Gerard Netek~PO1>1>1>EA>758.4~PID>F>>>>800767A-01  CONTAINER, REGULAR SLOTTED, ID 46.5L X 30.5W X 25D, KRAFT~SCH>1>EA>>>002>20091231~AMT>1>758.4~CTT>1~SE>25>1410001~GE>1>141~IEA>1>000000141~";


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
