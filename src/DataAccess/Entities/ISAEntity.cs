using System;
using System.Collections.Generic;
using EDIDocsProcessing.Core.DataAccess.Entities;

namespace EDIDocsProcessing.Core.DataAccess.Entities
{
    public class ISAEntity
    {
        public ISAEntity( )
        {
            DateSent = DateTime.Now;
            Documents = new List<DocumentEntity>();
        }

        public virtual int GroupControlNumber
        {
            get {return ControlNumber;  }
        }

        public virtual string GroupID
        {
            get;
            set;
        }

        public virtual IList<DocumentEntity> Documents
        {
            get;
            set;
        }

        public virtual int ControlNumber
        {
            get;
            set;
        }

        public virtual int PartnerID
        {
            get; set;
        }

        public virtual DateTime DateSent
        {
            get; set;
        }
        
        public virtual void Add(DocumentEntity doc)
        {
            doc.ISAEntity = this;
            Documents.Add(doc);
        }
    }
}