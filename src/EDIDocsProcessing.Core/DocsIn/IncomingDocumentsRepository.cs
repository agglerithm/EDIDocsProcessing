using System;
using System.Collections.Generic;
using System.Linq;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Core.DataAccess;
using FluentNHibernate;

namespace EDIDocsProcessing.Core.DocsIn
{
    public interface IIncomingDocumentsRepository
    {
        void Save(DocumentInDTO doc);
        DocumentInDTO GetByISAControlNumberAndPartnerID(int isaControlNum, int partnerID);
        DocumentInDTO GetByDocumentControlNumberAndPartnerID(int docControlNum, int partnerID);
        void Delete(DocumentInDTO info);
        IList<DocumentInDTO> GetAll();
    }

    public class IncomingDocumentsRepository : Repository, IIncomingDocumentsRepository 
    {
        public IncomingDocumentsRepository(ISessionSource sessionSource)
            : base(sessionSource) 
        {  
        }

        public IList<DocumentInDTO> GetAll()
        {
            return  GetAll<DocumentInDTO>().ToList();
        }

        public DocumentInDTO GetByISAControlNumberAndPartnerID(int isaControlNum, int partnerID)
        {
            return FindBy<DocumentInDTO>(doc => doc.ISAControlNumber == isaControlNum
                && doc.PartnerNumber == partnerID);
        }

        public DocumentInDTO GetByDocumentControlNumberAndPartnerID(int docControlNum, int partnerID)
        {
            try
            {

                return FindBy<DocumentInDTO>(
                    doc => doc.PartnerNumber == partnerID && 
                        doc.ControlNumber == docControlNum); 
            }
            catch (Exception ex)
            {
                throw new Exception("Error trying to find control number '" + docControlNum + "' for partnerID '" + partnerID +
                                    "'; message: " + ex.Message);
            }
        }

        public void Delete(DocumentInDTO doc)
        {
            base.Delete(doc);
        }


        public void Save(DocumentInDTO isa)
        {
            base.Save(isa);
          //  isa.Documents.ForEach(save_document);
        }

//        private void save_document(DocumentInDTO doc)
//        {
//            Save(doc);
//            doc.ResponseElements.ForEach(Save);
//        }


    }


    
}

