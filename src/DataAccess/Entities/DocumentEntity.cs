namespace EDIDocsProcessing.Core.DataAccess.Entities
{
    public class DocumentEntity
    {

        public virtual int ControlNumber
        {
            get;
            set;
        }

        public virtual ISAEntity ISAEntity
        {
            get; set;
        }

        public virtual int DocumentID
        {
            get; set;
        }

        public virtual string ERPID
        {
            get; set;
        }
    }
}