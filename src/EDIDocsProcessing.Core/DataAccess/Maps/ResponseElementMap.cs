using EDIDocsProcessing.Common.DTOs;
using FluentNHibernate.Mapping;

namespace EDIDocsProcessing.Core.DataAccess.Maps
{
    public class ResponseElementMap : ClassMap<ResponseElementEntity>
    {
        public ResponseElementMap()
        { 
            Table("ResponseElements");
            Id(x => x.ID).GeneratedBy.Guid(); 
            Map(x => x.ElementName);
            Map(x => x.Qualifier);
            Map(x => x.Value);
            //References(x => x.DocumentInDTO);
        }
    }

    public class LineItemResponseElementMap : ClassMap<LineResponseElementEntity>
    {
        public LineItemResponseElementMap()
        { 
            Table("LineItemResponseElements");
            Id(x => x.ID).GeneratedBy.Guid(); 
            Map(x => x.ElementName);
            Map(x => x.Qualifier);
            Map(x => x.Value);
            //References(x => x.DocumentInDTO);
        } 
    }
}
