using System;

namespace EDIDocsProcessing.Common.DTOs
{
    public class ResponseElementEntity
    {

        public virtual string Qualifier
        {
            get; set;
        }

        public virtual string ElementName
        {
            get; set;
        }

        public virtual string Value
        {
            get; set;
        }

        public virtual Guid ID
        {
            get; set;
        }

        public virtual DocumentInDTO DocumentInDTO 
        {
            get; set;
        }
    }

    public class LineResponseElementEntity
    {
        public virtual string Qualifier
        {
            get;
            set;
        }

        public virtual string ElementName
        {
            get;
            set;
        }

        public virtual string Value
        {
            get;
            set;
        }

        public virtual Guid ID
        {
            get;
            set;
        }

        public virtual DocumentLineItemEntity LineItem
        {
            get; set;
        }
    }
}
