using System;
using System.Collections.Generic;
using EDIDocsProcessing.Core.DocsIn.impl;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsIn.impl
{
    public class Generic850Parser : GenericDocParser
    {
        protected readonly IPOLineParser _lineParser;

        public Generic850Parser(IAddressParser addressParser, IPOLineParser lineParser) : base(addressParser, "850")
        {
            _lineParser = lineParser;
        }

        protected override IEdiMessage process_segment_list(List<Segment> seg_list)
        {
            validateSecondSegmentIsBEG(seg_list);
            return new CreateOrderMessage {ControlNumber = get_control_number(seg_list)};
        }

        private string get_control_number(List<Segment> seg_list)
        {
            List<string> arr = seg_list[0].GetElements(ElementDelimiter);
            return arr[2];
        }

        private static void validateSecondSegmentIsBEG(List<Segment> seg_list)
        {
            if (seg_list[1].Contents.StartsWith("BEG")) return;
            throw new Invalid850Exception("BEG Segment is missing!");
        }
    }

    public class Invalid850Exception : Exception
    {
        public Invalid850Exception(string message) : base(message)
        {
        }
    }
}