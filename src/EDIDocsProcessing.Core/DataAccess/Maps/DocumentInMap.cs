using EDIDocsProcessing.Common.DTOs;
using FluentNHibernate.Mapping;

namespace EDIDocsProcessing.Core.DataAccess.Maps
{
    public class DocumentInMap : ClassMap<DocumentInDTO>
    {
        public DocumentInMap()
        {
            Table("EDIDocsIn");
            Id(x => x.ID).GeneratedBy.Guid();
            Map(x => x.ControlNumber);
            Map(x => x.DocumentID);
            Map(x => x.ERPID);
            Map(x => x.ISAControlNumber);
            Map(x => x.PartnerNumber);
            Map(x => x.GroupID);
            Map(x => x.DateSent);
            HasMany(x => x.ResponseElements).Cascade.All();
            HasMany(x => x.LineItems).Cascade.All();
        }
    }
}
