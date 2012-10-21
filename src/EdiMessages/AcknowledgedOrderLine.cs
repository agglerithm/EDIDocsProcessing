using System;

namespace EdiMessages
{ 
    [Serializable]
    public class AcknowledgedOrderLine
    {
        public string ItemDescription
        {
            get;
            set;
        }

        public string ItemID
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

        public int ActualQuantity
        {
            get;
            set;
        }

        public double RequestedPrice
        {
            get;
            set;
        }

        public double ActualPrice
        {
            get;
            set;
        }

        public string Notes
        {
            get;
            set;
        }
  



        public string LineNumber
        {
            get;
            set;
        }

        public string StatusCode
        {
            get; set;
        }

        public DateTime EstimatedDeliveryDate
        {
            get; set;
        }
    }
}