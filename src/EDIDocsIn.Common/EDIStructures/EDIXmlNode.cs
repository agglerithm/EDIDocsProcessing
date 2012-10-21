using System.Xml.Linq;

namespace EDIDocsProcessing.Common.EDIStructures
{
    using EdiMessages;

    public class EDIXmlNode : XElement
    {  
        public EDIXmlNode(string name)
            : base(name)
        { 
        }
 
        public string Label
        {
            get { return test_attribute(EdiStructureNameConstants.Label); }
            set { set_attribute(EdiStructureNameConstants.Label, value); }
        }
 
        protected string test_element(string element_name)
        {
            return  XMLUtilities.test_element(this, element_name);
        }

        protected void set_element(string element_name, string el_val)
        {
            XMLUtilities.set_element(this, element_name, el_val);
        }

        protected string test_attribute(string name)
        {
            return XMLUtilities.test_attribute(name, this);
        }

        protected void set_attribute(string name, string val)
        {
            XMLUtilities.set_attribute(name, val, this);
        }

    }
}