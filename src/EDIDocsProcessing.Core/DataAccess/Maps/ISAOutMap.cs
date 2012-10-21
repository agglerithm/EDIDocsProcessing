using EDIDocsProcessing.Common.DTOs;
using FluentNHibernate.Mapping;

namespace EDIDocsProcessing.Core.DataAccess.Maps
{
    public class ISAOutMap :  ClassMap<ISAEntity>
    {
        public ISAOutMap()
        {
            Table("EDIISAOut"); 
            Id(x => x.ControlNumber).GeneratedBy.Identity();  
            Map(x => x.PartnerNumber);
            Map(x => x.DateSent);
            Map(x => x.GroupID); 
            HasMany(x => x.Documents).Cascade.All().Inverse();
        }
    }
}