using System.Collections.Generic;
using System.Linq;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.DTOs;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Extensions;

namespace EDIDocsProcessing.Core.DocsIn.impl
{
    public class EDIResponseReferenceRecorder : IEDIResponseReferenceRecorder
    {
        private readonly IList<ResponseElementEntity> _responseValues = new List<ResponseElementEntity>();
        private readonly IList<DocumentLineItemEntity> _lines = new List<DocumentLineItemEntity>();

        public IEnumerable<Segment> MemorizeOuterReferences(List<Segment> segList, string controlNumber,
            string elDelimiter, BusinessPartner partner)
        {
            segList.SetContexts();
            var returnList = new List<Segment>();
            IEnumerable<Segment> refs = segList.Where(s => s.Context == "root");
            foreach (Segment rf in refs.ToList())
            {
                process_outer_ref(rf, controlNumber, elDelimiter, partner);
                returnList.Add(rf); 
            }
            return returnList;
        }


        public void MemorizeInnerReferences(IList<DocumentLineItemEntity> refs, string controlNumber, BusinessPartner partner)
        {
            if (refs == null || refs.Count() == 0) return;
            refs.ForEach(s => save_line(s, controlNumber, partner));
        }

//        public static DocumentLineItemEntity BuildInnerResponse(IEnumerable<Segment> segList, int lineNumber)
//        {
//            IEnumerable<Segment> refs = from s in segList.ToList()
//                                        where s.Label == SegmentLabels.ReferenceLabel
//                                        select s;
//            return new DocumentLineItemEntity
//                           {LineIdentifier = lineNumber.ToString(), ResponseElements = build_elements(refs)};
// 
//        }

//        private static IList<LineResponseElementEntity> build_elements(IEnumerable<Segment> refSegs)
//        {
//            if (refSegs == null) return null;
//            var responseEls = new List<LineResponseElementEntity>();
//            refSegs.ForEach(s => add_line_response_element(s, responseEls));
//            return responseEls;
//
//        }

        public IEnumerable<ResponseElementEntity> GetResponseValues()
        {
            return _responseValues; 
        }

        public IEnumerable<DocumentLineItemEntity> GetLines()
        {
            return _lines; 
        }

        public void Clear()
        {
            _lines.Clear();
            _responseValues.Clear();
        }

//        private static void add_line_response_element(Segment seg, ICollection<LineResponseElementEntity> responseEls)
//        {
//            var elDelim = seg.Contents.Substring(3, 1);
//            var arr = seg.GetElements(elDelim);
//            responseEls.Add(new LineResponseElementEntity
//                                {
//                                    ElementName = "REF02",
//                                    Qualifier = arr[1],
//                                    Value = arr[2]
//                                });
//        }

        private void process_outer_ref(Segment rf, string controlNumber, string elDelimiter, BusinessPartner partner)
        {
            var arr = rf.GetElements(elDelimiter);
            assign_outer_reference(arr[1], arr[2], controlNumber, partner);
        }

        private void assign_outer_reference(string qualifier, string identification, string controlNumber, BusinessPartner partner)
        {
            _responseValues.Add(new ResponseElementEntity
            {
                DocumentInDTO = new DocumentInDTO
                {
                    ControlNumber = controlNumber.CastToInt(),
                    PartnerNumber = partner.Number
                },
                ElementName = "REF02",
                Value = identification,
                Qualifier = qualifier
            });

        }

 
        private void save_line(DocumentLineItemEntity line, string controlNo, BusinessPartner partner)
        {
            line.DocumentInDTO = new DocumentInDTO
            {
                ControlNumber = controlNo.CastToInt(),
                PartnerNumber = partner.Number
            };
            _lines.Add(line);

        }
    }
}