using System;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Common.EDIStructures
{
    using System.Collections.Generic;
    using System.Linq;

    public class EDIXmlMixedContainer : EDIXmlContainer 
    { 
        protected EDIXmlSegment _header;
        protected EDIXmlSegment _footer; 

        public EDIXmlMixedContainer(string name):base(name)
        {
            
        }

        public void AddSegment(EDIXmlSegment seg)
        { 
            base.Add(seg);
        }

        public void AddLoop(EDIXmlMixedContainer loop)
        {  
            base.Add(loop);
        }

        public void AddTransactionSet(EDIXmlTransactionSet ts)
        {
            if (Label != SegmentLabel.GroupLabel.Text)
                throw new Exception("Cannot add a transaction set to this object!"); 
            base.Add(ts);
        }

        public void AddFunctionGroup(EDIXmlFunctionGroup grp)
        {
            if (Label != SegmentLabel.InterchangeLabel.Text)
                throw new Exception("Cannot add a function group to this object!"); 
            base.Add(grp);
        }
 
        public new void Add(object obj)
        {
            throw new NotImplementedException("The 'Add' method is hidden at this level!");
        }

 
        public int Count(string name)
        {
            var lst = Descendants().Where(e => e.Name.LocalName == name);
            return lst.Count();
        }


    }
}