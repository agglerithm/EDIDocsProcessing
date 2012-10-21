using System.Collections.Generic;
using EDIDocsProcessing.Common.EDIStructures;

namespace EDIDocsProcessing.Common
{
    public interface IHierarchySplitter
    {
        IEnumerable<GroupContainer> SplitByGroup(EdiSegmentCollection segments, InterchangeContainer parent);
        IEnumerable<InterchangeContainer>   SplitByInterchange(EdiSegmentCollection segments);
        IEnumerable<DocContainer> SplitByDocument(EdiSegmentCollection segments, GroupContainer parent);
    }
}