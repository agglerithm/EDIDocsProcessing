using EDIDocsProcessing.Core.DataAccess.DTOs;
using FluentNHibernate.Mapping;

namespace EDIDocsProcessing.Core.DataAccess.Maps
{
    public class ISAInMap : ClassMap<ISAInDTO>
    {
        public ISAInMap()
        {
            WithTable("EDIISAIn");
            Id(x => x.ID).GeneratedBy.GuidComb();
            Map(x => x.ControlNumber);
            Map(x => x.PartnerID);
            Map(x => x.DateSent);
            Map(x => x.GroupID);
            HasMany(x => x.Documents).Cascade.All();
        }
    }
}
