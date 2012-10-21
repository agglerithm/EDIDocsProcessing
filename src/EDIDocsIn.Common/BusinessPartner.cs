using System;
using System.Linq;
using AFPST.Common.Enumerations;

namespace EDIDocsProcessing.Common
{
    public enum TransmissionPath {Edict, Initech }
    public class BusinessPartner : EnumerationOfInteger
    {
        private readonly string _receiverId;
        public static readonly BusinessPartner Initech = new BusinessPartner(1025, "Initech", "5801469", "055001924VAT   ");
        public static readonly BusinessPartner VandalayIndustries = new BusinessPartner(1026, "Vandalay", "5801469", "055001924V     "); 

        private BusinessPartner(int partnerNumber, string partnerID, string vendorId, string receiverId):base(partnerNumber,partnerID)
        {
            _receiverId = receiverId;
            VendorIdOfAfp = vendorId;
        }

        public string VendorIdOfAfp
        {
            get; private set;
        }
        public BusinessPartner() { }

        public int Number
        {
            get { return Value; }
        }

        public string Code
        {
            get { return base.Text; }
        }
         
        public static BusinessPartner FromReceiverId(string value)
        {
            var matchingItem = GetAll<BusinessPartner>().FirstOrDefault(item => item._receiverId == value);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid receiver ID in BusinessPartner", value);
                throw new ApplicationException(message);
            }
            return matchingItem;
        }
    }
}
