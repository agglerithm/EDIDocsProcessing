using System;
using System.Collections.Generic;
using System.Linq;
using AFPST.Common.Enumerations;
using AFPST.Common.Structures;

namespace EdiMessages
{
    [Serializable]
    public class InvoicedOrderMessage : IEdiMessageWithAddress
    { 

        public InvoicedOrderMessage()
        {
            LineItems = new List<InvoicedOrderLine>();
            DocumentId = 810;
        }

        public string GlobalCustomerName
        {
            get; set;
        }
        public string RequestDate { get; set; }

        public string CustomerPO { get; set; }

        public string OrderType { get; set; }

        public string Notes { get; set; }

        public string Notes2 { get; set; }

        public Customer Customer { get; set; }


        public CustomerAliases GetCustomerId()
        {
            return Customer != null ? Customer.CustomerIDs : null;
        }

        public string FileID { get; set; }

        public List<InvoicedOrderLine> LineItems { get; set; }

        public string Location { get; set; }

        public DateTime InvoiceDate { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime PODate { get; set; }

        public string TransactionType { get; set; }

        public string CurrencyCode { get; set; }

        public string BOL { get; set; }

        public string ControlNumber { get; set; }

        public string BusinessPartnerCode { get; set; }

        public string BusinessPurpose
        {
            get { return string.Empty; }
        }

        public int BusinessPartnerNumber
        {
            get;
            set;
        }

        public int DocumentId
        {
            get; set;
        }

        private Address _billToAddress;
        public Address BillToAddress
        {
            get { return _billToAddress; }
            set
            {
                if (value.IsBillToAddress() == false)
                    throw new Exception("Expecting a Bill To Address");
                _billToAddress = value;
            }
        }

        private Address _shipToAddress;
        private Address _shipFromAddress;

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

            if (address.IsShipFromAddress() || address.AddressType == AddressTypeConstants.Vendor)
            {
                ShipFromAddress = address;
                return;
            }
            
            throw new ArgumentException(string.Format("Address type {0} is not useful here", address.AddressType));
        }

        public Address ShipFromAddress
        {
            get { return _shipFromAddress; }
            set
            {
                if (value.IsShipFromAddress() == false && value.AddressType != AddressTypeConstants.Vendor)
                    throw new Exception("Expecting a Ship From Address");
                _shipFromAddress = value;
            }
        }

        public void Add(InvoicedOrderLine line)
        {
            LineItems.Add(line);
        }

        public int LineCount()
        {
            return LineItems.Count();
        }

        public decimal GetTotal()
        {
            decimal returnVal = 0;
            foreach (InvoicedOrderLine line in LineItems)
            {
                returnVal += line.TotalSales();
            }
            return returnVal + SalesTax;
        }

        public decimal SalesTax
        {
            get; set;
        }
        public override string ToString()
        {
            return
                string.Format(
                    "{18}[{19}]{18} Addresses: {0}{18} RequestDate: {1}{18} CustomerPO: {2}{18} OrderType: {3}{18} Notes: {4}{18} Notes2: {5}{18} Customer: {6}{18} FileID: {7}{18} LineItems: {8}{18} Location: {9}{18} InvoiceDate: {10}{18} InvoiceNumber: {11}{18} PODate: {12}{18} TransactionType: {13}{18} CurrencyCode: {14}{18} BOL: {15}{18} ControlNumber: {16}{18} BusinessPartnerCode: {17}{18}",
                    ShipToAddress, RequestDate, CustomerPO, OrderType, Notes, Notes2, Customer, FileID, LineItems,
                    Location, InvoiceDate, InvoiceNumber, PODate, TransactionType, CurrencyCode, BOL, ControlNumber,
                    BusinessPartnerCode, Environment.NewLine, GetType().Name);
        }

        public Address GetBillToAddress()
        {
            return Customer.BillToAddress;
        }
    }
}