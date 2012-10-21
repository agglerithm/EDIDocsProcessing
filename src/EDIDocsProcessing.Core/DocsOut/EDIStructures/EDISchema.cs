using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;


namespace EDIDocsOut.Common.EDIStructures
{
    public class EDISchema : EDIXmlContainer 
    {
        public EDISchema(string edi_str):base(XElement.Parse(edi_str))
        {
            set_children();
        }

        public new EDITransactionSchema this[int ndx]
        {
            get {return (EDITransactionSchema)Elements().ElementAt(ndx);}
        }

        public int TransactionCount
        {
            get { return Elements().Count();  }
        }

        protected override void swap_child(XElement obj)
        {
            if(obj.Name.LocalName != "transaction")
                throw new  Exception("Schema node was of the wrong type!");
            Add(new EDITransactionSchema(obj));
        }
    }
}