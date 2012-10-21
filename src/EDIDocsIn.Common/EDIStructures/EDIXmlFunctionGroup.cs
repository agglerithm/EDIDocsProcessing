using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Common.EDIStructures
{
    using EdiMessages;

    public class EDIXmlFunctionGroup : EDIXmlMixedContainer
    {
        private readonly ISegmentFactory _segmentFactory;

        public EDIXmlFunctionGroup(ISegmentFactory segmentFactory)
            : base(EdiStructureNameConstants.FunctionGroup)
        {
            _segmentFactory = segmentFactory;
            Label = SegmentLabel.GroupLabel.Text;
        }

        public void SetHeader(int controlNo, string functionalId)
        {
            _header = _segmentFactory.GetGroupHeader(functionalId,   controlNo);
            AddSegment(_header);
        }

        public void SetFooter(int numberOfTransactions,  int controlNo)
        {
            _footer = _segmentFactory.GetGroupFooter(numberOfTransactions, controlNo);
            AddSegment(_footer);
        }

 
    }
}