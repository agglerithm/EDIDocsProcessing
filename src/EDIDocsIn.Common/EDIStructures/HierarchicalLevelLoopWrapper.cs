namespace EDIDocsProcessing.Common.EDIStructures
{
    using System;
    using System.Linq;
    using EdiMessages;

    public class HierarchicalLevelLoopWrapper
    {
        private int _currentId = 1;
        private bool _printHasChildElement;
        private HierarchicalLevelLoop Root { get;   set; }

        public string Value
        {
            get { return Root.Value; } 
        }

        public object SegmentCount
        {
            get { return Root.Count(EdiStructureNameConstants.Segment); } 
        }

        public static HierarchicalLevelLoopWrapper BuildWrapper(string code, ISegmentFactory factory)
        {
            return new HierarchicalLevelLoopWrapper(code, factory, true);
        }
        public static HierarchicalLevelLoopWrapper BuildWrapper(string code, ISegmentFactory factory, bool hasChild)
        { 
            return new HierarchicalLevelLoopWrapper(code, factory, hasChild);
        }
        private HierarchicalLevelLoopWrapper(string code, ISegmentFactory factory, bool hasChild)
        { 
            Root = new HierarchicalLevelLoop(factory, hasChild);
            _printHasChildElement = hasChild;
            Root.SetHeader(_currentId, code);
        }

        public void AddTo(EDIXmlTransactionSet transactionSet)
        {
            transactionSet.AddLoop(Root);
        }
        public HierarchicalLevelLoop AddLevel(string code, HierarchicalLevelLoop parent)
        {
            if (parent != Root && !Root.Descendants().Contains(parent))
                throw new InvalidOperationException("Specified loop is not part of hierarchy");
            return parent.AddLevel(++_currentId, code);
        }

        public void AddLoop(EDIXmlMixedContainer loop)
        {
            Root.AddLoop(loop);
        }

        public void AddSegment(EDIXmlSegment segment)
        {
            Root.AddSegment(segment);
        }

        public HierarchicalLevelLoop AddLevel(string code)
        {
            return AddLevel(code, Root);
        }

        public string GetId()
        {
            return Root.GetId();
        }

        public string GetParent()
        {
            return Root.GetParent();
        }
    }
}