using System;
using System.Xml.Linq;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Core.DocsOut.EDIStructures
{
    public class EDIXmlFunctionGroup : EDIXmlMixedContainer
    {
 
        public EDIXmlFunctionGroup():base("ediFunctionGroup")
        { 
        }
        public void SetHeader(string receiver_id, int control_no, string functional_id)
        {
            header = SegmentFactory.GetGroupHeader(functional_id, receiver_id, control_no);
        }

//        public string GroupIDCode
//        {
//            get { return header.ElementByLabel("GS01").Value; }
//            set { header.ElementByLabel("GS01").Value = value; }
//        }
//
//        public string SenderCode
//        {
//            get { return header.ElementByLabel("GS02").Value; }
//            set { header.ElementByLabel("GS02").Value = value; }
//        }
//
//        public string ReceiverCode
//        {
//            get { return header.ElementByLabel("GS03").Value; }
//            set { header.ElementByLabel("GS03").Value = value; }
//        }

        public DateTime Date
        {
            get { return  EDIUtilities.DateFromEDIDateAndTime(RawDate,EDIXmlElement.DATE,
                                                              RawTime, EDIXmlElement.DA_TIME);
            }
            set {
                var rdate = "";
                var rtime = ""; 
                EDIUtilities.EDIDateAndTimeFromDate(value, ref rdate, EDIXmlElement.DATE,
                                                    ref rtime, EDIXmlElement.DA_TIME);
                RawDate = rdate;
                RawTime = rtime;
            }
        }

        public string RawDate
        {
            get { return header.ElementByLabel("GS04").Value; }
            set { header.ElementByLabel("GS04").Value = value; }
        }

        public string RawTime
        {
            get { return header.ElementByLabel("GS05").Value; }
            set { header.ElementByLabel("GS05").Value = value; }
        }

        public string ResponsibleAgencyCode
        {
            get { return header.ElementByLabel("GS07").Value; }
            set { header.ElementByLabel("GS07").Value = value; }
        }

        public string VersionCode
        {
            get { return header.ElementByLabel("GS08").Value; }
            set { header.ElementByLabel("GS08").Value = value; }
        }
    }
}