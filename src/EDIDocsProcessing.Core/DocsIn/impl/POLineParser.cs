using System;
using System.Collections.Generic;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Common.Extensions;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsIn.impl
{
    public class POLineParser : IPOLineParser
    {
        private readonly List<Segment> _currentLine = new List<Segment>();
        protected string _elDelimiter;

        #region IPOLineParser Members

        public virtual IList<DocumentLineItemEntity> ProcessLines(List<Segment> lst, IEdiMessage doc)
        {
            SegmentCount = 0;
            _currentLine.Clear();
            return null;
        }

        public int SegmentCount { get; set; }
 

        #endregion

        public CustomerOrderLine CreateLine(Segment lineSeg)
        {
            if (lineSeg.Label == SegmentLabel.PurchaseOrderChange)
                return createChangeLine(lineSeg);
            if (lineSeg.Label != SegmentLabel.PurchaseOrder) return null;
            SegmentCount++;
            string[] arr = GetEls(lineSeg);
            var line = new CustomerOrderLine
                           {
                               LineNumber = arr[1].CastToInt(),
                               RequestedQuantity = arr[2].CastToInt(),
                               RequestedPrice = arr[4].CastToDecimal()
                           };
            for (int i = 6; i < arr.Length - 1; i++)
            {
                if (arr[i] == "IN" || arr[i] == "BP")
                {
                    line.CustomerPartNumber = arr[i + 1]; 
                }
                if (arr[i] == "PD")
                    line.ItemDescription = arr[i + 1];
                if (arr[i] == "VN" || arr[i] == "VP")
                    line.ItemId = arr[i + 1];
            }
            return line;
        }

        private CustomerOrderChangeLine createChangeLine(Segment lineSeg)
        {
            SegmentCount++;
            string[] arr = GetEls(lineSeg);
            var line = new CustomerOrderChangeLine
            {
                LineNumber = arr[1].CastToInt(),
                RequestedQuantity = arr[3].CastToInt(),
                QtyLeftToReceive = arr[4].CastToInt(),
                RequestedPrice = arr[6].CastToDecimal(),
                ChangeCode = arr[2]
            };
            for (int i = 8; i < arr.Length - 1; i++)
            {
                if (arr[i] == "IN" || arr[i] == "BP")
                {
                    line.CustomerPartNumber = arr[i + 1];
                }
                if (arr[i] == "PD")
                    line.ItemDescription = arr[i + 1];
                if (arr[i] == "VN" || arr[i] == "VP")
                    line.ItemId = arr[i + 1];
            }
            return line;
        }

        private string[] GetEls(Segment lineSeg)
        {
            _elDelimiter = lineSeg.Contents.Substring(3, 1);
            return lineSeg.GetElements(_elDelimiter);
        }
    }
}