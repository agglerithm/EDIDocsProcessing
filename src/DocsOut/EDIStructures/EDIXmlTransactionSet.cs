using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Core.DocsOut.EDIStructures
{
    public class EDIXmlTransactionSet : EDIXmlMixedContainer
    {
//        public EDIXmlTransactionSet(string control_no, EDIXmlBuildValues bv)
//            : base("ediTransactionSet", bv)
//        {
//            SpecName = header.ElementBySpecName("ST01").Value;
//            control_no_element = "ST02";
//            ControlNumber = control_no;
//        }

        public EDIXmlTransactionSet()
            : base("ediTransactionSet")
        { 
        }


        public void SetHeader(string doc_type, int control_no)
        {
            _control_number = control_no;
            header = SegmentFactory.GetDocumentHeader(doc_type, control_no);
            AddSegment(header); 
        }

        public string TransactionType
        {
            get { return header.ElementByLabel("ST01").Value; }
        }

        protected  void set_expected_names()
        {
            initialize_expected_names(); 
            expected_loop_names.Add("ACK");  
            expected_loop_names.Add("AMT");
            expected_loop_names.Add("FOP");
            expected_loop_names.Add("FST"); 
            expected_loop_names.Add("HL"); 
            expected_loop_names.Add("IT1"); 
            expected_loop_names.Add("LIN"); 
            expected_loop_names.Add("N1");
            expected_loop_names.Add("N9");
            expected_loop_names.Add("PER"); 
            expected_loop_names.Add("PID" ); 
            expected_loop_names.Add("PO1"); 
            expected_loop_names.Add("POC"); 
            expected_loop_names.Add("SAC"); 
            expected_loop_names.Add("SLN");
            expected_loop_names.Add("AK2");
            expected_loop_names.Add("AK3"); 
            expected_loop_names.Add("ENT"); 
            
        }


        public void SetFooter()
        {
            footer = SegmentFactory.GetDocumentFooter(SegmentCount + 1, _control_number);
            AddSegment(footer);
        }
    }
}