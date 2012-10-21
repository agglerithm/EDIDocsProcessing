namespace EDIDocsProcessing.Core.DataAccess.Maps
{
    using Common.DTOs;
    using FluentNHibernate.Mapping;

    public class ReceiptAcknowledgementMap : ClassMap<ReceiptAcknowledgement>
    {
        public ReceiptAcknowledgementMap()
        {
            Table("ReceiptAcknowledgements");
            Id(x => x.ID).GeneratedBy.Guid();
            Map(x => x.AckControlNumber);
            Map(x => x.AckReceiveDate);
            Map(x => x.DocumentControlNumber);
            Map(x => x.DocumentSendDate);
            Map(x => x.DocumentType);
            Map(x => x.PartnerId);
        }
    }
}