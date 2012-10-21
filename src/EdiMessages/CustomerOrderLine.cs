using System;
using AFPST.Common.Structures;

namespace EdiMessages
{
    [Serializable]
    public class CustomerOrderLine 
    {
        public Guid CorrelationId
        {
            get; set;
        }

        public string ItemDescription
        {
            get; set;
        }

        public string ItemId
        {
            get;
            set;
        }

        public int OrderMultiple
        {
            get;
            set;
        }

        public string CustomerPartNumber
        {
            get;
            set;
        }

        public int RequestedQuantity
        {
            get;
            set;
        }

        public decimal RequestedPrice
        {
            get;
            set;
        }

        public string Notes
        {
            get;
            set;
        }

        public string CustomerPO
        {
            get;
            set;
        }

        public CustomerAliases CustomerIDs
        {
            get;
            set;
        }

        public bool TestMode
        {
            get;
            set;
        }

        public string RequestNumber
        {
            get;
            set;
        }

        public string OrderNumber
        {
            get; set;
        }

        public int LineNumber
        {
            get;
            set;
        }


        public override string ToString()
        {
            return string.Format("{13}[{14}]{13}  ItemDescription: {0}{13} ItemID: {1}{13} OrderMultiple: {2}{13} CustomerPartNumber: {3}{13} RequestedQuantity: {4}{13} RequestedPrice: {5}{13} Notes: {6}{13} CustomerPO: {7}{13} CustID: {8}{13} TestMode: {9}{13} RequestNumber: {10}{13} OrderNumber: {11}{13} LineNumber: {12}{13}", ItemDescription, ItemId, OrderMultiple, CustomerPartNumber, RequestedQuantity, RequestedPrice, Notes, CustomerPO, CustomerIDs, TestMode, RequestNumber, OrderNumber, LineNumber, Environment.NewLine,GetType().Name);
        }
    }

    public class CustomerOrderChangeLine : CustomerOrderLine
    {
        public string ChangeCode { get; set; }
        public int QtyLeftToReceive { get; set; }
    }
}