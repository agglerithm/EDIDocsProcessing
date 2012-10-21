using System.Xml.Linq;
using EDIDocsOut.Common.EDIStructures;

namespace EDIDocsOut.Common.EDIStructures
{
    public class EDITransactionSchema : EDISchemaNode
    {
        public EDITransactionSchema(XElement el):base(el)
        {
            set_children();
        }

 

        public new string Type
        {
            get { return test_attribute("id"); }
        }

  

    }
}