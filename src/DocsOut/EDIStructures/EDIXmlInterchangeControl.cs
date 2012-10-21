using System;
using System.Linq;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Core.DocsOut.EDIStructures
{
    public class EDIXmlInterchangeControl : EDIXmlMixedContainer
    {
        public EDIXmlInterchangeControl(string control_no)
            : base("ediInterchangeControl")
        {

            control_no_element = "ISA13";
            AuthorizationInfoQualifier = "00";
            AuthorizationInfo = "          ";
            SecurityInfoQualifier = "00";
            SecurityInfo = "          ";
            RepetitionSeparator = "U";
            VersionNumber = "00401";
            AcknowledgmentRequested = "0";
            ControlNumber = control_no;

        }
        public EDIXmlInterchangeControl()
            : base("ediInterchangeControl")
        { 
        }
        public void SetHeader(string receiver_id, int control_no, bool test)
        {
            header = SegmentFactory.GetInterchangeHeader(receiver_id, control_no, test);
        }
 
        public DateTime Date
        {
            get
            {
                return EDIUtilities.DateFromEDIDateAndTime(RawDate, EDIXmlElement.DA_DATE,
                                                           RawTime, EDIXmlElement.DA_TIME);
            }
            set
            {
                var rdate = "";
                var rtime = "";
                EDIUtilities.EDIDateAndTimeFromDate(value, ref rdate, EDIXmlElement.DA_DATE,
                                                    ref rtime, EDIXmlElement.DA_TIME);
                RawDate = rdate;
                RawTime = rtime;
            }
        }

        public new EDIXmlFunctionGroup this[int ndx]
        {
            get { return (EDIXmlFunctionGroup)Elements("ediFunctionGroup").ElementAt(ndx); }
        }

        public string AuthorizationInfoQualifier
        {
            get { return header.ElementByLabel("ISA01").Value; }
            set { header.ElementByLabel("ISA01").Value = value; }
        
        }

        public string AuthorizationInfo
        {
            get { return header.ElementByLabel("ISA02").Value; }
            set { header.ElementByLabel("ISA02").Value = value; }
        }

        public string SecurityInfoQualifier
        {
            get { return header.ElementByLabel("ISA03").Value; }
            set { header.ElementByLabel("ISA03").Value = value; }
        }

        public string SecurityInfo
        {
            get { return header.ElementByLabel("ISA04").Value; }
            set { header.ElementByLabel("ISA04").Value = value; }
        }

        public string SenderIDQualifier
        {
            get { return header.ElementByLabel("ISA05").Value; }
            set { header.ElementByLabel("ISA05").Value = value; }
        }

        public string SenderID
        {
            get { return header.ElementByLabel("ISA06").Value; }
            set { header.ElementByLabel("ISA06").Value = value; }
        }

        public string ReceiverIDQualifier
        {
            get { return header.ElementByLabel("ISA07").Value; }
            set { header.ElementByLabel("ISA07").Value = value; }
        }

        public string ReceiverID
        {
            get { return header.ElementByLabel("ISA08").Value; }
            set { header.ElementByLabel("ISA08").Value = value; }
        }

        public string RawDate
        {
            get { return header.ElementByLabel("ISA09").Value; }
            set { header.ElementByLabel("ISA09").Value = value; }
        }

        public string RawTime
        {
            get { return header.ElementByLabel("ISA10").Value; }
            set { header.ElementByLabel("ISA10").Value = value; }
        }

        public string RepetitionSeparator
        { 
            get { return header.ElementByLabel("ISA11").Value; }
            set { header.ElementByLabel("ISA11").Value = value; }
        }

        public string VersionNumber
        { 
            get { return header.ElementByLabel("ISA12").Value; }
            set { header.ElementByLabel("ISA12").Value = value; }
        }

        public string AcknowledgmentRequested
        {

            get { return header.ElementByLabel("ISA14").Value; }
            set { header.ElementByLabel("ISA14").Value = value; }
        }

        public string ComponentElementSeparator
        {

            get { return header.ElementByLabel("ISA15").Value; }
            set { header.ElementByLabel("ISA15").Value = value; }
        }
    }
}