using System;
using System.Collections.Generic;
using AFPST.Common.Infrastructure;
using EDIDocsProcessing.Common;
using EDIDocsProcessing.Common.EDIStructures;
using EDIDocsProcessing.Common.Enumerations;
using EDIDocsProcessing.Common.Extensions;
using EdiMessages;


namespace EDIDocsProcessing.Core.DocsOut
{
    [CoverageExclude("not worth testing, this stuff is more like a bunch of constants","Greg")]
    public class SegmentFactory : ISegmentFactory
    {
        private readonly IBusinessPartnerSpecificServiceResolver _serviceResolver;
        private   IBuildValueFactory _buildFactory;

        public SegmentFactory(IBusinessPartnerSpecificServiceResolver serviceResolver)
        {
            _serviceResolver = serviceResolver;
        }

        public void SetBuildValues(BusinessPartner partner)
        {
            _buildFactory = _serviceResolver.GetBuildValueFactoryFor(partner);
            if (_buildFactory == null)
                throw new ApplicationException("There was no build factory for partner " + partner.Code);
        }

        public EDIXmlSegment GetInterchangeHeader(int controlNo,   bool test)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("ISA", buildValues);
            string testVal = "P";
            if (test)
                testVal = "T";
            seg.Add(new EDIXmlElement("ISA01", "00", buildValues));
            seg.Add(new EDIXmlElement("ISA02", "          ", buildValues));
            seg.Add(new EDIXmlElement("ISA03", "00", buildValues));
            seg.Add(new EDIXmlElement("ISA04", "          ", buildValues));
            seg.Add(new EDIXmlElement("ISA05", buildValues.InterchangeSenderQualifier, buildValues));
            seg.Add(new EDIXmlElement("ISA06", buildValues.InterchangeSenderID.PadRight(15,' '), buildValues));
            seg.Add(new EDIXmlElement("ISA07", buildValues.InterchangeReceiverQualifier, buildValues));
            seg.Add(new EDIXmlElement("ISA08", buildValues.InterchangeReceiverID.PadRight(15,' '), buildValues));
            seg.Add(new EDIXmlElement("ISA09", DateTime.Today.ToString("yyMMdd"), buildValues));
            seg.Add(new EDIXmlElement("ISA10", DateTime.Now.ToString("hhmm"), buildValues));
            seg.Add(new EDIXmlElement("ISA11", "U", buildValues));
            seg.Add(new EDIXmlElement("ISA12", "00401", buildValues));
            seg.Add(new EDIXmlElement("ISA13", controlNo.ToString("00000000#"), buildValues));
            seg.Add(new EDIXmlElement("ISA14", "0", buildValues));
            seg.Add(new EDIXmlElement("ISA15", testVal, buildValues));
            seg.Add(new EDIXmlElement("ISA16", ">", buildValues));
            return  seg ;
        }

