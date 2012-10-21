using EDIDocsProcessing.Common.DTOs;

namespace EDIDocsProcessing.Core.DocsOut
{
    public interface IControlNumberRepository
    {
        ISAEntity GetNextISA(string group_id, int partner_id);
        DocumentEntity GetNextDocument(ISAEntity isa, int docType);
        DocumentEntity GetNextDocument(ISAEntity isa, int docType, string bol);
        void Save(ISAEntity isa);
    }
}