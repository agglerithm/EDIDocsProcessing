namespace EdiMessages
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class ReceiptAcknowledgementMsg
    {
        public string ControlNumber { get; set; }
        public int DocumentId { get; set; }
        public DateTime SendDate { get; set; }
    }

    [Serializable]
    public class Acknowledgements : IEdiMessage
    {
        public int DocumentId
        {
            get { return 997; }
        }

        public string ControlNumber { get; set; }

        public string BusinessPartnerCode
        {
            get; set;
        }

        public string BusinessPurpose
        {
            get; set;
        }

        public int BusinessPartnerNumber
        {
            get; set;
        }
         
        public List<ReceiptAcknowledgementMsg> Acks { get; set; }
    }
}