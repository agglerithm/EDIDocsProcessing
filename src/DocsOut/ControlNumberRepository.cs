using EDIDocsProcessing.Core.DataAccess.Entities;
using FluentNHibernate.Data;
using NHibernate;

namespace EDIDocsProcessing.Core.DocsOut
{
    public interface IControlNumberRepository
    {
        ISAEntity GetNextISA(string group_id, int partner_id);
        DocumentEntity GetNextDocument(ISAEntity isa, int docType);
        void Save(ISAEntity isa);
    }

    public class ControlNumberRepository : Repository, IControlNumberRepository 
    { 
        public ControlNumberRepository(ISession session) 
            : base(session) 
        {  
        }
        public ISAEntity GetNextISA(string group_id, int partner_id)
        {
            var isa = new ISAEntity() {GroupID = group_id, PartnerID = partner_id };

            Save(isa);

            return isa;
        }
       
        public  DocumentEntity GetNextDocument(ISAEntity isa, int docType)
        {
            var doc = new DocumentEntity {ISAEntity = isa, DocumentID = docType};

            Save(doc);

            return doc;

        }

        public void Save(ISAEntity isa)
        {
            base.Save(isa);
        }
    }
}