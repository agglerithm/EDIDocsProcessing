using EDIDocsProcessing.Core.DataAccess.Entities;
using FluentNHibernate.Mapping;

namespace EDIDocsProcessing.Core.DataAccess.Maps
{
    public class ISAMap :  ClassMap<ISAEntity>
    {
        public ISAMap()
        {
            WithTable("EDIISAOut");
            Id(x => x.ControlNumber).GeneratedBy.Identity();
            Map(x => x.PartnerID);
            Map(x => x.DateSent);
            Map(x => x.GroupID); 
            HasMany(x => x.Documents).Cascade.All().Inverse();
        }
    }
}