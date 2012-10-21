using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDIDocsProcessing.Core.DataAccess.Entities;
using FluentNHibernate.Mapping;

namespace EDIDocsProcessing.Core.DataAccess.Maps
{
    public class DocumentMap : ClassMap<DocumentEntity>
    {
        public DocumentMap()
        {
            WithTable("EDIDocsOut");
            Id(x => x.ControlNumber).GeneratedBy.Identity(); 
            Map(x => x.DocumentID);
            Map(x => x.ERPID);
            References(x => x.ISAEntity); 
        }
    }
}