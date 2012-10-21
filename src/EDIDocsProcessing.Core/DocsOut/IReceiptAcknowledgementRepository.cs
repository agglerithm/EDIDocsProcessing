namespace EDIDocsProcessing.Core.DocsOut
{
    using Common.DTOs;
    using EdiMessages;

    public interface IReceiptAcknowledgementRepository
    {
        ReceiptAcknowledgement GetSentDocument(int controlNum);
        void SetPlaceholder(ReceiptAcknowledgement ack);
        void SaveAcks(Acknowledgements acks);
    }
}