using System;
using System.Collections.Generic;
using AFPST.Common.Enumerations;
using MassTransit;

namespace EdiMessages
{
    [Serializable]
    public class ShippedLine : CorrelatedBy<Guid>
    {
        public ShippedLine()
        {
            TrackingNumbers = new List<string>();
        }
        public DateTime DateShipped { get; set; }

        public string LineNumber { get; set; }

        public string CustomerPartNo { get; set; }

        public string ItemID { get; set; }

        public int QtyShipped { get; set; }

        public int QtyShippedToDate { get; set; }

        public int QtyOrdered { get; set; }

        public string UOM { get; set; }

        public string GetStatus()
        {
            if (QtyShipped == 0) return "IW";
            if (QtyOrdered > QtyShipped) return "BP";
            return "AC";
        }

        public decimal Price { get; set; }

        public IList<string> TrackingNumbers { get; set; }

        public override string ToString()
        {
            return string.Format("DateShipped: {0}, LineNumber: {1}, CustomerPartNo: {2}, ItemID: {3}, QtyShipped: {4}, QtyShippedToDate: {5}, QtyOrdered: {6}, UOM: {7}, Price: {8}, TrackingNumbers: {9}, CorrelationId: {10}", DateShipped, LineNumber, CustomerPartNo, ItemID, QtyShipped, QtyShippedToDate, QtyOrdered, UOM, Price, TrackingNumbers.PrintAll(), CorrelationId);
        }

        public Guid CorrelationId
        {
            get; set;
        }
    }
}