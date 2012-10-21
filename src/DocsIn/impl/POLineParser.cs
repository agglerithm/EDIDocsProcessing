using System.Collections.Generic;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsIn.impl; 
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsIn.impl
{
    public class POLineParser : IPOLineParser
    {
        protected List<Segment> _current_line = new List<Segment>();

        #region IPOLineParser Members

        public virtual void ProcessLines(List<Segment> lst, IEdiMessage doc)
        {
            SegmentCount = 0;
            _current_line.Clear();
        }

        public int SegmentCount { get; protected set; }

        #endregion

        public CustomerOrderLine CreateLine(Segment line_seg)
        {
            if (line_seg.Label != "PO1") return null;
            SegmentCount++;
            string el_delimiter = line_seg.Contents.Substring(3, 1);
            List<string> arr = line_seg.GetElements(el_delimiter);
            var line = new CustomerOrderLine
                           {
                               LineNumber = arr[1].CastToInt(),
                               RequestedQuantity = arr[2].CastToInt(),
                               RequestedPrice = arr[4].CastToDouble()
                           };
            for (int i = 6; i < arr.Count - 1; i++)
            {
                if (arr[i] == "IN")
                    line.CustomerPartNumber = arr[i + 1];
                if (arr[i] == "PD")
                    line.ItemDescription = arr[i + 1];
                if (arr[i] == "VN")
                    line.ItemID = arr[i + 1];
            }
            return line;
        }
    }
}