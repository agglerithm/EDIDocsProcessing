using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using EDIDocsOut.Common.EDIStructures;

namespace EDIDocsOut.Common.Extensions
{
    public static class XElementEDIExtensions
    {
        public static IEnumerable<XElement> EDITransactions(this XElement el)
        {
            return el.Elements("ediTransactionSet");
        }

        public static IEnumerable<XElement> EDIFunctionGroups(this XElement el)
        {
            return el.Elements("ediFunctionGroup");
        }

        public static IEnumerable<XElement> EDIInterchangeControls(this XElement el)
        {
            return el.Elements("ediInterchangeControl");
        }

        public static IEnumerable<XElement> EDIElements(this XElement el)
        {
            return el.Elements("ediElement");
        }

        public static IEnumerable<XElement> EDISegments(this XElement el)
        {
            return el.Elements("ediSegment");
        }

        public static IEnumerable<XElement> EDILoops(this XElement el)
        {
            return el.Elements("ediLoop");
        }

        public static IEnumerable<XElement> EDILoops(this XElement el, string name)
        {
            return EDIXmlUtility.get_elements_by_spec_name(el, "ediLoop", name);
        }

        public static IEnumerable<XElement> EDISegments(this XElement el, string name)
        {
            return EDIXmlUtility.get_elements_by_spec_name(el, "ediSegment", name);
        }

        public static XElement EDIElement(this XElement el, string name)
        {
            return EDIXmlUtility.get_element_by_spec_name(el, "ediElement", name);
        }

        public static XElement EDIDescendantSegment(this XElement el, string name)
        {
            return EDIXmlUtility.get_descendant_by_spec_name(el, "ediSegment", name);
        }

        public static XElement EDISegment(this XElement el, string name)
        {
            return EDIXmlUtility.get_element_by_spec_name(el, "ediSegment", name);
        }

        public static XElement EDILoop(this XElement el, string name)
        {
            return EDIXmlUtility.get_element_by_spec_name(el, "ediLoop", name);
        }

        public static string EDIValue(this XElement el)
        {
            return el.Name.LocalName != "ediElement" ? "" : el.Element("elementValue").Value;
        }

        public static IEnumerable<XElement> EDINodes(this XElement el, string spec_name)
        {
            return  el.Elements().Where(x => (x.Attribute("SpecName").Value == spec_name));
        }

        public static string EDIElementValue(this XElement el, string name)
        {
            return el.EDIElement(name).EDIValue();
        }
 
 
    }
}