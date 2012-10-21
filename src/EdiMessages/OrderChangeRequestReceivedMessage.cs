namespace EdiMessages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AFPST.Common.Enumerations;
    using AFPST.Common.Structures;

    public class OrderChangeRequestReceivedMessage : IEdiMessageWithAddress
    {
        public OrderChangeRequestReceivedMessage()
        {
            DocumentId = 860;
            PhoneNumber = "Not Needed"; 
            Customer = new Customer();
            Notes = new List<string>();
            LineItems = new List<CustomerOrderChangeLine>();
        }
        public int LineCount
        {
            get
            {
                return LineItems == null ? 0 : LineItems.Count();
            }
        }
        public List<string> Notes
        {
            get; private set;
        }

        public Customer Customer
        {
            get; set;
        }

        public string PhoneNumber
        {
            get; set;
        }

        public int DocumentId
        {
            get; set;
        }

        public string ControlNumber
        {
            get; set;
        }

        public string BusinessPartnerCode
        {
            get; set;
        }

        public string BusinessPurpose
        {
            get { return string.Empty; }
        }

        public int BusinessPartnerNumber
        {
            get; set;
        }

        public DateTime ChangeRequestDate
        {
            get; set;
        }

        public   IList <CustomerOrderChangeLine> LineItems
        { get; private set; }

        public string BusinessProcessName
        {
            get;
            set;
        }

        public string CustomerBankDescription
        {
            get;
            set;
        }

        public string GeographicLocation
        {
            get;
            set;
        }

        public string SpecificLocationNumber
        {
            get;
            set;
        }

        public void AddAddress(Address address)
        {
            if (string.IsNullOrEmpty(address.AddressType))
                throw new ArgumentException("Address type is not specified");

            if (address.IsShipToAddress())
            {
                ShipToAddress = address;
                return;
            }
            if (address.IsBillToAddress())
            {
                Customer.BillToAddress = address;
                return;
            }

            throw new ArgumentException(string.Format("Address type {0} is not useful here", address.AddressType));
        }

        public Address ShipToAddress
        {
            get; set;
        }

        public string CustomerPO
        {
            get;
            set;
        }

        public string RequestDate
        {
            get;
            set;
        }


        public Address GetBillToAddress()
        {
            return Customer.BillToAddress;
        }

        public void Add(CustomerOrderChangeLine line)
        {
            LineItems.Add(line);
        }
    }
}