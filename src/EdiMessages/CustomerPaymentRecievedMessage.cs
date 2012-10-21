using System;
using System.Collections.Generic;
using System.Linq; 

namespace EdiMessages
{
    [Serializable]
    public class CustomerPaymentRecievedMessage : IEdiMessage
    {
 
        private IList<InvoicePayment> _payments = new List<InvoicePayment>();
        public InvoicePayment[] InvoicePayments
        {
            get
            {
                return _payments.ToArray();
            }
        }

        public decimal CreditAmount { get; set; }

        public string CheckNumber { get; set; }
        
        public DateTime CheckDate { get; set; }

        public string CustomerName { get; set; }
      
 
        public void Add(InvoicePayment pmt)
        {
            if (_payments.Contains(pmt)) return;

            pmt.CustomerName = CustomerName;
            _payments.Add(pmt);
        }

        public decimal GetTotalAmount()
        {
            var value = (decimal) 0.0;
            foreach (var invoicePayment in _payments)
            {
                value += invoicePayment.Amount;
            }
            return value;
        }

        public int DocumentId { get { return 0; } }

        public string ControlNumber { get; set; }

        public string BusinessPartnerCode { get; set; }

        public string BusinessPurpose
        {
            get { return string.Empty; }
        }

        public int BusinessPartnerNumber { get; set; }

    }
}