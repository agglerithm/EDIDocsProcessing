using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDIDocsOut.Common.DataAccess.Entities;
using FluentNHibernate.Mapping;

namespace EDIDocsOut.Common.DataAccess.Maps
{
    public class GroupMap : ClassMap<GroupEntity>
    {
        public GroupMap()
        {
            WithTable("EDIGroupsOut");
            Id(x => x.ControlNumber).GeneratedBy.Identity(); 
            Map(x => x.GroupID);

        }
    }
}
