using System.Collections.Generic;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsIn.impl;
using EdiMessages;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests
{
    [TestFixture]
    public class PoLineParserTester
    { 
        private IPOLineParser _sut;

        [TestFixtureSetUp]
        public void SetUp()
        { 
            _sut = new POLineParser(); 

        }

        [Test]
        public void can_extract_item_ids()
        {
 
            var line = _sut.CreateLine(new Segment
                                          {
                                              Contents = "PO1*1*500*EA*6.75**IN*CAD33*VN*FINCAD33*PD*BIG BOX*TP*S",
                                              Label = "PO1".GetSegmentLabel()
                                          } );
            Assert.That(line.ItemId == "FINCAD33");
            Assert.That(line.CustomerPartNumber == "CAD33");
            Assert.That(line.ItemDescription == "BIG BOX");
        }

        [Test]
        public void can_process_lines()
        {
 
            var line1 = _sut.CreateLine(new Segment
                                             {
                                                 Contents = "PO1*1*500*EA*6.75**IN*CAD33*VN*FINCAD33*PD*BIG BOX*TP*S",
                                                 Label = "PO1".GetSegmentLabel()
                                             });
            var line2 = _sut.CreateLine(new Segment
            {
                Contents = "PO1*3*15*EA*1.75**IN*SCAD33*VN*FINSCAD33*PD*FOAM THINGY*TP*S",
                Label = "PO1".GetSegmentLabel()
            });
            var line3 = _sut.CreateLine(new Segment
            {
                Contents = "PO1*3*15*EA*1.75**IN*SCAD33*VN*FINSCAD33*PD*LITTLE BOX*TP*S",
                Label = "PO1".GetSegmentLabel()
            });

            Assert.That(line1.ItemDescription == "BIG BOX");
            Assert.That(line2.ItemDescription == "FOAM THINGY");
            Assert.That(line3.ItemDescription == "LITTLE BOX");
        }
    }
}