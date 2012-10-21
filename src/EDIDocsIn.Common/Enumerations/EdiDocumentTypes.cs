using System;
using AFPST.Common.Enumerations;

namespace EDIDocsProcessing.Common.Enumerations
{
    public class EdiDocumentTypes : EnumerationOfInteger
    {
        public static EdiDocumentTypes Invoice = new EdiDocumentTypes(810,"Invoice");
        public static EdiDocumentTypes PurchaseOrder = new EdiDocumentTypes(850, "Purchase Order");
        public static EdiDocumentTypes PurchaseOrderAck = new EdiDocumentTypes(855, "Purchase Order Ack");
        public static EdiDocumentTypes PurchaseOrderChangeRequest = new EdiDocumentTypes(860, "Purchase Order Change Request");
        public static EdiDocumentTypes AdvanceShipNotice = new EdiDocumentTypes(856, "ASN");
        public static EdiDocumentTypes CashRecipts = new EdiDocumentTypes(820, "Cash Recipts");
        public static EdiDocumentTypes InventoryInquiry = new EdiDocumentTypes(846, "Inventory Inquiry");

        private EdiDocumentTypes(int documentNumber, string documentName)
            : base(documentNumber, documentName) {}

        public EdiDocumentTypes() { }

        public int DocumentNumber
        {
            get { return Value; }
        }

        public string DocumentName
        {
            get { return base.Text; }
        }

 
    }
}