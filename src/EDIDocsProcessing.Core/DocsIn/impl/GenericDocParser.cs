using System;
using System.Collections.Generic;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EdiMessages;

namespace EDIDocsProcessing.Core.DocsIn.impl
{
    using Common;

    public class GenericDocParser<TMessage>  : IGenericDocumentParser  where TMessage : IEdiMessage 
    { 
        private readonly IEDIResponseReferenceRecorder _recorder;
        private readonly string _docId;


        public GenericDocParser(
            IEDIResponseReferenceRecorder recorder)
        { 
            _recorder = recorder;
        }

        public void SetElementDelimiterFromHeader(Segment header)
        { 
            ElementDelimiter = get_element_delimiter(header);
        }
        public bool CanProcess(Segment header)
        {
            SetElementDelimiterFromHeader(header);
            string[] arr = header.Contents.Split(ElementDelimiter.ToCharArray());
            return arr[1] == _docId;
        }

        public string ElementDelimiter { get; private set; }

        public string SegmentDelimiter { get; set; }

        public void Process(List<Segment> segList)
        {
            ElementDelimiter = get_element_delimiter(segList[0]);
            _recorder.Clear();  
        }

        public IEnumerable<ResponseElementEntity> ResponseValues
        {
            get { return _recorder.GetResponseValues(); }
        }

        public IEnumerable<DocumentLineItemEntity> Lines
        {
            get { return _recorder.GetLines(); }
        }

        public IEnumerable<Segment> MemorizeOuterReferences(List<Segment> segList, string controlNumber, BusinessPartner partner)
        {
            return _recorder.MemorizeOuterReferences(segList, controlNumber, ElementDelimiter,  partner);
        }

        public void MemorizeInnerReferences(IList<DocumentLineItemEntity> refs, string controlNumber, BusinessPartner partner)
        {
            _recorder.MemorizeInnerReferences(refs, controlNumber, partner);
        }

        private static string get_element_delimiter(Segment header)
        {
            return header.Contents.Substring(2, 1);
        }

        protected string get_control_number(IList<Segment> segList)
        {
            if (segList.Count == 0 || segList[0].Label != SegmentLabel.DocumentLabel)
                throw new InvalidEDIDocumentException("There is no document header!");
            var arr = segList[0].GetElements(ElementDelimiter);
            return arr[2];
        }

        protected virtual IEdiMessage process_segment_list(List<Segment> docList) 
        {
            throw new NotImplementedException();
        }
    }
}