namespace EDIDocsProcessing.Tests.UnitTests.MicroCenter
{
    using System.Collections.Generic;
    using System.Linq;
    using EDIDocsProcessing.Common;
    using EDIDocsProcessing.Common.DTOs;
    using EDIDocsProcessing.Common.EDIStructures;
    using EDIDocsProcessing.Common.Enumerations;
    using EDIDocsProcessing.Common.Extensions;
    using EDIDocsProcessing.Core.DocsIn;
    using EDIDocsProcessing.Core.DocsIn.impl;
    using FedexEDIDocsIn;
    using global::EdiMessages;
    using MicroCenterEDIDocsIn;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class MicroCenter850ParserTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _addrParser = new Mock<IAddressParser>();
            _lineParser = new Mock<IMicroCenter850LineParser>();
            _recorder = new Mock<IEDIResponseReferenceRecorder>();
            _genericParser = new Mock<IGeneric850Parser>();
            _sut = new MicroCenter850Parser(_addrParser.Object, _lineParser.Object,  _recorder.Object, _genericParser.Object);
        }

        #endregion

        private IDocumentParser _sut;
        private Mock<IAddressParser> _addrParser;
        private Mock<IMicroCenter850LineParser> _lineParser;
        private Mock<IEDIResponseReferenceRecorder> _recorder;
        private Mock<IGeneric850Parser> _genericParser;

        [Test]
        public void can_determine_if_can_process()
        {
            var seg = new Segment {Contents = "ST*850*0001", Label = "ST".GetSegmentLabel()};
            _genericParser.Setup(p => p.CanProcess(seg)).Returns(true);
            Assert.That(_sut.CanProcess(BusinessPartner.MicroCenter, "850"));
        }

        [Test]
        public void can_process_addresses()
        {
            //ARRANGE
            var segmentList = new List<Segment>
                                  {
                                      new Segment {Contents = "ST*850*0001", Label = SegmentLabel.DocumentLabel},
                                      new Segment
                                          {Contents = "BEG*00*SA*25151**20120625", Label = "BEG".GetSegmentLabel()},
//                              new Segment {Contents = "N1*ST*EPS LOADING DOCK*92*SUSA", Label = "N1"},
//                              new Segment {Contents = "N3*12825 FLUSHING MEADOWS DR FL 2*STE 280", Label = "N3"},
//                              new Segment {Contents = "N3*Third Floor*", Label = "N3"},
//                              new Segment {Contents = "N3*Next to the bank*", Label = "N3"},
//                              new Segment {Contents = "N4*SAINT LOUIS*MO*638592*US", Label = "N4"},
                                      new Segment {Contents = "SE*8*0001", Label = SegmentLabel.DocumentClose}
                                  }; 
            _genericParser.Setup(p => p.ProcessSegmentList(segmentList)).Returns(new OrderRequestReceivedMessage(){ControlNumber = "0001"});
            _genericParser.Setup(p => p.ElementDelimiter).Returns("*");
            _addrParser.Setup(x => x.ProcessAddresses(segmentList, It.IsAny<OrderRequestReceivedMessage>())).Returns(5);

            _lineParser.Setup(x => x.ProcessLines(It.IsAny<List<Segment>>(), It.IsAny<IEdiMessage>()));
            //_lineParser.Setup(x => x.SegmentCount).Returns(1);

            //ACT
            OrderRequestReceivedMessage orderRequestReceived = (OrderRequestReceivedMessage) _sut.ProcessSegmentList(segmentList).Message;

            //ASSERT
            Assert.That(segmentList.Count, Is.EqualTo(0));
            _addrParser.VerifyAll();


//            Assert.That(order.ShipToAddress.City == "SAINT LOUIS");
//            Assert.That(order.ShipToAddress.AddressName == "EPS LOADING DOCK");
//            Assert.That(order.ShipToAddress.Address1 == "12825 FLUSHING MEADOWS DR FL 2");
//            Assert.That(order.ShipToAddress.State == "MO");
//            Assert.That(order.ShipToAddress.Zip == "638592");
        }

        [Test]
        public void can_process_admin_contacts()
        {
            //ARRANGE
            var segmentList = new List<Segment>
                                  {
                                      new Segment {Contents = "ST*850*0001", Label = SegmentLabel.DocumentLabel},
                                      new Segment
                                          {Contents = "BEG*00*SA*25151**20120625", Label = "BEG".GetSegmentLabel()},
                                      new Segment {Contents = "PER*DC*Jennifer Williams", Label = "PER".GetSegmentLabel()},
                                      new Segment {Contents = "SE*5*0001", Label = SegmentLabel.DocumentClose}
                                  };

            _genericParser.Setup(p => p.ProcessSegmentList(segmentList)).Returns(new OrderRequestReceivedMessage() { ControlNumber = "0001" });
            _genericParser.Setup(p => p.ElementDelimiter).Returns("*");
            _addrParser.Setup(x => x.ProcessAddresses(segmentList, It.IsAny<OrderRequestReceivedMessage>())).Returns(1);

            _lineParser.Setup(x => x.ProcessLines(It.IsAny<List<Segment>>(), It.IsAny<IEdiMessage>()));
            //_lineParser.Setup(x => x.SegmentCount).Returns(4);

            //ACT
            _sut.ProcessSegmentList(segmentList);

            //ASSERT
            Assert.That(segmentList.Count, Is.EqualTo(0));
        }

        [Test]
        public void can_process_addresses_and_admin_contacts()
        {
            //ARRANGE
            var segmentList = new List<Segment>
                                  {
                                      new Segment {Contents = "ST*850*1001", Label = SegmentLabel.DocumentLabel},
                                      new Segment
                                          {Contents = "BEG*00*SA*25151**20120625", Label = "BEG".GetSegmentLabel()},
                              new Segment {Contents = "N1*ST*EPS LOADING DOCK*92*SUSA", Label = "N1".GetSegmentLabel()},
                              new Segment {Contents = "N3*12825 FLUSHING MEADOWS DR FL 2*STE 280", Label = "N3".GetSegmentLabel()},
                              new Segment {Contents = "N3*Third Floor*", Label = "N3".GetSegmentLabel()},
                              new Segment {Contents = "N3*Next to the bank*", Label = "N3".GetSegmentLabel()},
                              new Segment {Contents = "N4*SAINT LOUIS*MO*638592*US", Label = "N4".GetSegmentLabel()},
                                      new Segment {Contents = "PER*DC*Jennifer Williams", Label = "PER".GetSegmentLabel()},
                                    new Segment {Contents = "SE*9*1001", Label = SegmentLabel.DocumentClose}
                                  };

            _genericParser.Setup(p => p.ProcessSegmentList(segmentList)).Returns(new OrderRequestReceivedMessage() { ControlNumber = "1001" });
            _genericParser.Setup(p => p.ElementDelimiter).Returns("*");
            _addrParser.Setup(x => x.ProcessAddresses(segmentList, It.IsAny<OrderRequestReceivedMessage>())).Returns(5);

            _lineParser.Setup(x => x.ProcessLines(It.IsAny<List<Segment>>(), It.IsAny<IEdiMessage>()));
            //_lineParser.Setup(x => x.SegmentCount).Returns(1);

            //ACT
            OrderRequestReceivedMessage orderRequestReceived = (OrderRequestReceivedMessage) _sut.ProcessSegmentList(segmentList).Message;

            //ASSERT
            Assert.That(segmentList.Count, Is.EqualTo(5));
            _addrParser.VerifyAll();


            //            Assert.That(order.ShipToAddress.City == "SAINT LOUIS");
            //            Assert.That(order.ShipToAddress.AddressName == "EPS LOADING DOCK");
            //            Assert.That(order.ShipToAddress.Address1 == "12825 FLUSHING MEADOWS DR FL 2");
            //            Assert.That(order.ShipToAddress.State == "MO");
            //            Assert.That(order.ShipToAddress.Zip == "638592");
        }
        [Test]
        public void can_process_lines()
        {
            //ARRANGE
            var segmentList = new List<Segment>
                                  {
                                      new Segment {Contents = "ST*850*1001", Label = SegmentLabel.DocumentLabel},
                                      new Segment
                                          {Contents = "BEG*00*SA*25151**20120625", Label = "BEG".GetSegmentLabel()},
                                      new Segment {Contents = "SE*8*1001", Label = SegmentLabel.DocumentClose}
                                  };

            _genericParser.Setup(p => p.ProcessSegmentList(segmentList)).Returns(new OrderRequestReceivedMessage() { ControlNumber = "1001" });
            _genericParser.Setup(p => p.ElementDelimiter).Returns("*");
            _addrParser.Setup(x => x.ProcessAddresses(segmentList, It.IsAny<OrderRequestReceivedMessage>())).Returns(1);

            _lineParser.Setup(x => x.ProcessLines(It.IsAny<List<Segment>>(), It.IsAny<IEdiMessage>()));
            _lineParser.Setup(x => x.SegmentCount).Returns(4);

            //ACT
            _sut.ProcessSegmentList(segmentList);

            //ASSERT
            Assert.That(segmentList.Count, Is.EqualTo(0));
            _lineParser.VerifyAll();
        }

        [Test]
        public void can_process_references()
        {
            //ARRANGE
            var refList = new List<Segment> { 
                                      new Segment {Contents = "REF*I5*OR", Label = "REF".GetSegmentLabel()},
                                      new Segment
                                          {Contents = "REF*19*001", Label = "REF".GetSegmentLabel()},
                                      new Segment
                                          {Contents = "REF*XE*1", Label = "REF".GetSegmentLabel()},
                                      new Segment
                                          {Contents = "REF*ZZ*CAD33", Label = "REF".GetSegmentLabel()} 
                                  };
            var segmentList = new List<Segment>
                                  {
                                      new Segment {Contents = "ST*850*1001", Label = SegmentLabel.DocumentLabel},
                                      new Segment
                                          {Contents = "BEG*00*SA*25151**20120625", Label = "BEG".GetSegmentLabel()},
                                      refList[0],
                                      refList[1],
                                      refList[2],
                                      refList[3],
                                      new Segment {Contents = "SE*13*1001", Label = SegmentLabel.DocumentClose}
                                  };
            _genericParser.Setup(p => p.ProcessSegmentList(segmentList)).Returns(new OrderRequestReceivedMessage() { ControlNumber = "1001" });
            _genericParser.Setup(p => p.ElementDelimiter).Returns("*");
            _addrParser.Setup(x => x.ProcessAddresses(segmentList, It.IsAny<OrderRequestReceivedMessage>())).Returns(1);

            _lineParser.Setup(x => x.ProcessLines(It.IsAny<List<Segment>>(), It.IsAny<IEdiMessage>())).Returns(get_lines());
            _lineParser.Setup(x => x.SegmentCount).Returns(5);

            _recorder.Setup(r => r.MemorizeOuterReferences(It.IsAny<List<Segment>>(),It.IsAny<string>(),It.IsAny<string>(),It.IsAny<BusinessPartner>())).Returns(refList);
            _recorder.Setup(r => r.GetResponseValues()).Returns(new List<ResponseElementEntity>
                                                                    {
                                                                        new ResponseElementEntity
                                                                            {
                                                                                ElementName = "REF02", 
                                                                                Qualifier =
                                                                                    Qualifier.ServiceLevelNumber.Value
                                                                            }
                                                                    });
            //ACT
            var orderRequestReceivedMessage = (OrderRequestReceivedMessage)_sut.ProcessSegmentList(segmentList).Message;

            //ASSERT
            Assert.That(segmentList.Count, Is.EqualTo(0)); 
        }

        private IList<DocumentLineItemEntity>  get_lines()
        { 
            return new List<DocumentLineItemEntity>
                            {
                                new DocumentLineItemEntity()
                                    {
                                        ResponseElements = new List<LineResponseElementEntity>
                                                                       {
                                                                           new LineResponseElementEntity {ElementName = "REF02", Qualifier = "CO", Value = "345234"},
                                                                           new LineResponseElementEntity {ElementName = "REF02", Qualifier = "BF", Value = "333444"}
                                                                       }
                                    }
                            };
        }


       
        [Test, ExpectedException(typeof (Invalid850Exception),
            UserMessage = "BEG Segment is missing!")]
        public void will_fail_if_begin_segment_missing()
        {
            //ARRANGE
            var segmentList = new List<Segment>
                                  {
                                      new Segment {Contents = "ST*850*1001", Label = SegmentLabel.DocumentLabel},
                                      new Segment {Contents = "SE*11*1001", Label = SegmentLabel.DocumentClose}
                                  };
            _genericParser.Setup(p => p.ProcessSegmentList(segmentList)).Returns(new OrderRequestReceivedMessage() { ControlNumber = "1001" });
            _genericParser.Setup(p => p.ElementDelimiter).Returns("*");
            _addrParser.Setup(x => x.ProcessAddresses(segmentList, It.IsAny<OrderRequestReceivedMessage>())).Returns(5);

            _lineParser.Setup(x => x.ProcessLines(It.IsAny<List<Segment>>(), It.IsAny<IEdiMessage>()));
            //_lineParser.Setup(x => x.SegmentCount).Returns(1);

            //ACT
            _sut.ProcessSegmentList(segmentList);
        }

        [Test, ExpectedException(typeof (InvalidEDIDocumentException),
            UserMessage = "Control numbers in _header and _footer do not match!")]
        public void will_fail_with_mismatched_header_and_footer()
        {
            //ARRANGE
            var segmentList = new List<Segment>
                                  {
                                      new Segment {Contents = "ST*850*1001", Label = SegmentLabel.DocumentLabel},
                                      new Segment
                                          {Contents = "BEG*00*SA*25151**20120625", Label = "BEG".GetSegmentLabel()},
                                      new Segment {Contents = "SE*9*0002", Label = SegmentLabel.DocumentClose}
                                  };
            _genericParser.Setup(p => p.ProcessSegmentList(segmentList)).Returns(new OrderRequestReceivedMessage() { ControlNumber = "1001" });
            _genericParser.Setup(p => p.ElementDelimiter).Returns("*");
            _addrParser.Setup(x => x.ProcessAddresses(segmentList, It.IsAny<OrderRequestReceivedMessage>())).Returns(5);

            _lineParser.Setup(x => x.ProcessLines(It.IsAny<List<Segment>>(), It.IsAny<IEdiMessage>()));
            //_lineParser.Setup(x => x.SegmentCount).Returns(1);

            //ACT
            _sut.ProcessSegmentList(segmentList);
        }

        [Test, ExpectedException(typeof (InvalidEDIDocumentException),
            UserMessage = "Segments processed does not match included segment count!")]
        public void will_fail_with_mismatched_number_of_segments()
        {
            //ARRANGE
            var segmentList = new List<Segment>
                                  {
                                      new Segment {Contents = "ST*850*1001", Label = SegmentLabel.DocumentLabel},
                                      new Segment
                                          {Contents = "BEG*00*SA*25151**20120625", Label = "BEG".GetSegmentLabel()},
                                      new Segment {Contents = "SE*11*0001", Label = SegmentLabel.DocumentClose}
                                  };
            _genericParser.Setup(p => p.ProcessSegmentList(segmentList)).Returns(new OrderRequestReceivedMessage() { ControlNumber = "1001" });
            _genericParser.Setup(p => p.ElementDelimiter).Returns("*");
            _addrParser.Setup(x => x.ProcessAddresses(segmentList, It.IsAny<OrderRequestReceivedMessage>())).Returns(5);

            _lineParser.Setup(x => x.ProcessLines(It.IsAny<List<Segment>>(), It.IsAny<IEdiMessage>()));
            //_lineParser.Setup(x => x.SegmentCount).Returns(1);

            //ACT
            _sut.ProcessSegmentList(segmentList);
        }

       
    }
}