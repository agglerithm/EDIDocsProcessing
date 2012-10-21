using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace EDIDocsOut.Common.EDIStructures
{
    public class EDISchemaNode : EDIXmlContainer 
    {
        public EDISchemaNode(XElement el):base(el)
        {
            set_children();
        }

        protected override void swap_child(XElement obj)
        {
            Add(new EDISchemaNode(obj));
        }

        public string Type
        {
            get { return Name.LocalName;  }
        }

        public new EDISchemaNode this[int ndx]
        {
            get { return (EDISchemaNode) Elements().ElementAt(ndx); }
        }

        public IEnumerable<EDISchemaNode> Loops
        {
            get { return (IEnumerable<EDISchemaNode>)Elements("loop");  }
        }

        public IEnumerable<EDISchemaNode> Segments
        {
            get { return (IEnumerable<EDISchemaNode>)Elements("segment"); }
        }

        public int NodeCount
        {
            get { return Elements().Count(); }
        }
    }
}