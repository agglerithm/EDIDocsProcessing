using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Common.EDIStructures
{
    public class EDIXmlContainer : EDIXmlNode
    {
        protected EDIXmlContainer _element_list;

        public EDIXmlContainer(string name) : base(name)
        { 
        }

    }
}