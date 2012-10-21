using System.Collections.Generic;
using EDIDocsProcessing.Common.EDIStructures;

namespace EDIDocsProcessing.Common
{
    public interface ISegmentSplitter
    {
        EdiSegmentCollection Split(string contents);
    }
}