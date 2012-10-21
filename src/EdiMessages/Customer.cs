using System;

namespace EdiMessages
{
    [Serializable]
    public class Customer
    {
        public Customer()
        {
            Terms = new TermsOfSale {NetDays = 30};
            CustomerIDs = new CustomerAliases();
        }

        public CustomerAliases CustomerIDs { get; set; }

        public string Salesperson { get; set; }
        public string CustomerName { get; set; }

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

        public TermsOfSale Terms
        {
            get; set;
        }

        public override string ToString()
        {
            return string.Format("{0}[{5}]{0} CustomerID: {1}{0} CustomerName: {2}{0} BillToAddresses: {3}{0} Terms: {4}{0}", Environment.NewLine, CustomerIDs, CustomerName, BillToAddress, Terms, GetType().Name);
        }
    }
}