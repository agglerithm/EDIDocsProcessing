using System.Collections.Generic;
using System.Xml.Linq;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Common.Extensions
{
    using EdiMessages;

    public static class XElementEDIExtensions
    {
        public static IEnumerable<XElement> EDITransactions(this XElement el)
        {
            return el.Elements(EdiStructureNameConstants.TransactionSet);
        }

        public static IEnumerable<XElement> EDIFunctionGroups(this XElement el)
        {
            return el.Elements(EdiStructureNameConstants.FunctionGroup);
        }

        public static IEnumerable<XElement> EDIInterchangeControls(this XElement el)
        {
            return el.Elements(EdiStructureNameConstants.InterchangeControl);
        }

        public static IEnumerable<XElement> EDIElements(this XElement el)
        {
            return el.Elements(EdiStructureNameConstants.Element);
        }

        public static IEnumerable<XElement> EDISegments(this XElement el)
        {
            return el.Elements(EdiStructureNameConstants.Segment);
        }

        public static IEnumerable<XElement> EDILoops(this XElement el)
        {
            return el.Elements(EdiStructureNameConstants.Loop);
        }

        public static IEnumerable<XElement> EDILoops(this XElement el, string name)
        {
            return EDIXmlUtility.get_elements_by_label(el, EdiStructureNameConstants.Loop, name);
        }

        public static IEnumerable<XElement> EDISegments(this XElement el, string name)
        {
            return EDIXmlUtility.get_elements_by_label(el, EdiStructureNameConstants.Segment, name);
        }

        public static XElement EDIElement(this XElement el, string name)
        {
            return EDIXmlUtility.get_element_by_label(el, EdiStructureNameConstants.Element, name);
        }

        public static XElement EDIDescendantSegment(this XElement el, string name)
        {
            return EDIXmlUtility.get_descendant_by_label(el, EdiStructureNameConstants.Segment, name);
        }

        public static XElement EDISegment(this XElement el, string name)
        {
            return EDIXmlUtility.get_element_by_label(el, EdiStructureNameConstants.Segment, name);
        }

        public static XElement EDILoop(this XElement el, string name)
        {
            return EDIXmlUtility.get_element_by_label(el, EdiStructureNameConstants.Loop, name);
        }

        public static string EDIValue(this XElement el)
        {
            return el.Name.LocalName != EdiStructureNameConstants.Element ? "" : el.Element("elementValue").Value;
        }

        public static string EDIValue(this EDIXmlSegment seg, string name)
        {
            var el = seg.EDIElement(name);
            return el.EDIValue();
        }


//        public static IEnumerable<XElement> EDINodes(this XElement el, string spec_name)
//        {
//            return  el.Elements().Where(x => (x.Attribute("SpecName").Value == spec_name));
//        }

//        public static string EDIElementValue(this XElement el, string name)
//        {
//            return el.EDIElement(name).EDIValue();
//        }
 
 
    }
}