using AFPST.Common.Enumerations;

namespace EDIDocsProcessing.Common.Enumerations
{
    public class Qualifier : EnumerationOfString<Qualifier>
    {
        public static Qualifier InvoiceVendorPart = new Qualifier("InvoiceVendorPart", "VP");
        public static Qualifier VendorItemNumber = new Qualifier("VendorItemNumber","VN");
        public static Qualifier BuyerItemNumber = new Qualifier("BuyerItemNumber", "IN");
        public static Qualifier BuyerPartNumber = new Qualifier("BuyerPartNumber", "BP");
        public static Qualifier PONumber = new Qualifier("PONumber", "PO");
        public static Qualifier POLineNumber = new Qualifier("POLineNumber","PL");
        public static Qualifier PartDescription = new Qualifier("PartDescription","PD");
        public static Qualifier TelephoneNumber = new Qualifier("TelephoneNumber", "TE");
        public static Qualifier MutuallyAssignedCode = new Qualifier("MutuallyAssignedCode","ZZ");
        public static Qualifier AirBillNumber = new Qualifier("AirbillNumber", "AW");
        public static Qualifier CostCenter = new Qualifier("CostCenter","BF");
        public static Qualifier AssignedByBuyer = new Qualifier("AssignedByBuyer", "92");
        public static Qualifier ProductType = new Qualifier("ProductType","TP");
        public static Qualifier EmptyQualifier = new Qualifier("EmptyQualifier", "");
        public static Qualifier ServiceLevelNumber = new Qualifier("ServiceLevelNumber", "XE");
        public static Qualifier ManufacturerPart = new Qualifier("ManufacturerPart", "MG");

        private Qualifier(string displayName, string val):base(val, displayName)
        { 

        }

        public Qualifier()
        { 
        }
    }
}
