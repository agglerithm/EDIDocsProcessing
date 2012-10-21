using System;

namespace EdiMessages
{
    public class InvoicedOrderLine 
    {
        public string ItemDescription { get; set; }

        public string ItemID { get; set; }

        public int OrderMultiple { get; set; }

        public string CustomerPartNumber { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string Notes { get; set; }

        public string CustomerPO { get; set; }

        public string CustID { get; set; }

        public string RequestNumber { get; set; }
        
        public int LineNumber { get; set; }

        public int RequestedQuantity { get;  set; }

        public decimal TotalSales()
        {
            return Quantity*decimal.Round(Price,2);
        }


        public override string ToString()
        {
            return string.Format("{12}[{13}]{12} ItemDescription: {0}{12} ItemID: {1}{12} OrderMultiple: {2}{12} CustomerPartNumber: {3}{12} Quantity: {4}{12} Price: {5}{12} Notes: {6}{12} CustomerPO: {7}{12} CustID: {8}{12} RequestNumber: {9}{12} LineNumber: {10}{12} RequestedQuantity: {11}{12}", ItemDescription, ItemID, OrderMultiple, CustomerPartNumber, Quantity, Price, Notes, CustomerPO, CustID, RequestNumber, LineNumber, RequestedQuantity, Environment.NewLine,GetType().Name);
        }
    }
}