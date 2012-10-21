using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Core;
using EDIDocsProcessing.Core.DocsIn.impl;
using NUnit.Framework;

namespace EDIDocsProcessing.Tests.UnitTests.Core
{
    [TestFixture]
    public class SplitLoopTester
    {
 

        [Test]
        public void can_split_loop()
        {
            var listList = get_list().SplitLoop();
            listList.Count.ShouldEqual(3);

            listList[0].Count.ShouldEqual(4);
            listList[1].Count.ShouldEqual(3);
            listList[2].Count.ShouldEqual(2);
        }

 

        private List<Segment> get_list()
        {
            return new List<Segment>
                              {
                                  new Segment {Label = SegmentLabel.PurchaseOrderChange},
                                  new Segment {Label = SegmentLabel.ReferenceLabel},
                                  new Segment {Label = SegmentLabel.PricingInformation},
                                  new Segment {Label = SegmentLabel.ProductItemDescription},
                                  new Segment {Label = SegmentLabel.PurchaseOrderChange}, 
                                  new Segment {Label = SegmentLabel.PricingInformation},
                                  new Segment {Label = SegmentLabel.ProductItemDescription},
                                  new Segment {Label = SegmentLabel.PurchaseOrderChange},
                                  new Segment {Label = SegmentLabel.ReferenceLabel}, 
                              };
        }
    }
}
