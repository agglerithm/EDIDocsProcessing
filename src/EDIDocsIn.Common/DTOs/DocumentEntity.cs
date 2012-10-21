using System;
using System.Collections.Generic;

namespace EDIDocsProcessing.Common.DTOs
{
    public class DocumentEntity
    {

        public virtual int ControlNumber
        {
            get;
            set;
        }

        public virtual ISAEntity ISAEntity
        {
            get; set;
        }

        public virtual int DocumentID
        {
            get; set;
        }

        public virtual string ERPID
        {
            get; set;
        }
    }

    public class DocumentInDTO  
    {
        private IList<ResponseElementEntity> _reList;
        private IList<DocumentLineItemEntity> _lineList;
        public virtual Guid ID { get; set; }
        private DateTime _dateSent = DateTime.Today;

        public virtual int ControlNumber
        {
            get; set;
        }

        public virtual int DocumentID
        {
            get;
            set;
        }

        public virtual  string ERPID
        {
            get;
            set;
        }

        public virtual int ISAControlNumber
        {
            get; set;
        }

        public virtual int PartnerNumber
        {
            get;
            set;
        }

        public virtual DateTime DateSent
        {
            get { return _dateSent;
            }
            set{ _dateSent = value;}
        }

        public virtual string GroupID
        {
            get; set;
        }

        public virtual IList<DocumentLineItemEntity> LineItems
        {
            get
            {
                if (_lineList == null) _lineList = new List<DocumentLineItemEntity>();
                    return _lineList;}
            set
            {
                _lineList = value;
            }
        }

        public virtual void AddLineItem(string lineID, IList<LineResponseElementEntity> elements)
        {
            LineItems.Add(new DocumentLineItemEntity()
                              {DocumentInDTO = this, LineIdentifier = lineID, ResponseElements = elements});
        }

        public virtual   IList<ResponseElementEntity> ResponseElements
        {
            get { 
                if(_reList == null) _reList = new List<ResponseElementEntity>();
                    return _reList;
            }
            set
            {
                _reList = value;
            }
        }

        public virtual void AddResponseElement(string name, string value, string qualifier)
        {
            ResponseElements.Add(new ResponseElementEntity() {DocumentInDTO = this, ElementName = name, Value = value, Qualifier = qualifier});
        }
    }
}