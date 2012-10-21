using System.Xml.Linq;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Common.EDIStructures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EdiMessages;
    using Extensions;

    public class EDIXmlSegment : EDIXmlContainer
    {

        public EDIXmlSegment(string lbl, EdiXmlBuildValues ediXmlBuildValues)
            : base(EdiStructureNameConstants.Segment)
        { 
            Label = lbl; 
            base.Add(new XElement("label",lbl));
            _element_list = new EDIXmlContainer("elementList");
            base.Add(_element_list);
            base.Add(new XElement("ediSegmentDelimiter", ediXmlBuildValues.SegmentDelimiter));
        }

        public new void Add(object obj)
        {  
            _element_list.Add(obj);
        }

        public string SegmentName
        {
            get { return test_element(EdiStructureNameConstants.Label); }
            set { set_element(EdiStructureNameConstants.Label, value); }
        }

        public IEnumerable<EDIXmlElement> EdiElements
        {
            get
            {
                return Descendants().Where(e => e.Name.LocalName == "ediElement").Select(el => (EDIXmlElement) el);
            }
        }
        public EDIXmlElement ElementByLabel(string spec_name)
        {
            return (EDIXmlElement)EDIXmlUtility.get_element_by_label(_element_list, EdiStructureNameConstants.Element, spec_name);
        }

        public void StripEmptyTrailingElements()
        {
            var lastEl = EdiElements.Last();
            while(lastEl.Value == "")
            {
                lastEl.Remove();
                lastEl = EdiElements.Last();
            }
        }

        public void AddElements(IEnumerable<EDIXmlElement> els)
        {
            els.ForEach(Add);
        }
    }
}