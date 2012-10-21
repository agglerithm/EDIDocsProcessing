using System;
using System.Collections.Generic;
using System.Linq;
using AFPST.Common.Enumerations;
using AFPST.Common.Structures;

namespace EdiMessages
{
    [Serializable]
    public class OrderRequestReceivedMessage : IEdiMessageWithAddress
    {

        public OrderRequestReceivedMessage()
        {
            DocumentId = 850;
            PhoneNumber = "Not Needed"; 
            Customer = new Customer();
            Notes = new List<string>();
        }

        public string TempOrderId { get; set; }

        public string BusinessProcessName { get; set; }

        public string RequestDate { get; set; }

        public string CustomerPO { get; set; }

        public Customer Customer { get; set; }

        private Address _shipToAddress;
        public Address ShipToAddress
        {
            get { return _shipToAddress; }
            set
            {
                if (value.IsShipToAddress() == false)
                    throw new Exception("Expecting a Ship To Address");
                _shipToAddress = value;
            }
        }

        public CustomerAliases CustomerIDs
        {
            get
            {
                return Customer == null ? null : Customer.CustomerIDs;
            }

           
        }

        public string CustomerBankDescription { get; set; }

        public IList<CustomerOrderLine> LineItems { get; set; }

        public int LineCount
        {
            get
            {
                return LineItems == null ? 0 : LineItems.Count();
            }
        }

        public string GeographicLocation { get; set; }

        public string SpecificLocationNumber { get; set; }

        public string PhoneNumber { get; set; }


        public string BusinessPartnerCode { get; set; }

        public string BusinessPurpose
        {
            get { return string.Empty; }
        }

        public int BusinessPartnerNumber
        {
            get; set;
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

        public string ControlNumber { get; set; }

        public int DocumentId { get; set; }

        public ServiceLevel LevelOfService { get; set; }

        public void Add(CustomerOrderLine line)
        {
            line.CustomerIDs = CustomerIDs;
            line.CustomerPO = CustomerPO;

            if (LineItems == null)
                LineItems = new List<CustomerOrderLine>();
            LineItems.Add(line);
        }

        public bool IsBackorder
        {
            get; set;
        }

        public IList<string> Notes
        {
            get; set;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "{0}[{11}]{0} {0} BusinessProcessName: {1}{0} BusinessPartnerCode: {2}{0} CustomerBankDescription: {11}{0} RequestDate: {3}{0} CustomerPO: {4}{0} Customer: {5}{0} "
                    + "ShipToAddress: {14}{0} LineItems: {6}{0} ControlNumber: {7}{0} DocumentID: {8}{0} Location: {12}{0} Warehouse: {9}{0} PhoneNumber: {10}{0} CustomerBankDescription {13}{0}",
                    Environment.NewLine, BusinessProcessName, BusinessPartnerCode, RequestDate, CustomerPO, Customer, 
                    LineItems.PrintAll(), ControlNumber, DocumentId, SpecificLocationNumber, PhoneNumber, GetType().Name, GeographicLocation,
                    CustomerBankDescription, ShipToAddress);
        }

        public Address GetBillToAddress()
        {
            return Customer.BillToAddress;
        }

        public string GetCustomerShipToCode()
        {
            return ShipToAddress.AddressCode.CustomerCode;
        }
    }
}