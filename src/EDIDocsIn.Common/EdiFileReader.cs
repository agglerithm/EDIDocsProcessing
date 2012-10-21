using System;
using System.Collections.Generic;
using System.Linq;
using EDIDocsProcessing.Common.EDIStructures;

namespace EDIDocsProcessing.Common
{
    public class EdiFileReader : IEdiFileReader
    {
        private readonly IHierarchySplitter _hierarchySplitter;

        public EdiFileReader(IHierarchySplitter hierarchySplitter)
        {
            _hierarchySplitter = hierarchySplitter;
        }

        public EdiFileInfo Read(EdiSegmentCollection segments)
        {
            var interchanges = _hierarchySplitter.SplitByInterchange(segments);
            return new EdiFileInfo(interchanges);
        }
 
 
    }
}