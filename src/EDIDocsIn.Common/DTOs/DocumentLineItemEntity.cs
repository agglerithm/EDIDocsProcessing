using System;
using System.Collections.Generic;
using System.Linq;
using EDIDocsProcessing.Common.Extensions;

namespace EDIDocsProcessing.Common.DTOs
{
    public class DocumentLineItemEntity
    {
        private IList<LineResponseElementEntity> _reList;

        public virtual string LineIdentifier
        {
            get; set;
        }

        public virtual IList<LineResponseElementEntity> ResponseElements
        {
            get
            {
                if (_reList == null) _reList = new List<LineResponseElementEntity>();
                return _reList;
            }
            set
            {
                _reList = value;
                _reList.ForEach(l =>
                                    {
                                        if(l.LineItem == null)  
                                            l.LineItem = this;
                                    });
            }
        }

        public virtual IList<LineResponseElementEntity> GetResponseElementsMatching(string elementName)
        {
            return ResponseElements.Where(r => r.ElementName == elementName).ToList();
        }

//        public virtual void AddResponseElement(string name, string value, string qualifier)
//        {
//            ResponseElements.Add(new LineResponseElementEntity() 
//            { LineItem = this, ElementName = name, Value = value, Qualifier = qualifier });
//        }

        public virtual Guid ID
        {
            get;
            set;
        }

        public virtual DocumentInDTO DocumentInDTO
        {
            get;
            set;
        }

 
    }
}
