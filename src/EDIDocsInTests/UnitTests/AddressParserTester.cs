using System.Collections.Generic;
using AFPST.Common.Structures;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsIn.impl;
using EdiMessages;
using NUnit.Framework;
using StructureMap;

namespace EDIDocsProcessing.Tests.UnitTests
{
    [TestFixture]
    public class AddressParserTester
    {
        private IAddressParser _aparse;

        [TestFixtureSetUp]
        public void SetUp()
        {
        }

        [Test]
        public void can_extract_addresses()
        {
            ObjectFactory.Initialize(x =>
                                         {
                                             x.For<IAddressParser>().Use<AddressParser>();
                                         });

            _aparse = ObjectFactory.GetInstance<IAddressParser>();
            var order = new OrderRequestReceivedMessage { Customer = new Customer { CustomerIDs = new CustomerAliases() { LegacyCustomerID = "Initech" }, CustomerName = "Initech" } };
            var lst = new List<Segment>
                          {
                              new Segment {Contents = "N1*ST*EPS LOADING DOCK*92*SUSA", Label = "N1".GetSegmentLabel()},
                              new Segment {Contents = "N3*12825 FLUSHING MEADOWS DR FL 2*STE 280", Label = "N3".GetSegmentLabel()},
                              new Segment {Contents = "N3*Third Floor*", Label = "N3".GetSegmentLabel()},
                              new Segment {Contents = "N3*Next to the bank*", Label = "N3".GetSegmentLabel()},
                              new Segment {Contents = "N4*SAINT LOUIS*MO*638592*US", Label = "N4".GetSegmentLabel()},
                              new Segment {Contents = "N1*BT*EPS SETTLEMENTS GROUP*92*SUSA", Label = "N1".GetSegmentLabel()},
                              new Segment {Contents = "N3*12825 FLUSHING MEADOWS DR FL 2*STE 280", Label = "N3".GetSegmentLabel()},
                              new Segment {Contents = "N3*Third Floor*", Label = "N3".GetSegmentLabel()},
                              new Segment {Contents = "N3*Next to the bank*", Label = "N3".GetSegmentLabel()},
                              new Segment {Contents = "N4*SAINT LOUIS*MO*638592*US", Label = "N4".GetSegmentLabel()},
                              new Segment {Contents = "SE*16*0001~", Label = SegmentLabel.DocumentClose}
                          };
            _aparse.ProcessAddresses(lst, order);

            Assert.That(order.ShipToAddress.AddressName == "EPS LOADING DOCK");

            Assert.That(order.Customer.BillToAddress.AddressName == "EPS SETTLEMENTS GROUP");
        }
    }
}