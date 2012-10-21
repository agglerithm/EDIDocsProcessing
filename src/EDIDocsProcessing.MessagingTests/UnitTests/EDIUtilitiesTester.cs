using System;
using System.Collections.Generic;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Extensions;

using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsIn.impl;
using EdiMessages;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests
{
    [TestFixture]
    public class EDIUtilitiesTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
        }

        #endregion

       

        [Test]
        public void can_determine_date_6()
        {
            Assert.That("090101".DateFromEDIDate() == DateTime.Parse("1/1/2009"));
        }

        [Test]
        public void can_determine_date_8()
        {
            Assert.That("20090101".DateFromEDIDate() == DateTime.Parse("1/1/2009"));
        }

        [Test]
        public void can_process_footer()
        {
            var doc = new OrderRequestReceivedMessage {ControlNumber = "553230001"};
            var segs = new List<Segment> {new Segment {Contents = "SE*1*553230001", Label = "SE".GetSegmentLabel()}};
            EDIUtilities.ProcessFooter(segs, doc, "*", 1);
        }
    }
}