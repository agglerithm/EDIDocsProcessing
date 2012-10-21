using System;
using System.Collections.Generic;
using AFPST.Common.Structures;
using EdiMessages.Enumerations;


namespace EdiMessages
{
    [Serializable]
    public class ChangeOrderMessage : IEdiMessageWithAddress 
    {
        public ChangeOrderMessage()
        {
            DocumentId = 860;
            Lines = new List<ChangeOrderLine>();
            Notes = new List<string>();
        }

        public int DocumentId
        {
            get; private set;
        }

        public string PurposeCode
        {
            get; set;
        }

        public string PONumber
        {
            get; set;
        }

        public DateTime PODate
        {
            get; set;
        }

        public DateTime ChangeDate
        {
            get; set;
        }
        public string ControlNumber
        {
            get;
            set;
        }

        public string BusinessPartnerCode
        {
            get;
            set;
        }

        public string BusinessPurpose
        {
            get;
            set;
        }

        public int BusinessPartnerNumber
        {
            get;
            set;
        }

        public void AddAddress(Address address)
        {
            if(address.AddressType == AddressTypeConstants.ShipTo)
            ShipToAddress = address;
        }

        public Address ShipToAddress
        {
            get; private set;
        }

        public IList<string> Notes
        {
            get; set;
        }

        public string BusinessProcessName
        {
            get; set;
        }

        public List<ChangeOrderLine> Lines
        {
            get; set;
        }

        public string GeographicLocation
        {
            get; set;
        }
    }

    [Serializable]
    public class ChangeOrderLine  
    {
        public ChangeType Type
        {
            get; set;
        }

        public int OriginalQuantityOrdered
        {
            get; set;
        }

        public int QuantityLeftToReceive
        {
            get; set;
        }

        public decimal UnitPrice
        {
            get; set;
        }

        public string CustomerPartNumber
        {
            get; set;
        }

        public string LineNumber
        {
            get; set;
        }
    }
}
