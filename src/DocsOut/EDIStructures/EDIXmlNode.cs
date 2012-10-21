using System.Xml.Linq;
using EDIDocsProcessing.Common;


namespace EDIDocsProcessing.Core.DocsOut.EDIStructures
{
    public class EDIXmlNode : XElement
    {  
        public EDIXmlNode( )
            : base("ediDocument")
        { 
        }

        public EDIXmlNode(string name)
            : base(name)
        { 
        }
 
        public EDIXmlNode(XElement el)
            : base(el)
        {
            if (el.Parent == null) return;
            //el.Parent.Add(this);
            el.Remove();
        }

        public string Label
        {
            get { return test_attribute("ediLabel"); }
            set { set_attribute("ediLabel", value); }
        }
 
        public string RawValue
        {
            get { return Value; }
        }

        protected string test_element(string element_name)
        {
            /*
            if (this[element_name] == null) return "";
            if (this[element_name].FirstChild == null) return "";
            if (this[element_name].FirstChild.GetType() != Type.GetType("TextNode")) return "";
            return this[element_name].InnerText;
             * */
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

 
        public bool compare_numbers(string num1, string num2)
        {
            try
            {
                return (double.Parse(num1) == double.Parse(num2));
            }
            catch { return false; }
        }

  
    }
}