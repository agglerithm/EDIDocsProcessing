using System.Collections.Generic;
using EDIDocsOut.Common.DataAccess.Entities;

namespace EDIDocsOut.Common.DataAccess.Entities
{
    public class GroupEntity
    {
        public GroupEntity()
        {
            Documents = new List<DocumentEntity>();
        }

        public virtual int ISAControlNumber
        {
            get; set;
        }

        public virtual int ControlNumber
        {
            get; set;
        }

        public virtual string GroupID
        {
            get; set;
        }

        public virtual IList<DocumentEntity> Documents
        {
            get; set;
        }
    }
}