using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using AFPST.Common.Extensions;


namespace AFPST.Common.EDI
{
    public class EDIXmlUtility
    {
        /*
        public static string test_attribute(string attribute_name, 
             XElement nde)
        {
            if (nde == null) return "";
            if (nde.Attribute((XName)attribute_name) == null)
                return "";
            return nde.Attribute((XName)attribute_name).Value;
        }

        public static XElement first_element_child(XElement nde)
        {
            var qry = from n in nde.Elements()
                      select n;
            if (qry == null) return null;
            return qry.First(); 
        }

        public static string encode(string val)
        {
            val = XmlConvert.EncodeName(val);
            return val;
        }

        public static XElement get_first_element(string name, XDocument doc)
        {
            var qry = from e in doc.Elements()
                      where e.Name.LocalName == name
                      select e;
            if (qry == null) return null;
            return qry.First(); 
        }

        public static string get_encoded_text(XElement nde)
        {
            if (nde.FirstNode == null)
                return "";
            if (nde.FirstNode.NodeType == XmlNodeType.Text)
                return nde.FirstNode.ToString();
            return "";

        }

        public static void set_attribute(string attribute_name, string value,
             XElement nde)
        { 
            if (nde == null) return;
            if (nde.Attribute(attribute_name) == null)
            {
                nde.Add(generate_attribute(attribute_name, value));
            }
            else
                nde.Attribute(attribute_name).Value = value;
        }

        public static XAttribute generate_attribute(string name, string val)
        { 
            return new XAttribute(name, val); 
        }

        public static XElement generate_element(string name, string val)
        {
            XElement el = new XElement(name);
            if(val != null)
            el.Add(new XText(val));
            return el;
        }

        public static string test_element(XElement root, string element_name)
        {
            if (root.Element(element_name) == null) return "";
            //if(root.Element(element_name).FirstNode.NodeType != System.Xml.XmlNodeType.Text) return "";
            return root.Element(element_name).Value;
        }

        public static void set_boolean(XElement root, string name, bool val)
        {
            if (val)
            {
                if (root.Element(name) == null)
                    root.Add(generate_element(name,null));
            }
            else
            {
                if (root.Elements(name) != null)
                    root.Elements(name).Remove();
            }
        }

        public static bool get_boolean(XElement root, string name)
        {
            return root.Element(name) != null;
        }

        public static void set_element(XElement root, 
            string element_name, string el_val)
        { 
            XElement el = root.Element(element_name);
            if (el == null)
            {
                el = new XElement(element_name);
                root.Add(el);
            }
            if(el_val != null)
                root.SetElementValue(element_name, el_val);
        }
        */
        public static IEnumerable<XElement> get_elements_by_spec_name(XElement elm,
                                                                      string el_name, string spec_name)
        {
            var els = from e in elm.Elements(el_name)
                      where  e.Attribute("SpecName").GetSafeValue() == spec_name
                      select e;
            //Just return first element
            return els;
        }

        public static IEnumerable<XElement> get_descendants_by_spec_name(XElement elm,
                                                                         string el_name, string spec_name)
        {
            var els = from e in elm.Descendants(el_name)
                      where e.Attribute("SpecName").GetSafeValue() == spec_name
                      select e;
            //Just return first element
            return els;
        } 

        public static XElement get_element_by_spec_name(XElement elm,
                                                        string el_name, string spec_name)
        {
            var els = from e in elm.Elements(el_name)
                      where e.Attribute("SpecName").GetSafeValue() == spec_name
                      select e;
            //Just return first element
            foreach (var el in els)
                return el;
            return null;
        }

        public static XElement get_descendant_by_spec_name(XElement elm,
                                                           string el_name, string spec_name)
        {
            var els = from e in elm.Descendants(el_name)
                      where e.Attribute("SpecName").GetSafeValue() == spec_name
                      select e;
            //Just return first element
            foreach (var el in els)
                return el;
            return null;
        }
    }
}