using System;

namespace EdiMessages
{
    public class StatusConstants
    {
        public const string Accepted = "IA";
        public const string AcceptedWithChanges = "IC";
        public const string Rejected = "IR";
    }

    public class AckTypeConstants
    {
        public const string DetailAndChange = "AC";
        public const string DetailAndNoChange = "AD";
        public const string Rejected = "RJ";
    }

    public class ReferenceConstants
    {
        public const string VendorOrderNumber = "EU";
    }

    public class EdiStructureNameConstants
    {
        public const string Segment = "ediSegment";
        public const string Element = "ediElement";
        public const string FunctionGroup = "ediFunctionGroup";
        public const string InterchangeControl = "ediInterchangeControl";
        public const string TransactionSet = "ediTransactionSet";
        public const string Loop = "ediLoop";
        public const string Label = "ediLabel";
    }
    public class AddressTypeConstants
    {
        public const string ShipTo = "ST";
        public const string BillTo = "BT";
        public const string RemittanceReceiver = "RE";
        public const string SellingParty = "SE";
        public const string ShipFrom = "SF";

        public const string Vendor = "VN";
    }

    public class NameCodeConstants
    {
        public const string Vendor = "VN";
        public const string AssignedByBuyer = "92";
    }

    public class DateTypeConstants
    {
        public const string DeliveryRequestedOn = "002";
        public const string EstimatedDeliveryOn = "017";
        public const string EstimatedShippedOn = "139";
        public const string Shipped = "011";
    }

    public class UnitOfMeasureConstants
    {
        public const string Each = "EA";
    }

    public class ItemReferenceConstants
    {
        public const string CustomerPartNumberCode = "IN";
        public const string ItemIDCode = "VN";
        public const string ItemDescriptionCode = "PD";
        public const string ProductTypeCode = "TP";
    }

    public class InvoiceTransactionTypes
    {
        public const string CreditMemo = "CR";
        public const string DebitMemo = "DR";
    }

    public class BOLQualifiers
    {
        public const string PackingListNumber = "PK";
        public const string VendorOrderNumber = "VN";
    }

    public class BillingQualifiers
    {
        public const string BillingCenter = "BF";
        public const string Department = "19";
    }
}