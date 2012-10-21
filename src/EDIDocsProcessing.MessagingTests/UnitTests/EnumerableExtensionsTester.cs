using System.Collections.Generic;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsIn.impl;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests
{
    [TestFixture]
    public class EnumerableExtensionsTester
    {
        [Test]
        public void can_remove_while()
        {
            var segs = new List<Segment>
                           {
                               new Segment {Label = SegmentLabel.InterchangeLabel},
                               new Segment {Label = SegmentLabel.GroupLabel},
                               new Segment {Label = SegmentLabel.DocumentLabel},
                               new Segment {Label = SegmentLabel.ReferenceLabel},
                               new Segment {Label = SegmentLabel.ReferenceLabel},
                               new Segment {Label = SegmentLabel.AddressNameLabel},
                               new Segment {Label = SegmentLabel.AddressLineLabel},
                               new Segment {Label = SegmentLabel.GeographicLabel},
                               new Segment {Label = SegmentLabel.DocumentClose},
                               new Segment {Label = SegmentLabel.GroupClose},
                               new Segment {Label = SegmentLabel.InterchangeClose}
                           };
            segs.RemoveWhile(seg => seg.Label != SegmentLabel.AddressNameLabel);

            Assert.That(segs[0] != null);

            Assert.That(segs[0].Label == SegmentLabel.AddressNameLabel);
        }
    }
}