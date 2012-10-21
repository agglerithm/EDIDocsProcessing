using System.Collections.Generic;
using EDIDocsProcessing.Common.EDIStructures;

namespace EDIDocsProcessing.Common
{
    public interface IEdiFileReader
    {
        EdiFileInfo Read(EdiSegmentCollection segments);
    }
}