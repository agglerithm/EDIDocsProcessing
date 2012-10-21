using System.Linq;
using System.Xml.Linq;
using EDIDocsProcessing.Core.DocsOut.EDIStructures; 

namespace EDIDocsProcessing.Core.DocsOut.EDIStructures
{
    public class EDIXmlSegment : EDIXmlContainer
    {




        public EDIXmlSegment(string lbl)
            : base("ediSegment")
        { 
            Label = lbl; 
            base.Add(new XElement("label",lbl));
            _element_list = new EDIXmlContainer("elementList");
            base.Add(_element_list);
            base.Add(new XElement("ediSegmentDelimiter",EDIXmlBuildValues.SegmentDelimiter));
        }

        public new void Add(object obj)
        {  
            _element_list.Add(obj);
        }
        public EDIXmlElement Element(string el_name)
        {
            return (EDIXmlElement)_element_list.EDIElement(el_name);
        }

 
        public new EDIXmlElement this[int ndx]
        {
            get { return (EDIXmlElement)_element_list.Elements("ediElement").ElementAt(ndx);  }
        }

        public string SegmentName
        {
            get { return test_element("ediLabel"); }
            set { set_element("ediLabel", value); }
        }

 

        public EDIXmlElement ElementByLabel(string spec_name)
        {
            return (EDIXmlElement)EDIXmlUtility.get_element_by_spec_name(_element_list, "ediElement", spec_name);
        }

 
    }
}