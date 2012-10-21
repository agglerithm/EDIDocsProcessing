namespace EDIDocsProcessing.Common.EDIStructures
{
    using System;
    using EDIDocsProcessing.Common;
    using EdiMessages;

    public class HierarchicalLevelLoop : EDIXmlMixedContainer 
    {
        private readonly ISegmentFactory _segmentFactory;
        private readonly bool _showHasChildElement;
        private int _id = 1;
        private bool _childElementAdded;

        public HierarchicalLevelLoop(ISegmentFactory segmentFactory, bool showHasChildElement)
            : base(EdiStructureNameConstants.Loop)
        {
            Label = "HL";
            _segmentFactory = segmentFactory;
            _showHasChildElement = showHasChildElement;
        }

 
         
        internal void SetHeader(int id, string levelCode)
        { 
            _id = id; 
            _header = _segmentFactory.GetHierarchicalLevel(GetId(), GetParent(), levelCode);
            AddSegment(_header);
        }

 
        public string GetId()
        {
            return _id.ToString();
        }

        public string GetParent()
        { 
            EDIXmlNode parent = (EDIXmlNode) Parent;
            if (parent == null) return "";
            if (parent.Label == "HL")
            {
                var hlParent = (HierarchicalLevelLoop) parent;
                return hlParent.GetId();
            }
            return "";
        }


        public HierarchicalLevelLoop AddLevel(int id, string code)
        {
            var l = new HierarchicalLevelLoop(_segmentFactory, _showHasChildElement);
            if(_showHasChildElement && !_childElementAdded )
                addChildElement();
            AddLoop(l);
            l.SetHeader(id, code);
            return l;
        }

        private void addChildElement()
        {
            _childElementAdded = true;
            _header.Add(_segmentFactory.GetHLChildElement());
        }


    }
}