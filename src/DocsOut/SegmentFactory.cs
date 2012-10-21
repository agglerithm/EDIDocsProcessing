using System; 
using System.Xml.Linq;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Core.DocsOut
{
    public class SegmentFactory
    {
        public static EDIXmlSegment GetInterchangeHeader(string receiver_id, int control_no,
                                                         bool test)
        {
            var seg = new EDIXmlSegment("ISA");
            string test_val = "P";
            if (test)
                test_val = "T"; 
            seg.Add(new EDIXmlElement("ISA01", "00"));
            seg.Add(new EDIXmlElement("ISA02", "          "));
            seg.Add(new EDIXmlElement("ISA03", "00"));
            seg.Add(new EDIXmlElement("ISA04", "          "));
            seg.Add(new EDIXmlElement("ISA05", "12"));
            seg.Add(new EDIXmlElement("ISA06", "EEC5122516063  "));
            seg.Add(new EDIXmlElement("ISA07", "08"));
            seg.Add(new EDIXmlElement("ISA08", receiver_id));
            seg.Add(new EDIXmlElement("ISA09", DateTime.Today.ToString("yyMMdd")));
            seg.Add(new EDIXmlElement("ISA10", DateTime.Now.ToString("hhmm")));
            seg.Add(new EDIXmlElement("ISA11", "U"));
            seg.Add(new EDIXmlElement("ISA12", "00401"));
            seg.Add(new EDIXmlElement("ISA13", control_no.ToString("00000000#")));
            seg.Add(new EDIXmlElement("ISA14", "0"));
            seg.Add(new EDIXmlElement("ISA15", test_val));
            seg.Add(new EDIXmlElement("ISA16", ">"));
            return  seg ;
        }

        public static EDIXmlSegment GetGroupHeader(string functional_id, string receiver_code,
                                                   int control_number)
        {
            var seg = new EDIXmlSegment("GS");  
            seg.Add(new EDIXmlElement("GS01", functional_id));
            seg.Add(new EDIXmlElement("GS02", "EEC5122516063"));
            seg.Add(new EDIXmlElement("GS03", receiver_code));
            seg.Add(new EDIXmlElement("GS04", DateTime.Today.ToString("yyyyMMdd")));
            seg.Add(new EDIXmlElement("GS05", DateTime.Now.ToString("hhmmss")));
            seg.Add(new EDIXmlElement("GS06", control_number.ToString("00000000#")));
            seg.Add(new EDIXmlElement("GS07", "X"));
            seg.Add(new EDIXmlElement("GS08", "004010")); 
            return  seg ;
        }

        public static EDIXmlSegment GetDocumentHeader(string doc_type, int control_number)
        {
            var seg = new EDIXmlSegment("ST");
            seg.Add(new EDIXmlElement("ST01", doc_type));
            seg.Add(new EDIXmlElement("ST02", control_number.ToString()));
            return seg;
        }

        public static EDIXmlSegment GetDocumentFooter(int num_segments, int control_number)
        { 
            var seg = new EDIXmlSegment("SE");
            seg.Add(new EDIXmlElement("SE01", num_segments.ToString()));
            seg.Add(new EDIXmlElement("SE02", control_number.ToString()));
            return  seg ;
        }

        public static EDIXmlSegment GetGroupFooter(int num_docs, int control_number)
        { 
            var seg = new EDIXmlSegment("GE");
            seg.Add(new EDIXmlElement("GE01", num_docs.ToString()));
            seg.Add(new EDIXmlElement("GE02", control_number.ToString("00000000#")));
            return seg ;
        }

        public static EDIXmlSegment GetInterchangeFooter(int num_groups, int control_number)
        { 
            var seg = new EDIXmlSegment("IEA");
            seg.Add(new EDIXmlElement("IEA01", num_groups.ToString()));
            seg.Add(new EDIXmlElement("IEA02", control_number.ToString("00000000#")));
            return  seg;
        }

        public static EDIXmlSegment GetDateTimeSegment(string date_type, DateTime dtm)
        {
            var seg = new EDIXmlSegment("DTM");
            seg.Add(new EDIXmlElement("DTM01", date_type));
            seg.Add(new EDIXmlElement("DTM02", dtm.ToString("yyyyMMdd")));
            seg.Add(new EDIXmlElement("DTM03", dtm.ToString("hhmmss")));

            return seg;
        }

        public static EDIXmlSegment GetPurchaseOrderReference(string po_number)
        {
            var seg = new EDIXmlSegment("PRF");
            seg.Add(new EDIXmlElement("PRF01",po_number));
            return seg;
        }

        public static EDIXmlSegment GetHierarchicalLevel(string id, string parent_id, 
                                                         string code, bool has_children)
        {
            var child_code = "";
            if (has_children)
                child_code = "1";
            var seg = new EDIXmlSegment("HL");
            seg.Add(new EDIXmlElement("HL01", id));
            seg.Add(new EDIXmlElement("HL02", parent_id));
            seg.Add(new EDIXmlElement("HL03", code));
            seg.Add(new EDIXmlElement("HL04", child_code));

            return seg;
        }

        public static EDIXmlSegment GetAddressName(string name, string address_type, 
                                                   string code_qualifier, string code)
        { 
            var seg = new EDIXmlSegment("N1");
            seg.Add(new EDIXmlElement("N101", address_type));
            seg.Add(new EDIXmlElement("N102", name));
            seg.Add(new EDIXmlElement("N103", code_qualifier));
            seg.Add(new EDIXmlElement("N104", code));

            return seg;
        }

        public static EDIXmlSegment GetAddressLine(string addr_info_1, string addr_info_2)
        {
            var seg = new EDIXmlSegment("N3");
            seg.Add(new EDIXmlElement("N301", addr_info_1));
            seg.Add(new EDIXmlElement("N301", addr_info_2));

            return seg;
        }

        public static EDIXmlSegment GetGeographicInfo(string city, string state,
                                                      string zip, string country)
        {
            var seg = new EDIXmlSegment("N4");
            seg.Add(new EDIXmlElement("N401", city));
            seg.Add(new EDIXmlElement("N402", state));
            seg.Add(new EDIXmlElement("N403", zip));
            seg.Add(new EDIXmlElement("N404", country));

            return seg;
        }

        public static EDIXmlSegment GetLineItem(string line_num, string customerPartNum, 
                                                string itemID)
        {
            var seg = new EDIXmlSegment("LIN");
            seg.Add(new EDIXmlElement("LIN01", line_num));
            seg.Add(new EDIXmlElement("LIN02", "IN"));
            seg.Add(new EDIXmlElement("LIN03", customerPartNum));
            seg.Add(new EDIXmlElement("LIN04", "VN"));
            seg.Add(new EDIXmlElement("LIN05", itemID));

            return seg;
        }

        public static EDIXmlSegment GetLineItemShipmentDetail(string line_num, int qty_shipped,
                                                              int qty_ordered, string unit_of_measure, string status)
        {
            var seg = new EDIXmlSegment("SN1");
            seg.Add(new EDIXmlElement("SN101", line_num));
            seg.Add(new EDIXmlElement("SN102",
                                      qty_shipped.ToString()));
            seg.Add(new EDIXmlElement("SN103", unit_of_measure));
            seg.Add(new EDIXmlElement("SN104", ""));
            seg.Add(new EDIXmlElement("SN105", qty_ordered.ToString()));
            seg.Add(new EDIXmlElement("SN106", unit_of_measure));
            seg.Add(new EDIXmlElement("SN107", ""));
            seg.Add(new EDIXmlElement("SN108", status));

            return seg;
        }

        public static EDIXmlSegment GetTransactionTotal(int total_lines)
        {
            var seg = new EDIXmlSegment("CTT");
            seg.Add(new EDIXmlElement("CTT01",total_lines.ToString()));
            return seg;
        }
    }
}