using System;
using System.Collections.Generic;
using System.Linq;
using AFPST.Common;
using AFPST.Common.IO;
using AFPST.Common.Services;
using EDIDocsIn.config;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.Extensions;
using EdiMessages;
using InitechEDIDocsIn;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests.Initech
{
    [TestFixture]
    public class InitechFileParseServiceTester
    {
        private IFileParser _sut;

        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();
            _sut = BusinessPartner.Initech.Parser();
        }

        private static string get_contents_from_file(string path)
        {
            var fu = ServiceLocator.Current.GetInstance<IFileUtilities>();
            IList<FileEntity> list = fu.GetListFromFolder(path, "*.*", DateTime.Today.AddYears(-30));
            if (list.Count() == 0) return "";
            string filePath = list[0].FullPath;
            return Utilities.QuickFileText(filePath);
        }

        [Test]
        public void can_split_file()
        {
            string contents = get_contents_from_file(@"\\AUTOMATION\d$\Download\Initech\Test\");

            if (string.IsNullOrEmpty(contents)) return;

            var splitter = ServiceLocator.Current.GetInstance<ISegmentSplitter>();
            var segs = splitter.Split(contents);
            IEnumerable<OrderRequestReceivedMessage> customerOrders =   _sut.Parse(segs).Select(m => (OrderRequestReceivedMessage)m);


            customerOrders.Count().ShouldEqual(1);
        }

        [Test]
        public void can_split_string()
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

            var splitter = ServiceLocator.Current.GetInstance<ISegmentSplitter>();
            var segs = splitter.Split(contents);
            var customerOrders =   _sut.Parse(segs);


            customerOrders.Count().ShouldEqual(1);
        }
    }
}