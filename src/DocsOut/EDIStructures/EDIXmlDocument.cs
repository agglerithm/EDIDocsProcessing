using System.Xml.Linq;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Core.DocsOut.EDIStructures
{
    public class EDIXmlDocument : EDIXmlContainer
    { 
 

        public EDIXmlDocument(string docType, int control_number):base("ediDocument")
        {
            var seg = SegmentFactory.GetDocumentHeader(docType, control_number);
            Add(seg);
        }

//        public string FilePath
//        {
//            get { return test_attribute("filePath"); }
//            set { set_attribute("filePath", value); }
//        }

        public new EDIXmlInterchangeControl this[int ndx]
        {
            get { return (EDIXmlInterchangeControl)base[ndx]; }
        }

    }
}