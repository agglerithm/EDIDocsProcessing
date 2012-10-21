using System.Xml.Linq; 

namespace EDIDocsProcessing.Core.DocsOut.EDIStructures
{
    public class EDIXmlElement : EDIXmlNode
    {
//        public const int CITY_NAME = 19;
//        public const int GS_CONTROL_NUM = 28;
        public const int DA_DATE = 29;
        public const int DA_TIME = 30;
//        public const int EQUIPMENT_DESC_CODE = 40;
//        public const int HEIGHT = 65;
//        public const int ID_CODE_QUALIFIER = 66;
//        public const int ID_CODE = 67;
//        public const int INVOICE_NO = 76;
//        public const int LADING_QTY = 80;
//        public const int WEIGHT = 81;
//        public const int LENGTH = 82;
//        public const int MARKS_AND_NUMBERS = 87;
//        public const int MARKS_AND_NUMBERS_QUALIFIER = 88;
//        public const int TRANSPORT_TYPE_CODE = 91;
//        public const int PO_TYPE_CODE = 92;
//        public const int NAME = 93;
//        public const int SEG_COUNT = 96;
//        public const int ENTITY_IDENTIFIER = 98;
//        public const int CURRENCY_CODE = 100;
//        public const int PACKAGING_CODE = 103;
//        public const int POSTAL_CODE = 116;
//        public const int APP_RECEIVE_CODE = 124;
//        public const int REFERENCE_NUMBER = 127;
//        public const int REFERENCE_NUMBER_QUALIFIER = 128;
//        public const int APP_SEND_CODE = 142;
//        public const int TRANSACTION_SET_ID_CODE = 143;
//        public const int SHIPMENT_PAYMENT_METHOD = 146;
//        public const int STATE_OR_PROVINCE_CODE = 156;
//        public const int ADDRESS_INFORMATION = 166;
//        public const int WEIGHT_QUALIFIER = 187;
//        public const int WIDTH = 189;
//        public const int EQUIPMENT_INITIAL = 206;
//        public const int EQUIPMENT_NUMBER = 207;
//        public const int UNIT_PRICE = 212;
//        public const int SEAL_NUMBER = 225;
//        public const int PRODSERV_ID = 234;
//        public const int PRODSERV_ID_QUALIFIER = 235;
//        public const int ALLOWANCE_CHG_INDICATOR = 248;
//        public const int SERVICE_LVL_CODE = 284;
//        public const int LOCATION_QUALIFIER = 309;
//        public const int PO_NUMBER = 324;
//        public const int REQUEST_REF_NUM = 326;
//        public const int CHANGE_ORDER_SEQ = 327;
//        public const int PO_RELEASE_NUMBER = 328;
//        public const int TRANSACTION_SET_CONTROL_NUMBER = 329;
//        public const int QUANTITY_ORDERED = 330;
//        public const int TERMS_TYPE_CODE = 336;
        public const int TIME = 337;
//        public const int TERMS_DISCOUNT_PERCENT = 338;
//        public const int HASH_TOTAL = 347;
//        public const int ITEM_DESCRIPTION_TYPE = 349;
//        public const int ASSIGNED_IDENTIFICATION = 350;
//        public const int TERMS_DISCOUNT_DAYS_DUE = 351;
//        public const int DESCRIPTION = 352;
//        //This has a min-length of 1 and max-length of 80, so should accommodate anything
//        public const int UNKNOWN = 352;
//        public const int PURPOSECODE = 353;
//        public const int NUMBER_OF_LINE_ITEMS = 354;
//        public const int UNIT_OF_MEASURE_CODE = 355;
//        public const int PACK_COUNT = 356;
//        public const int PACK_SIZE = 357;
//        public const int QUANTITY_INVOICED = 358;
//        public const int COMMUNICATION_NUMBER = 364;
//        public const int COMMUNICATION_NUMBER_QUALIFIER = 365;
//        public const int CONTACT_FUNCTION_CODE = 366;
        public const int DATE = 373;
//        public const int DATETIME_QUALIFIER = 374;
//        public const int SCHED_QTY = 380;
//        public const int NUM_UNITS_SHIPPED = 382;
//        public const int TERMS_NET_DAYS = 386;
//        public const int ROUTING = 387;
//        public const int SHIPMENTID = 396;
//        public const int UNIT_COUNT = 405;
//        public const int RESPONSIBLE_AGENCY_CODE = 455;
//        public const int FUNCTIONAL_ID = 479;
//        public const int GS_VERSION = 480;
//        public const int AMT_QUALIFIER_CODE = 522;
//        public const int AGENCY_QUAL_CODE = 559;
//        public const int SAC_AMT = 601;
//        public const int TDS_AMT = 610;
//        public const int HL_ID = 628;
//        public const int BASIS_OF_UP_CODE = 639;
//        public const int TRANSACTION_TYPE_CODE = 640;
//        public const int RELATIONSHIP_CODE = 662;
//        public const int LINE_ITEM_STATUS_CODE = 668;
//        public const int CHANGE_TYPE_CODE = 670;
//        public const int QTY_LEFT_TO_REC = 671;
//        public const int INTERCHANGE_VERSION_ID = 703;
//        public const int ISA_ID_QUALIFIER = 704;
//        public const int ISA_SEND_ID = 705;
//        public const int ISA_REC_ID = 706;
//        public const int INTERCHANGE_CONTROL_NUM = 709;
//        public const int ISA_STANDARDS_ID = 726;
//        public const int HL_PARENT_ID = 734;
//        public const int HL_LEVEL_CODE = 735;
//        public const int AUTH_INFO_QUALIFIER = 744;
//        public const int AUTH_INFO = 745;
//        public const int SECURITY_INFO_QUAL = 746;
//        public const int SECURITY_INFO = 747;
//        public const int TEST_INDICATOR = 748;
//        public const int ACK = 749;
//        public const int MONETARY_AMT = 782;
//        public const int INNER_PACK_COUNT = 810;
        //public const int SOURCE_SUBQUAL = 822;
        //public const int PALLET_TYPE = 883;
        //public const int MESSAGE = 933;
        //public const int TAX_TYPE_CODE = 963;
        //public const int HIERARCHICAL_STRUCT_CODE = 1005;
        //public const int INDUSTRY_CODE = 1271;
        //public const int SAC_CODE = 1300;
 
        public EDIXmlElement(string name, string value )
            : base("ediElement")
        {
            Label = name;
            Add(new XElement("delimiter", EDIXmlBuildValues.ElementDelimiter));
            Add(new XElement("elementValue", value));
        }
        public EDIXmlElement(XElement el)
            : base(el)
        {

        }

        public new string Value
        {
            get { return test_element("elementValue"); }
            set { set_element("elementValue", value); }
        }


    }
}