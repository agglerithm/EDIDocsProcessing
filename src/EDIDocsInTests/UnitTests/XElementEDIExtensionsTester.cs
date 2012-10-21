using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;
using InitechEDIDocsOut;
using Moq;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests
{
    using global::EdiMessages;

    [TestFixture]
    public class XElementEDIExtensionsTester
    {
        private SegmentFactory _seg;
        private Mock<IBusinessPartnerSpecificServiceResolver> _resolver;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _resolver = new Mock<IBusinessPartnerSpecificServiceResolver>();
            _resolver.Setup(r => r.GetBuildValueFactoryFor(BusinessPartner.Initech)).Returns(new InitechBuildValueFactory());
            _seg = new SegmentFactory(_resolver.Object);
        }

        [Test]
        public void can_get_correct_edi_value()
        {
            var ediXmlBuildValues = new EdiXmlBuildValues {ElementDelimiter = "~", SegmentDelimiter = "\n"};

            var seg = new EDIXmlSegment("ST", ediXmlBuildValues) {Value = "hello"};
            var el = new EDIXmlElement("ST01", "hello", ediXmlBuildValues);
            Assert.That(seg.EDIValue() == "");
            Assert.That(el.EDIValue() == "hello");
        }

        [Test]
        public void can_get_edi_descendant_segment()
        {
            var x = new XElement(EdiStructureNameConstants.Loop);
            x.Add(new XElement("ediSegment", new XAttribute(EdiStructureNameConstants.Label, "N1")));
            XElement f = x.EDIDescendantSegment("N1");
            Assert.That(f != null);
        }

        [Test]
        public void can_get_edi_element()
        {
            var x = new XElement(EdiStructureNameConstants.Segment);
            x.Add(new XElement(EdiStructureNameConstants.Element, new XAttribute(EdiStructureNameConstants.Label, "N101")));
            x.Add(new XElement(EdiStructureNameConstants.Element, new XAttribute(EdiStructureNameConstants.Label, "N102")));
            x.Add(new XElement(EdiStructureNameConstants.Element, new XAttribute(EdiStructureNameConstants.Label, "N103")));
            XElement f = x.EDIElement("N102");
            Assert.That(f != null);
        }

        [Test]
        public void can_get_edi_elements()
        {
            var x = new XElement(EdiStructureNameConstants.Segment);
            x.Add(new XElement(EdiStructureNameConstants.Element, new XAttribute(EdiStructureNameConstants.Label, "N101")));
            x.Add(new XElement(EdiStructureNameConstants.Element, new XAttribute(EdiStructureNameConstants.Label, "N102")));
            x.Add(new XElement(EdiStructureNameConstants.Element, new XAttribute(EdiStructureNameConstants.Label, "N103")));
            List<XElement> f = x.EDIElements().ToList();
            Assert.That(f.Count == 3);
        }

        [Test]
        public void can_get_edi_loop()
        {
            var x = new XElement("ediTransaction");
            x.Add(new XElement(EdiStructureNameConstants.Loop, new XAttribute(EdiStructureNameConstants.Label, "N1")));
            x.Add(new XElement(EdiStructureNameConstants.Loop, new XAttribute(EdiStructureNameConstants.Label, "PO1")));
            x.Add(new XElement(EdiStructureNameConstants.Loop, new XAttribute(EdiStructureNameConstants.Label, "CTT")));
            XElement f = x.EDILoop("PO1");
            Assert.That(f != null);
        }

        [Test]
        public void can_get_edi_loops()
        {
            var x = new XElement("ediTransaction");
            x.Add(new XElement(EdiStructureNameConstants.Loop, new XAttribute(EdiStructureNameConstants.Label, "N1")));
            x.Add(new XElement(EdiStructureNameConstants.Loop, new XAttribute(EdiStructureNameConstants.Label, "PO1")));
            x.Add(new XElement(EdiStructureNameConstants.Loop, new XAttribute(EdiStructureNameConstants.Label, "CTT")));
            List<XElement> f = x.EDILoops().ToList();
            Assert.That(f.Count == 3);

            f = x.EDILoops("PO1").ToList();
            Assert.That(f.Count == 1);
        }

        [Test]
        public void can_get_edi_segment()
        {
            var x = new XElement(EdiStructureNameConstants.Loop);
            x.Add(new XElement(EdiStructureNameConstants.Segment, new XAttribute(EdiStructureNameConstants.Label, "N1")));
            x.Add(new XElement(EdiStructureNameConstants.Segment, new XAttribute(EdiStructureNameConstants.Label, "N3")));
            x.Add(new XElement(EdiStructureNameConstants.Segment, new XAttribute(EdiStructureNameConstants.Label, "N4")));
            XElement f = x.EDISegment("N3");
            Assert.That(f != null);
        }

        [Test]
        public void can_get_edi_segments()
        {
            var x = new XElement(EdiStructureNameConstants.Loop);
            x.Add(new XElement(EdiStructureNameConstants.Segment, new XAttribute(EdiStructureNameConstants.Label, "N1")));
            x.Add(new XElement(EdiStructureNameConstants.Segment, new XAttribute(EdiStructureNameConstants.Label, "N3")));
            x.Add(new XElement(EdiStructureNameConstants.Segment, new XAttribute(EdiStructureNameConstants.Label, "N4")));
            List<XElement> f = x.EDISegments().ToList();
           f.Count.ShouldEqual(3);
        }

        [Test]
        public void can_get_envelopes()
        {
            var ediXmlBuildValues = new EdiXmlBuildValues {ElementDelimiter = "~", SegmentDelimiter = "\n"};

            var root = new XElement("file");
            var ic = new EDIXmlInterchangeControl(_seg);
            var grp = new EDIXmlFunctionGroup(_seg);
            grp.AddTransactionSet(new EDIXmlTransactionSet(_seg));
            grp.AddTransactionSet(new EDIXmlTransactionSet(_seg));
            ic.AddFunctionGroup(grp);
            root.Add(ic);
            IEnumerable<XElement> ic_s = root.EDIInterchangeControls();
            foreach (EDIXmlInterchangeControl i in ic_s)
            {
                IEnumerable<XElement> grp_s = i.EDIFunctionGroups();
                foreach (EDIXmlFunctionGroup g in grp_s)
                {
                    List<XElement> ts_s = g.EDITransactions().ToList();
                    foreach (EDIXmlTransactionSet t in ts_s)
                        Assert.That(t.Label == "ST");
                }
            }
            //Assert.That(root.EDINodes("ISA").ToList().Count == 1);
        }
    }
}