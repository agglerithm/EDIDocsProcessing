using System.Collections.Generic;

namespace EDIDocsProcessing.Core.DocsOut.EDIStructures
{
    public class NodeTemplate
    {
        private readonly List<NodeTemplate> _list = new List<NodeTemplate>();

        public string Value 
        { 
            get
            { 
                return string.Format("ID={0},NodeType={1},NodeName={2},Context={3}",
                                     ID, NodeType, NodeName, Context);
            }
        }

        public int ID
        { 
            get; set;
        } 

        public string NodeType
        {
            get; set;
        }

        public string NodeName
        {
            get; set;
        }

        public string Context
        {
            get; set;
        }

        public NodeTemplate Parent
        {
            get; set;
        }

        public List<NodeTemplate> Children
        { 
            get
            {
                return _list;
            }
        }

        public void Add(NodeTemplate node)
        {
            _list.Add(node);
        }
    }

    public class DocTemplate : NodeTemplate
    { 
        public int DocType
        {
            get; set;
        }

        public int BPID
        {
            get; set;
        }

 
    }
}