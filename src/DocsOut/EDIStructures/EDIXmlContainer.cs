using System.Linq;
using System.Xml.Linq;  

namespace EDIDocsProcessing.Core.DocsOut.EDIStructures
{
    public class EDIXmlContainer : EDIXmlNode
    {

        protected EDIXmlContainer _element_list;

        public int Count(string element_name)
        {
            return  list_node.EDINodes(element_name).Count();
        }

        public virtual XElement this[int ndx]
        {
            get { return list_node.Elements().ElementAt(ndx); }
        }

        public virtual int ChildCount
        {
            get { return list_node.Elements().Count(); }
        }

        private EDIXmlNode list_node
        {
            get { if(_element_list == null) return this;
                return _element_list; 
            }
        }

        public EDIXmlContainer(string name) : base(name)
        { 
        }

        protected static string el_delimiter
        { get { return EDIXmlBuildValues.ElementDelimiter;}
        }

        private static string seg_delimiter
        {
            get { return EDIXmlBuildValues.SegmentDelimiter; }
        }

 


  
 
    }
}