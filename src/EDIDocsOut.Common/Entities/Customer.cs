using System.Collections.Generic;
using EDIDocsOut.Common.Entities;

namespace EDIDocsOut.Common.Entities
{
    public class Customer 
    {
        public void AddAddress(Address addr)
        {
            test_address();
            Addresses.Add(addr);
        }

        private void test_address()
        {
            if(Addresses == null)
                Addresses = new List<Address>();
        }

 

        public string CustomerID
        {
            get; set;
        }

        public string CustomerName
        {
            get; set;
        }

        public Address ShipToAddress
        {
            get; set;
        }

        public List<Address> Addresses
        {
            get; private set;
        }
    }
}