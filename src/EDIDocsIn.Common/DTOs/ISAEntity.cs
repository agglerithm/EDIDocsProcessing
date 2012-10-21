using System;
using System.Collections.Generic;

namespace EDIDocsProcessing.Common.DTOs
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

        public virtual int PartnerNumber
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

//    public class ISAInDTO  
//    {
//        private int _partnerID;
//        private int _controlNumber;
//
//        public virtual int ControlNumber
//        {
//            get { return _controlNumber; }
//            set
//            {
//                Documents = get_docs();
//                Documents.ForEach(d => d.ISAControlNumber = value);
//                _controlNumber = value;
//            }
//        }
//
//        public virtual Guid ID { get; set; }
//        public virtual IList<DocumentInDTO> Documents
//        {
//            get; set;
//        }
//
//        public virtual int PartnerNumber
//        {
//            get { return _partnerID; }
//            set
//            {
//                Documents = get_docs();
//                Documents.ForEach(d => d.PartnerNumber = value);
//                _partnerID = value;
//            }
//        }
//
//        public virtual DateTime DateSent
//        {
//            get; set;
//        }
//
//        public virtual string GroupID
//        {
//            get;
//            set;
//        }
//
//        public virtual void Add(DocumentInDTO doc)
//        {
//            Documents = get_docs();
//            doc.ISAInDTO = this;
//            doc.ISAControlNumber = ControlNumber;
//            doc.PartnerNumber = PartnerNumber;
//            Documents.Add(doc);
//        }
//
//        private IList<DocumentInDTO> get_docs()
//        {
//            if(Documents == null)
//                Documents = new List<DocumentInDTO>();
//            return Documents;
//        }
//
////        public virtual bool ContainsDocument(int controlNumber)
////        {
////            if (Documents == null) return false;
////            bool found = false;
////            Documents.ForEach(d => { if (d.ControlNumber == controlNumber) found = true; }) ;
////            return found;
////        }
//        public virtual DocumentInDTO GetDocument(int docControlNum)
//        {
//            return Documents.Find(d => d.ControlNumber == docControlNum);
//        }
//    }
}