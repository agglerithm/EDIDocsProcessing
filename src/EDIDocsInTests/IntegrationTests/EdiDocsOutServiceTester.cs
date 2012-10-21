using System;
using System.Collections.Generic;
using EDIDocsOut;
using EDIDocsOut.config;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Core.DocsOut;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;
using EDIDocsProcessing.Core.impl;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.IntegrationTests
{
    [TestFixture]
    public class EdiDocsOutServiceTester
    {
        private EdiDocsOutSubscriberStarter _sut;

        [TestFixtureSetUp]
        public void SetUp()
        {
            StructureMapBootstrapper.Execute();

            _sut = new EdiDocsOutSubscriberStarter();
        }

        [Test]
        public void can_build_invoice_detail()
        {
            var segFactory = new SegmentFactory(new BusinessPartnerSpecificServiceResolver());
            segFactory.SetBuildValues(BusinessPartner.Initech);
            IDictionary<Qualifier, string> values = new Dictionary<Qualifier, string>
                                                        {
                                                            {Qualifier.InvoiceVendorPart, "FIN23432"},
                                                            {Qualifier.PartDescription, "rectangular box"},
                                                            {Qualifier.PONumber, "32543243"},
                                                            {Qualifier.POLineNumber, "10"}
                                                        };
            EDIXmlSegment seg = segFactory.GetLineItemInvoiceDetail("10",
                                                                    1, (decimal) .99, values);
            Console.WriteLine(seg.Value);
        }

        [Test]
        public void can_start_service()
        {
            _sut.Start(true);
        }
    }
}