using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Common.Extensions;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Common.EDIStructures
{
    using EdiMessages;

    public class EDIXmlInterchangeControl : EDIXmlMixedContainer
    {
        private readonly ISegmentFactory _segmentFactory;
 

        public EDIXmlInterchangeControl(ISegmentFactory segmentFactory)
            : base(EdiStructureNameConstants.InterchangeControl)
        {
            Label = SegmentLabel.InterchangeLabel.Text;
            _segmentFactory = segmentFactory;
        }

        public void SetHeader(int controlNo, string functionalID, bool test)
        {
            _header = _segmentFactory.GetInterchangeHeader(controlNo,  test);
            var functionGroup = new EDIXmlFunctionGroup(_segmentFactory);
            functionGroup.SetHeader(controlNo, functionalID);
            AddSegment(_header);
            AddFunctionGroup(functionGroup);
        }

        public void SetFooter(int controlNo, int numberOfTransactions)
        {
            var functionGroup = (EDIXmlFunctionGroup) this.EDIFunctionGroups().First();
            functionGroup.SetFooter(numberOfTransactions, controlNo);
            _footer = _segmentFactory.GetInterchangeFooter(1, controlNo);
            AddSegment(_footer);
        }

        public virtual string ControlNumber
        {
            get
            {
                EDIXmlElement element = _header.ElementByLabel("ISA13");
                return element.Value;
            }
        }

        public void AddContent(EDIXmlTransactionSet transactionSet)
        {
            IEnumerable<XElement> ediFunctionGroups = this.EDIFunctionGroups();
            
            if (ediFunctionGroups == null) 
                throw new ApplicationException("Function Group has not been set.");
            
            XElement first = ediFunctionGroups.First();
            
            if (first == null) 
                throw new ApplicationException("No Function Group found in list.");

            first.Add(transactionSet);
        }

        public virtual string FunctionalID
        {
            get
            {
                var grp = this.EDIFunctionGroups().First();
                var seg = grp.EDISegments().First();
                var els = seg.Element("elementList")  ;
                var el = els.EDIElements().ToArray()[0];
                return el.EDIValue();
            }
        }
//        public DateTime Date
//        {
//            get
//            {
//                return EDIUtilities.DateFromEDIDateAndTime(RawDate, EDIXmlElement.DA_DATE,
//                                                           RawTime, EDIXmlElement.DA_TIME);
//            }
//            set
//            {
//                var rdate = "";
//                var rtime = "";
//                EDIUtilities.EDIDateAndTimeFromDate(value, ref rdate, EDIXmlElement.DA_DATE,
//                                                    ref rtime, EDIXmlElement.DA_TIME);
//                RawDate = rdate;
//                RawTime = rtime;
//            }
//        }
//
 

//        public string AuthorizationInfoQualifier
//        {
//            get { return _header.ElementByLabel("ISA01").Value; }
//            set { _header.ElementByLabel("ISA01").Value = value; }
//        
//        }
//
//        public string AuthorizationInfo
//        {
//            get { return _header.ElementByLabel("ISA02").Value; }
//            set { _header.ElementByLabel("ISA02").Value = value; }
//        }
//
//        public string SecurityInfoQualifier
//        {
//            get { return _header.ElementByLabel("ISA03").Value; }
//            set { _header.ElementByLabel("ISA03").Value = value; }
//        }

//        public string SecurityInfo
//        {
//            get { return _header.ElementByLabel("ISA04").Value; }
//            set { _header.ElementByLabel("ISA04").Value = value; }
//        }
//
//        public string SenderIDQualifier
//        {
//            get { return _header.ElementByLabel("ISA05").Value; }
//            set { _header.ElementByLabel("ISA05").Value = value; }
//        }
//
//        public string SenderID
//        {
//            get { return _header.ElementByLabel("ISA06").Value; }
//            set { _header.ElementByLabel("ISA06").Value = value; }
//        }
//
//        public string ReceiverIDQualifier
//        {
//            get { return _header.ElementByLabel("ISA07").Value; }
//            set { _header.ElementByLabel("ISA07").Value = value; }
//        }

//        public string ReceiverID
//        {
//            get { return _header.ElementByLabel("ISA08").Value; }
//            set { _header.ElementByLabel("ISA08").Value = value; }
//        }
//
//        public string RawDate
//        {
//            get { return _header.ElementByLabel("ISA09").Value; }
//            set { _header.ElementByLabel("ISA09").Value = value; }
//        }
//
//        public string RawTime
//        {
//            get { return _header.ElementByLabel("ISA10").Value; }
//            set { _header.ElementByLabel("ISA10").Value = value; }
//        }
//
//        public string RepetitionSeparator
//        { 
//            get { return _header.ElementByLabel("ISA11").Value; }
//            set { _header.ElementByLabel("ISA11").Value = value; }
//        }
//
//        public string VersionNumber
//        { 
//            get { return _header.ElementByLabel("ISA12").Value; }
//            set { _header.ElementByLabel("ISA12").Value = value; }
//        }

//        public string AcknowledgmentRequested
//        {
//
//            get { return _header.ElementByLabel("ISA14").Value; }
//            set { _header.ElementByLabel("ISA14").Value = value; }
//        }

//        public string ComponentElementSeparator
//        {
//
//            get { return _header.ElementByLabel("ISA15").Value; }
//            set { _header.ElementByLabel("ISA15").Value = value; }
//        }
    }
}