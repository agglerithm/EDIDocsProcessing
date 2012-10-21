using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.EDIStructures;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsIn.impl
{
    using Common;

    public interface IGeneric850Parser 
    {
        bool CanProcess(Segment header); 
        IEnumerable<ResponseElementEntity> ResponseValues { get; }
        IEnumerable<DocumentLineItemEntity> Lines { get; }
        string ElementDelimiter { get; }
        string SegmentDelimiter { get; }
        OrderRequestReceivedMessage ProcessSegmentList(List<Segment> segList);
        IEnumerable<Segment> MemorizeOuterReferences(List<Segment> segList, string controlNumber, BusinessPartner partner);
        void MemorizeInnerReferences(IList<DocumentLineItemEntity> refs, string controlNumber, BusinessPartner partner);
    }

    public class Generic850Parser : IGeneric850Parser
    { 
        private readonly IGenericDocumentParser  _docParser;

        public Generic850Parser(IGenericDocumentParser docParser) 
            
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

        public OrderRequestReceivedMessage ProcessSegmentList(List<Segment> segList) 
        {
            _docParser.CanProcess(segList[0]);
            validate_second_segment_is_BEG(segList);
            return new OrderRequestReceivedMessage { ControlNumber = get_control_number(segList)  };
        }

        public IEnumerable<Segment> MemorizeOuterReferences(List<Segment> segList, string controlNumber, BusinessPartner partner)
        {
            return  _docParser.MemorizeOuterReferences(segList, controlNumber, partner);
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
            if (segList[1].Contents.StartsWith("BEG")) return;
            throw new Invalid850Exception("BEG Segment is missing!");
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

 

    [Serializable]
    public class InvalidEDIDocumentException : Exception
    {
        public InvalidEDIDocumentException(string message)
            : base(message)
        {
        }
    }

    [Serializable]
    public class  EDIWorkflowException : Exception
    {
        public EDIWorkflowException(string message)
            : base(message)
        {
        }
    }
    [Serializable]
    public class Invalid860Exception : Exception
    {
        public Invalid860Exception(string message)
            : base(message)
        {
        }
    }
    [Serializable]
    public class Invalid850Exception : Exception
    {
       
        public Invalid850Exception(string message) : base(message)
        {
        }

        public Invalid850Exception(string message, Exception innerException): base(message, innerException)
        {
        }


        protected Invalid850Exception(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }
    }
}