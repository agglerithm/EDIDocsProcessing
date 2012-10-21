using System;
using System.Collections.Generic;
using EdiMessages;

namespace EdiMessages
{
    [Serializable]
    public class AcknowledgedOrderMessage : IEdiMessage
    {
        public IList<AcknowledgedOrderLine> Lines { get; private set; }

        public AcknowledgedOrderMessage()
        {
            Lines = new List<AcknowledgedOrderLine>(); 
        }
        public string BusinessPartnerCode
        {
            get;
            set;
        }
        public void Add(AcknowledgedOrderLine line)
        { 
            Lines.Add(line);
        }

        public string CustomerPO
        {
            get; set;
        }

        public string BOL
        { 
            get; set;
        }

        public string AckType
        {
            get; set;
        }

        public int DocumentID
        {
            get { return 855; }
        }

        public string ControlNumber
        {
            get; set;
        }

        public void AddAddress(Address address)
        {
            throw new System.NotImplementedException();
        }
    }
}