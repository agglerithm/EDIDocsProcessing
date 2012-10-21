using System;

namespace EdiMessages
{
    [Serializable]
    public class CustomerAliases
    {
        public CustomerAliases()
        {
            
        }
        public CustomerAliases(string legacyCustid, 
            string sytelineCustid, 
            string amtechCustid)
        {
            LegacyCustomerID = legacyCustid;
            SytelineCustomerID = sytelineCustid;
            AmtechCustomerID = amtechCustid;
        }

        public string CustIdByErp(string erp)
        {
             return  erp.ToLower() == "legacy" ?
             LegacyCustomerID:
                erp.ToLower() == "syteline" ? 
                SytelineCustomerID : AmtechCustomerID;
        }
        public string LegacyCustomerID
        {
            get; set;
        }

        public string SytelineCustomerID
        {
            get; set;
        }

        public string AmtechCustomerID
        {
            get; set;
        }

        public override string ToString()
        {
            return string.Format("LegacyCustomerID: {0}, SytelineCustomerID: {1}, AmtechCustomerID: {2}", LegacyCustomerID, SytelineCustomerID, AmtechCustomerID);
        }
    }
}