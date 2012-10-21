using EDIDocsProcessing.Common.DTOs;
using FluentNHibernate.Mapping;

namespace EDIDocsProcessing.Core.DataAccess.Maps
{
    public class DocumentLineItemMap : ClassMap<DocumentLineItemEntity>
    {
        public DocumentLineItemMap()
        {
            Table("DocumentLines");
            Id(x => x.ID).GeneratedBy.Guid();
            Map(x => x.LineIdentifier); 
            HasMany(x => x.ResponseElements).Cascade.All();
            //References(x => x.DocumentInDTO);
        }
    }
}
