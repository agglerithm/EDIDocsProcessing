using System.Collections.Generic;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsIn;
using EdiMessages;
using FedexEDIDocsIn;
using FlexEDIDocsIn;
using Moq;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.Flex
{
    [TestFixture]
    public class FlexLineParserTester
    {
        private Flex850LineParser _sut;
        private Mock<IEDIResponseReferenceRecorder> _recorder;
        private Mock<IPOLineParser> _lineParser;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _lineParser = new Mock<IPOLineParser>();
            _lineParser.Setup(p => p.CreateLine(It.IsAny<Segment>())).Returns(new CustomerOrderLine(){});
            _sut = new Flex850LineParser(_lineParser.Object);
            _recorder = new Mock<IEDIResponseReferenceRecorder>(); 
            
        }

        [Test]
        public void can_extract_item_ids()
        {
            var segmentList = new List<Segment>
                                  {
                                      new Segment {Contents = "ST*850*0001", Label = SegmentLabel.DocumentLabel},
                                      new Segment
                                          {Contents = "BEG*00*SA*4110076497**20071031*CUR*BY*USD", Label = "BEG".GetSegmentLabel()},
                                      new Segment
                                          {
                                              Contents = "PO1*1*500*EA*6.75**IN*CAD33*VN*FINCAD33*PD*BIG BOX*TP*S",
                                              Label = "PO1".GetSegmentLabel()
                                          },
                                          new Segment
                                              {
                                                  Contents = 
                                                  "REF~CO~249040291943406",
                                                  Label = "REF".GetSegmentLabel()
                                              },
                                          new Segment
                                              {
                                                  Contents = 
                                                  "REF~BF~249040",
                                                  Label = "REF".GetSegmentLabel()
                                              },
                                              new Segment{Contents = "CTT~4", Label = SegmentLabel.SummaryLabel },
                                      new Segment {Contents = "SE*7*0001", Label = SegmentLabel.DocumentClose}
                                  };
            var order = new OrderRequestReceivedMessage();
             _sut.ProcessLines(segmentList, order);
        }

        [Test]
        public void can_process_lines()
        {
            var segmentList = new List<Segment>
                                  {
                                      new Segment {Contents = "ST*850*0001", Label = SegmentLabel.DocumentLabel},
                                      new Segment
                                          {Contents = "BEG*00*SA*4110076497**20071031*CUR*BY*USD", Label = "BEG".GetSegmentLabel()},
                                      new Segment
                                          {
                                              Contents = "PO1*1*500*EA*6.75**IN*CAD33*VN*FINCAD33*PD*BIG BOX*TP*S",
                                              Label = "PO1".GetSegmentLabel()
                                          },
                                      new Segment
                                          {
                                              Contents =
                                                  "PO1*2*110*EA*4.75**IN*CAD34-A*VN*FINCAD34-A*PD*LITTLE BOX*TP*S",
                                              Label = "PO1".GetSegmentLabel()
                                          },
                                          new Segment
                                              {
                                                  Contents = 
                                                  "REF~CO~249040291943406",
                                                  Label = "REF".GetSegmentLabel()
                                              },
                                          new Segment
                                              {
                                                  Contents = 
                                                  "REF~BF~249040",
                                                  Label = "REF".GetSegmentLabel()
                                              },
                                          new Segment
                                              {
                                                  Contents = 
                                                  "REF~P4~9002087",
                                                  Label = "REF".GetSegmentLabel()
                                              },
                                      new Segment
                                          {
                                              Contents = "PO1*3*15*EA*1.75**IN*SCAD33*VN*FINSCAD33*PD*FOAM THINGY*TP*S",
                                              Label = "PO1".GetSegmentLabel()
                                          },
                                              new Segment{Contents = "CTT~4", Label = SegmentLabel.SummaryLabel },
                                      new Segment {Contents = "SE*5*0001", Label = SegmentLabel.DocumentClose}
                                  };
            var order = new OrderRequestReceivedMessage();
            var docLines = _sut.ProcessLines(segmentList, order);
            Assert.That(order.LineCount == 3);
            Assert.That(docLines.Count == 3);  
        }
    }
}