        public EDIXmlSegment GetGroupHeader(string functionalID,   int controlNumber )
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("GS", buildValues);
            seg.Add(new EDIXmlElement("GS01", functionalID, buildValues));
            seg.Add(new EDIXmlElement("GS02", buildValues.InterchangeSenderID, buildValues));
            seg.Add(new EDIXmlElement("GS03", buildValues.FunctionGroupReceiverID, buildValues));
            seg.Add(new EDIXmlElement("GS04", DateTime.Today.ToString("yyyyMMdd"), buildValues));
            seg.Add(new EDIXmlElement("GS05", DateTime.Now.ToString("HHmmss"), buildValues));
            seg.Add(new EDIXmlElement("GS06", controlNumber.FormatByTransport(buildValues.Transport), buildValues));
            seg.Add(new EDIXmlElement("GS07", "X", buildValues));
            seg.Add(new EDIXmlElement("GS08", "004010", buildValues)); 
            return  seg ;
        }

        public EDIXmlSegment GetDocumentHeader(string docType, int controlNumber)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("ST", buildValues);
            seg.Add(new EDIXmlElement("ST01", docType, buildValues));
            seg.Add(new EDIXmlElement("ST02", controlNumber.ToString().PadLeft(6, '0'), buildValues));
            return seg;
        }

        public EDIXmlSegment GetDocumentFooter(int numSegments, int controlNumber)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("SE", buildValues);
            seg.Add(new EDIXmlElement("SE01", numSegments.ToString(), buildValues));
            seg.Add(new EDIXmlElement("SE02", controlNumber.ToString().PadLeft(6, '0'), buildValues));
            return  seg ;
        }

        public EDIXmlSegment GetGroupFooter(int numDocs, int controlNumber)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("GE", buildValues);
            seg.Add(new EDIXmlElement("GE01", numDocs.ToString(), buildValues));
            seg.Add(new EDIXmlElement("GE02", controlNumber.FormatByTransport(buildValues.Transport), buildValues));
            return seg ;
        }

        public EDIXmlSegment GetInterchangeFooter(int numGroups, int controlNumber)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("IEA", buildValues);
            seg.Add(new EDIXmlElement("IEA01", numGroups.ToString(), buildValues));
            seg.Add(new EDIXmlElement("IEA02", controlNumber.ToString("00000000#"), buildValues));
            return  seg;
        }

        public EDIXmlSegment GetDateTimeSegment(string dateType, DateTime dtm)
        {
            var dte = dtm.ToString("yyyyMMdd");
            var tme = dtm.ToString("HHmmss");
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("DTM", buildValues);
            seg.Add(new EDIXmlElement("DTM01", dateType, buildValues));
            seg.Add(new EDIXmlElement("DTM02", dte, buildValues)); 
            seg.Add(new EDIXmlElement("DTM03", tme, buildValues));

            return seg;
        }

 
        public EDIXmlSegment GetPurchaseOrderReference(string poNumber)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("PRF", buildValues);
            seg.Add(new EDIXmlElement("PRF01", poNumber, buildValues));
            return seg;
        }

        public EDIXmlSegment GetHierarchicalLevel(string id, string parentID,
                                                         string code)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues(); 
            var seg = new EDIXmlSegment("HL", buildValues);
            seg.Add(new EDIXmlElement("HL01", id, buildValues));
            seg.Add(new EDIXmlElement("HL02", parentID, buildValues));
            seg.Add(new EDIXmlElement("HL03", code, buildValues));

            return seg;
        }

        public EDIXmlSegment GetAddressName(string name, string addressType, string codeQualifier, string code)
        {
            var pair = new QualifierValuePair(codeQualifier, code, 3);
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("N1", buildValues);
            seg.Add(new EDIXmlElement("N101", addressType, buildValues));
            seg.Add(new EDIXmlElement("N102", name, buildValues));
            seg.AddElements(pair.GetQualfierValuePair(seg.SegmentName, buildValues));

            return seg;
        }

 

        public EDIXmlSegment GetAddressLine(string addrInfo1, string addrInfo2)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("N3", buildValues);
            seg.Add(new EDIXmlElement("N301", addrInfo1, buildValues));
            seg.Add(new EDIXmlElement("N301", addrInfo2, buildValues));

            return seg;
        }

        public EDIXmlSegment GetGeographicInfo(string city, string state,
                                                      string zip, string country)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("N4", buildValues);
            seg.Add(new EDIXmlElement("N401", city, buildValues));
            seg.Add(new EDIXmlElement("N402", state, buildValues));
            seg.Add(new EDIXmlElement("N403", zip, buildValues));
            seg.Add(new EDIXmlElement("N404", country, buildValues));

            return seg;
        }

        public EDIXmlSegment GetShipmentLineItem(string lineNum, IEnumerable<QualifierValuePair> pairs)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("LIN", buildValues);
            seg.Add(new EDIXmlElement("LIN01", lineNum, buildValues));
            pairs.ForEach(p => seg.AddElements(p.GetQualfierValuePair(seg.SegmentName, buildValues)));
            return seg;
        }

        public  EDIXmlSegment GetShipmentLineItem(string lineNum, string customerPartNum,
                                                string itemID )
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("LIN", buildValues);
            seg.Add(new EDIXmlElement("LIN01", lineNum, buildValues));
            seg.Add(new EDIXmlElement("LIN02", Qualifier.BuyerItemNumber.Value, buildValues));
            seg.Add(new EDIXmlElement("LIN03", customerPartNum, buildValues));
            seg.Add(new EDIXmlElement("LIN04", Qualifier.VendorItemNumber.Value, buildValues));
            seg.Add(new EDIXmlElement("LIN05", itemID, buildValues));
            seg.Add(new EDIXmlElement("LIN06", Qualifier.ProductType.Value, buildValues));
            seg.Add(new EDIXmlElement("LIN07", "N", buildValues));

            return seg;
        }
 

 

        public EDIXmlElement GetHLChildElement()
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            return new EDIXmlElement("HL04", "1", buildValues); 
        }

        public EDIXmlSegment GetLineItemShipmentDetail(string lineNum, int qtyShipped,
                                                              int qtyOrdered, 
            int qtyShippedToDate, 
            string status)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("SN1", buildValues);
            seg.Add(new EDIXmlElement("SN101", lineNum, buildValues));
            seg.Add(new EDIXmlElement("SN102",
                                      qtyShipped.ToString(), buildValues));
            seg.Add(new EDIXmlElement("SN103", "EA", buildValues));
            seg.Add(new EDIXmlElement("SN104", 
                qtyShippedToDate.ToString(), buildValues));
            seg.Add(new EDIXmlElement("SN105", qtyOrdered.ToString(), 
                buildValues));
            seg.Add(new EDIXmlElement("SN106", "EA", buildValues));
            seg.Add(new EDIXmlElement("SN107", "", buildValues));
            seg.Add(new EDIXmlElement("SN108", status, buildValues));

            return seg;
        }
 


        public EDIXmlSegment GetTransactionTotal(int totalLines)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("CTT", buildValues);
            seg.Add(new EDIXmlElement("CTT01", totalLines.ToString(), buildValues));
            return seg;
        }

        public EDIXmlSegment GetPOLine(string lineNo, 
            int quantity, decimal price, string custPartNo,
            string itemID, string itemDescription)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("PO1", buildValues);
            seg.Add(new EDIXmlElement("PO101", lineNo, buildValues));
            seg.Add(new EDIXmlElement("PO102", quantity.ToString(), buildValues));
            seg.Add(new EDIXmlElement("PO103", "EA", buildValues));
            seg.Add(new EDIXmlElement("PO104", price.ToString("#.00"), buildValues));
            seg.Add(new EDIXmlElement("PO105", "", buildValues));
            seg.Add(new EDIXmlElement("PO106", ItemReferenceConstants.CustomerPartNumberCode, buildValues));
            seg.Add(new EDIXmlElement("PO107", custPartNo, buildValues));
            var itemcode = "";
            if (!itemID.IsNullOrEmpty())
                itemcode = ItemReferenceConstants.ItemIDCode;
            seg.Add(new EDIXmlElement("PO108", itemcode, buildValues));
            seg.Add(new EDIXmlElement("PO109", itemID, buildValues));
            seg.Add(new EDIXmlElement("PO110", ItemReferenceConstants.ItemDescriptionCode, buildValues));
            seg.Add(new EDIXmlElement("PO111", itemDescription.SafeTrim().TruncateTo(48), buildValues));
            seg.Add(new EDIXmlElement("PO112", ItemReferenceConstants.ProductTypeCode, buildValues));
            seg.Add(new EDIXmlElement("PO113", "N", buildValues));

            return seg;
        }

 

        public EDIXmlSegment GetAckLine(string statusCode,  
            int quantity, string requestedShipDate, string custPartNo,
            string itemID, string itemDescription)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("ACK", buildValues);
            seg.Add(new EDIXmlElement("ACK01", statusCode, buildValues));
            seg.Add(new EDIXmlElement("ACK02", quantity.ToString(), buildValues));
            seg.Add(new EDIXmlElement("ACK03", "EA", buildValues));
            seg.Add(new EDIXmlElement("ACK04", DateTypeConstants.EstimatedDeliveryOn, buildValues));
            seg.Add(new EDIXmlElement("ACK05", requestedShipDate, buildValues));
            seg.Add(new EDIXmlElement("ACK06", "", buildValues));
            seg.Add(new EDIXmlElement("ACK07", ItemReferenceConstants.CustomerPartNumberCode, buildValues));
            seg.Add(new EDIXmlElement("ACK08", custPartNo, buildValues));
            var itemcode = "";
            if (!itemID.IsNullOrEmpty())
                itemcode = ItemReferenceConstants.ItemIDCode;
            seg.Add(new EDIXmlElement("ACK09", itemcode, buildValues));
            seg.Add(new EDIXmlElement("ACK10", itemID, buildValues));
            seg.Add(new EDIXmlElement("ACK11", ItemReferenceConstants.ItemDescriptionCode, buildValues));
            seg.Add(new EDIXmlElement("ACK12", itemDescription, buildValues));
            seg.Add(new EDIXmlElement("ACK13", ItemReferenceConstants.ProductTypeCode, buildValues));
            seg.Add(new EDIXmlElement("ACK14", "N", buildValues));

            return seg;
        }

        public EDIXmlSegment GetCurrencySegment(string qualifier, string code)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("CUR", buildValues);
            seg.Add(new EDIXmlElement("CUR01", qualifier, buildValues));
            seg.Add(new EDIXmlElement("CUR02", code, buildValues));
            return seg;
        }

        public EDIXmlSegment GetReferenceIDSegment(string qualifier, string code )
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("REF", buildValues);
            seg.Add(new EDIXmlElement("REF01", qualifier, buildValues));
            seg.Add(new EDIXmlElement("REF02", code, buildValues));
            return seg;
        }

        public EDIXmlSegment GetTotalMonetaryValue(decimal val)
        {
            val *= 100;
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("TDS", buildValues);
            seg.Add(new EDIXmlElement("TDS01",((int)val).ToString(),buildValues));
            return seg;
        }

        public EDIXmlSegment GetRoutingCarrierDetails(string routingCode, string codeQualifier, 
            string idCode, string transportationCode, string carrier, string statusCode)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("TD5", buildValues);
            seg.Add(new EDIXmlElement("TD501", routingCode, buildValues));
            seg.Add(new EDIXmlElement("TD502", codeQualifier, buildValues));
            seg.Add(new EDIXmlElement("TD503", idCode, buildValues));
            seg.Add(new EDIXmlElement("TD504", transportationCode, buildValues));
            seg.Add(new EDIXmlElement("TD505", carrier, buildValues));
            if(!string.IsNullOrEmpty(statusCode))
                seg.Add(new EDIXmlElement("TD506", statusCode, buildValues));
            return seg;
        }

        public EDIXmlSegment GetEquipmentCarrierDetails(string code, string equipmentNumber)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("TD3", buildValues);
            seg.Add(new EDIXmlElement("TD301", code, buildValues));
            seg.Add(new EDIXmlElement("TD302", "", buildValues));
            seg.Add(new EDIXmlElement("TD303", equipmentNumber, buildValues));
            return seg;
        }

        public EDIXmlSegment GetQtyWeightCarrierDetails(string packaging, int qty, string qualifier, int weight, string um)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("TD1", buildValues); 
            seg.Add(new EDIXmlElement("TD101", packaging, buildValues));
            seg.Add(new EDIXmlElement("TD102", qty.ToString(), buildValues));
            seg.Add(new EDIXmlElement("TD103", "", buildValues));
            seg.Add(new EDIXmlElement("TD104", "", buildValues));
            seg.Add(new EDIXmlElement("TD105", "", buildValues));
            seg.Add(new EDIXmlElement("TD106", qualifier, buildValues));
            seg.Add(new EDIXmlElement("TD107", weight.ToString(), buildValues));
            seg.Add(new EDIXmlElement("TD108", um, buildValues));
            return seg;
        }


        public EdiXmlBuildValues BuildValues
        {
            get { return _buildFactory.GetValues(); }
        }

        public EDIXmlSegment GetTerms(string typecode, decimal discountpercent, DateTime invoiceDate, int discountDays, int netDays)
        {
            DateTime discountduedate = invoiceDate.AddDays(discountDays);
            var dddtext = "";
            if(discountDays > 0)
                dddtext = discountduedate.EDIDateFromDate(true);
            DateTime netduedate = invoiceDate.AddDays(netDays);
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("ITD", buildValues);
            seg.Add(new EDIXmlElement("ITD01", typecode, buildValues));
            seg.Add(new EDIXmlElement("ITD02", "3", buildValues));
            seg.Add(new EDIXmlElement("ITD03", (discountpercent * 100).ToString("#.00"), buildValues));
            seg.Add(new EDIXmlElement("ITD04", dddtext, buildValues));
            seg.Add(new EDIXmlElement("ITD05", discountDays.ToString(), buildValues));
            seg.Add(new EDIXmlElement("ITD06", netduedate.EDIDateFromDate(true), buildValues));
            seg.Add(new EDIXmlElement("ITD06", netDays.ToString(), buildValues)); 
            return seg;
        }

        public EDIXmlSegment GetLineItemInvoiceDetail(string lineNum, int quantity, decimal price,   
            IDictionary<Qualifier, string> detail)
        {       
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("IT1", buildValues);
            seg.Add(new EDIXmlElement("IT101", lineNum, buildValues));
            seg.Add(new EDIXmlElement("IT102",
                                      quantity.ToString(), buildValues));
            seg.Add(new EDIXmlElement("IT103", "EA", buildValues));
            seg.Add(new EDIXmlElement("IT104", price.ToString(), buildValues));
            seg.Add(new EDIXmlElement("IT105","PE", buildValues));
            int num = 6;
            detail.ForEach(p =>  add_invoice_detail(seg, p, buildValues, ref num) ); 

            return seg; 
        }

        public EDIXmlSegment GetProductItemDescription(string description)
        { 
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("PID", buildValues);
            seg.Add(new EDIXmlElement("PID01", "F", buildValues));
            seg.Add(new EDIXmlElement("PID02", "", buildValues));
            seg.Add(new EDIXmlElement("PID03", "", buildValues));
            seg.Add(new EDIXmlElement("PID04", "", buildValues));
            seg.Add(new EDIXmlElement("PID05", description, buildValues));
            return seg;
        }

        public EDIXmlSegment GetTaxInformationSegment(string taxType, decimal amount)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("TXI", buildValues);
            seg.Add(new EDIXmlElement("TXI01", taxType, buildValues));
            seg.Add(new EDIXmlElement("TXI02", amount.ToString("#.00"), buildValues));
            return seg;
        }

        public EDIXmlSegment GetQuantitySegment(string qual, decimal qty)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("QTY", buildValues);
            seg.Add(new EDIXmlElement("QTY01", qual, buildValues));
            seg.Add(new EDIXmlElement("QTY02", qty.ToString(), buildValues));
            return seg;
        }

        public EDIXmlSegment GetCarrierDetail(string code, string carrier)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("CAD", buildValues);
            seg.Add(new EDIXmlElement("CAD01", code, buildValues));
            seg.Add(new EDIXmlElement("CAD02", "", buildValues));
            seg.Add(new EDIXmlElement("CAD03", "", buildValues));
            seg.Add(new EDIXmlElement("CAD04", "", buildValues));
            seg.Add(new EDIXmlElement("CAD05", carrier, buildValues));
            return seg;
        }

        public EDIXmlSegment GetFreightOnBoard(string code)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("FOB", buildValues);
            seg.Add(new EDIXmlElement("FOB01",code, buildValues));
            return seg;
        }

        public EDIXmlSegment GetInvoiceShipmentSummary(string uom, int unitsShipped)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("ISS", buildValues);
            seg.Add(new EDIXmlElement("ISS01", unitsShipped.ToString(), buildValues));
            seg.Add(new EDIXmlElement("ISS02", uom, buildValues));
            return seg;
        }

        public EDIXmlSegment GetMarksAndNumbersSegment(string qualifier, string numbers)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("MAN", buildValues);
            seg.Add(new EDIXmlElement("MAN01", qualifier, buildValues));
            seg.Add(new EDIXmlElement("MAN02", numbers, buildValues));
            return seg;
        }

        public EDIXmlSegment GetServiceAllowanceAndChargeSegment(string primaryCode, string secondaryCode, decimal amount)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("SAC", buildValues);
            seg.Add(new EDIXmlElement("SAC01", primaryCode, buildValues));
            seg.Add(new EDIXmlElement("SAC02", secondaryCode, buildValues));
            seg.Add(new EDIXmlElement("SAC03", "", buildValues));
            seg.Add(new EDIXmlElement("SAC04", "", buildValues));
            seg.Add(new EDIXmlElement("SAC05", amount.ToString(), buildValues));
            return seg;
        }

        public EDIXmlSegment GetPricingInformation(string priceIdCode, decimal unitPrice)
        {
            EdiXmlBuildValues buildValues = _buildFactory.GetValues();
            var seg = new EDIXmlSegment("CTP", buildValues);
            seg.Add(new EDIXmlElement("CTP01", "", buildValues));
            seg.Add(new EDIXmlElement("CTP02", priceIdCode, buildValues));
            seg.Add(new EDIXmlElement("CTP03", unitPrice.ToString(), buildValues));
            return seg;
        }
        private static void add_invoice_detail(EDIXmlSegment seg, 
            KeyValuePair<Qualifier, string> pair, 
            EdiXmlBuildValues buildValues, 
            ref int i)
        { 
            var elName1 = "IT1" + i.ToString("0#");
            i++;
            var elName2 = "IT1" + i.ToString("0#");
            i++;
            seg.Add(new EDIXmlElement(elName1,pair.Key.Value, buildValues));
            seg.Add(new EDIXmlElement(elName2, pair.Value,buildValues));
        }
    }
}