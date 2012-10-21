using EDIDocsProcessing.Common.Enumerations;

namespace EDIDocsProcessing.Common.EDIStructures
{
    public class Segment
    {
        public string Contents
        {
            get; set;
        }

        public SegmentLabel Label
        {
            get; set;
        }

        public string Context
        {
            get; set;
        }

        public string [] GetElements(string elementDelimiter)
        {
            if (Contents == null)
                return null;
            return Contents.Split(elementDelimiter.ToCharArray());
        }
    }
}