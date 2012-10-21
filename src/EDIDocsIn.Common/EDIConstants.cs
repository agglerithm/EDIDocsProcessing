using System;
using AFPST.Common.Infrastructure; 

namespace EDIDocsProcessing.Common
{
 
        
    [CoverageExclude("Just constants", "John")]
    public class GroupTypeConstants
    {
        public const string Invoice = "IN";
        public const string PurchaseOrder = "PO";
        public const string POAcknowledgement = "PR";
        public const string AdvanceShipNotice = "SH";
        public const string Inventory = "IB";

    }
        [CoverageExclude("Just constants", "John")]
    public class EDIDateQualifiers
        {
            public const string Shipped = "011";
        }

    [CoverageExclude("Just constants", "John")]
    public class EDI850Constants
    {
        public const string BeginLabel = "BEG";
        public const string LineItemLabel = "PO1";
    }
}