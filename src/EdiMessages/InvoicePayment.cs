using System;

namespace EdiMessages
{

    [Serializable]
    public class InvoicePayment
    {
        public string InvoiceNumber
        {
            get; set;
        }

        public string OrderNumber
        {
            get; set;
        }

        public decimal Amount
        {
            get; set;
        }

        public decimal Discount
        {
            get; set;
        }

        public DateTime InvoiceDate
        {
            get; set;
        }

        public string CustomerName
        {
            get; set;
        }

        public string CustomerID
        {
            get; set;
        }

        public string Location
        {
            get; set;
        }

        public override string ToString()
        {
            return string.Format(@"InvoiceNumber: {0}, OrderNumber: {1}, Amount: {2}, 
                Discount {7}, InvoiceDate: {3}, CustomerName: {4}, CustomerID: {5}, Location: {6}", 
                                                                                  InvoiceNumber, 
                                                                                  OrderNumber, 
                                                                                  Amount, 
                                                                                  InvoiceDate, 
                                                                                  CustomerName, 
                                                                                  CustomerID, 
                                                                                  Location, 
                                                                                  Discount);
        }
    }
}