

using System.Collections.Generic;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Core.DocsIn;
using EDIDocsProcessing.Core.DocsIn.impl;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsIn.impl
{
    using Common;

    public class Generic860Parser : IGeneric860Parser
    {
        private readonly IGenericDocumentParser  _docParser;

        public Generic860Parser(IGenericDocumentParser docParser) 
            
        { 
            _docParser = docParser;
        }

        public string ElementDelimiter
        {
            get { return _docParser.ElementDelimiter; }
        }

        public string SegmentDelimiter
        {
            get { return _docParser.SegmentDelimiter; }
        }

        public OrderChangeRequestReceivedMessage ProcessSegmentList(List<Segment> segList) 
        {
            _docParser.CanProcess(segList[0]);
            validate_second_segment_is_BEG(segList);
            return new OrderChangeRequestReceivedMessage { ControlNumber = get_control_number(segList) };
        }

        public IEnumerable<Segment> MemorizeOuterReferences(List<Segment> segList, string controlNumber, BusinessPartner partner)
        {
            return  _docParser.MemorizeOuterReferences(segList, controlNumber,   partner);
        }

        public void MemorizeInnerReferences(IList<DocumentLineItemEntity> refs, string controlNumber, BusinessPartner partner)
        {
            _docParser.MemorizeInnerReferences(refs, controlNumber, partner);
        }


        private string get_control_number(IList<Segment> segList)
        { 
            var arr = segList[0].GetElements(_docParser.ElementDelimiter);
            return arr[2];
        }

        private static void validate_second_segment_is_BEG(IList<Segment> segList)
        {
            if (segList[1].Contents.StartsWith("BCH")) return;
            throw new Invalid860Exception("BCH Segment is missing!");
        }

        public bool CanProcess(Segment header)
        {
            return _docParser.CanProcess(header);
        }

 

        public IEnumerable<ResponseElementEntity> ResponseValues
        {
            get { return _docParser.ResponseValues; }
        }

        public IEnumerable<DocumentLineItemEntity> Lines
        {
            get { return _docParser.Lines; }
        }
    }
}

public class Generic860Parser : GenericDocParser<ChangeOrderMessage> 
{
    protected readonly IPOLineParser _lineParser;
    protected readonly ISegmentExtractor _segmentExtractor;

    protected Generic860Parser(IAddressParser addressParser, IPOLineParser lineParser, 
        IEDIResponseReferenceRecorder recorder,
        ISegmentExtractor segmentExtractor)
        : base(recorder)
    {
        _lineParser = lineParser;
        _segmentExtractor = segmentExtractor;
    }

    private   ChangeOrderMessage process_segment_list(List<Segment> segList)
    {
        return new ChangeOrderMessage { ControlNumber = get_control_number(segList) };
    }

 
    private static void validate_second_segment_is_BCH(IList<Segment> segList)
    {
    }
}
