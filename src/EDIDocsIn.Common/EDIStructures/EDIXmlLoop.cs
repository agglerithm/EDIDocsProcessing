using EDIDocsOut.Common.EDIStructures;

namespace EDIDocsOut.Common.EDIStructures
{
    public class EDIXmlLoop : EDIXmlMixedContainer 
    {
        public EDIXmlLoop() : base("ediLoop")
        { 
        }

  
        protected  void set_expected_names()
        {
            initialize_expected_names();
            switch (Label)
            { 
                case "AK3":
                    expected_segment_names.Add("AK4");
                    break;
                case "HL":
                    expected_segment_names.Add("HL");
                    expected_segment_names.Add("PRF");
                    expected_segment_names.Add("REF");
                    expected_segment_names.Add("LIN");
                    expected_segment_names.Add("SN1");
                    expected_segment_names.Add("DTM");
                    expected_loop_names.Add("N1");
                    break;
                case "IT1":
                    expected_segment_names.Add("SAC");
                    expected_segment_names.Add("PID");
                    expected_segment_names.Add("TXI");
                    break;
                case "ENT":
                    expected_segment_names.Add("ENT");
                    expected_segment_names.Add("RMR");
                    expected_segment_names.Add("DTM");
                    break;
                case "N1":
                    expected_segment_names.Add("N1");
                    expected_segment_names.Add("N3");
                    expected_segment_names.Add("N4");
                    expected_segment_names.Add("PER");
                    break;
                case "N9":
                    expected_segment_names.Add("MSG");
                    break;
                case "PO1": 
                    expected_segment_names.Add("MSG");
                    expected_segment_names.Add("REF");
                    expected_loop_names.Add("PID");
                    expected_loop_names.Add("AMT");
                    expected_loop_names.Add("SCH");
                    expected_loop_names.Add("PKG");
                    break; 
                case "SCH":
                    expected_segment_names.Add("SCH");
                    break;
                case "PKG":
                    expected_segment_names.Add("PKG");
                    break;

            }
        }

 

    }
}