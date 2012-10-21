namespace EDIDocsProcessing.Common.DTOs
{
    using System; 

    public class ReceiptAcknowledgement
    {
        public virtual Guid ID { get; set; }
        public virtual int DocumentType { get; set; }
        public virtual int DocumentControlNumber { get; set; }
        public virtual DateTime? DocumentSendDate { get; set; }
        public virtual int AckControlNumber { get; set; }
        public virtual DateTime? AckReceiveDate { get; set; }
        public virtual int PartnerId { get; set; } 
    }
}