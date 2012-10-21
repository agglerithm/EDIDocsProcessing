namespace EDIDocsProcessing.Common.EDIStructures
{
    using Common;
    using DTOs;
    using  Enumerations;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using EdiMessages;

//    public interface IEDIXmlTransactionSet
//    {
//        EDIXmlTransactionSet SetHeader(string docType, int controlNo);
//        string TransactionType { get; }
//        string Value { get; }
//        EDIXmlTransactionSet SetFooter();
//        void AddSegment(EDIXmlSegment seg);
//        void AddLoop(EDIXmlMixedContainer loop);
//        void AddTransactionSet(EDIXmlTransactionSet ts);
//        void AddFunctionGroup(EDIXmlFunctionGroup grp); 
//        ISAEntity ISA { get; set;} 
//    }

    public class EDIXmlTransactionSet : EDIXmlMixedContainer 
    {
        private readonly ISegmentFactory _segmentFactory;
        private int _controlNumber;


        public EDIXmlTransactionSet(ISegmentFactory segmentFactory)
            : base(EdiStructureNameConstants.TransactionSet)
        {
            Label = SegmentLabel.DocumentLabel.Text;
            _segmentFactory = segmentFactory;
        }

        public int PartnerId
        {
            get;   set;
        }

        public ISAEntity ISA
        {
            get; set;
        }

         
        public EDIXmlTransactionSet SetHeader(string docType, int controlNo)
        {
            _controlNumber = controlNo; 
            _header = _segmentFactory.GetDocumentHeader(docType, controlNo);
            AddSegment(_header);
            return this;
        }

        public string TransactionType
        {
            get { return _header.ElementByLabel("ST01").Value; }
        }

        public IEnumerable<EDIXmlSegment> Segments
        {
            get { return Descendants().Where(s => s.Name.LocalName == EdiStructureNameConstants.Segment).Select(seg => (EDIXmlSegment)seg); } 
            
        }

        public void StripEmptyTrailingElements()
        {
            Segments.ForEach(s => s.StripEmptyTrailingElements());
        }

//        protected  void set_expected_names()
//        {
//            initialize_expected_names(); 
//            expected_loop_names.Add("ACK");  
//            expected_loop_names.Add("AMT");
//            expected_loop_names.Add("FOP");
//            expected_loop_names.Add("FST"); 
//            expected_loop_names.Add("HL"); 
//            expected_loop_names.Add("IT1"); 
//            expected_loop_names.Add("LIN"); 
//            expected_loop_names.Add("N1");
//            expected_loop_names.Add("N9");
//            expected_loop_names.Add("PER"); 
//            expected_loop_names.Add("PID" ); 
//            expected_loop_names.Add("PO1"); 
//            expected_loop_names.Add("POC"); 
//            expected_loop_names.Add("SAC"); 
//            expected_loop_names.Add("SLN");
//            expected_loop_names.Add("AK2");
//            expected_loop_names.Add("AK3"); 
//            expected_loop_names.Add("ENT"); 
//            
//        }
        private int SegmentCount
        {
            get
            { 
               return  Segments.Count();
            }
        }

        public EDIXmlTransactionSet SetFooter()
        {
            _footer = _segmentFactory.GetDocumentFooter(SegmentCount + 1, _controlNumber);
            AddSegment(_footer);
            return this;
        }
    }
}