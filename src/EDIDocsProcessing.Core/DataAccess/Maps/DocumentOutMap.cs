using EDIDocsProcessing.Common.DTOs;
using FluentNHibernate.Mapping;

namespace EDIDocsProcessing.Core.DataAccess.Maps
{
    public class DocumentOutMap : ClassMap<DocumentEntity>
    {
        public DocumentOutMap()
        {
            Table("EDIDocsOut"); 
            Id(x => x.ControlNumber).GeneratedBy.Identity(); 
            Map(x => x.DocumentID);
            Map(x => x.ERPID);
            References(x => x.ISAEntity); 
        }
    }
}