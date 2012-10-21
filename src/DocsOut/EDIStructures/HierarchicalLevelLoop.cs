using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Core.DocsOut.EDIStructures
{
    public class HierarchicalLevelLoop : EDIXmlMixedContainer 
    {
        private string _id;
        public HierarchicalLevelLoop()
            : base("ediLoop")
        {
            Label = "HL"; 
        }

        public void SetHeader(string ID, string level_code)
        {
            _id = ID;
            header = SegmentFactory.GetHierarchicalLevel(ID, "", level_code, true);
            AddSegment(header);
        }
     
        public void AddLevel(string ID, string level_code)
        {
            AddSegment(SegmentFactory.GetHierarchicalLevel(ID, _id, level_code, false));
        }

        public void AddLevel(string ID, string parent_id, string level_code)
        {
            AddSegment(SegmentFactory.GetHierarchicalLevel(ID, parent_id, level_code, false));
        }
    }
}