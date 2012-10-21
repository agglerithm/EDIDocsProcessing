using System;
using AFPST.Common;

namespace EdiMessages
{
    [Serializable]
    public class Address
    {
        public Address()
        {
            Country = "US"; 
            AddressCode = new AddressCode();
        }

        public string AddressType { get; set; }

        public AddressCode AddressCode { get; set; }

        public string AddressName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        public string ContactName { get; set; }

        public string PhoneNumber { get; set; }

        public string GetAutomationAddressType()
        {
            if (AddressType == "ST") return AFPST.Common.Enumerations.AddressType.ShipTo.Text;
            if (AddressType == "BT") return AFPST.Common.Enumerations.AddressType.BillTo.Text;
            if (AddressType == "SF") return AFPST.Common.Enumerations.AddressType.ShipFrom.Text;
            throw new Exception(string.Format("Unknown Address Type '{0}'",AddressType));
        }

        public override string ToString()
        {
            return string.Format("{0}[Address]{0} AddressType: {1}{0} AddressCode: {2}{0} AddressName: {3}{0} Address1: {4}{0} Address2: {5}{0} City: {6}{0} State: {7}{0} Zip: {8}{0} Country: {9}{0} ContactName: {10}{0} PhoneNumber: {11}{0}", Environment.NewLine, AddressType, AddressCode, AddressName, Address1, Address2, City, State, Zip, Country, ContactName, PhoneNumber);
        }
    }

    [Serializable]
    public class AddressCode
    {
        public string InternalCode
        {
            get; set;
        }
        public string CustomerCode
        {
            get; set;
        }
    }
}