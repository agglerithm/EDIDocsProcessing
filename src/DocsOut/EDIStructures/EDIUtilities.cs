using System;
using System.Xml;
using System.Xml.Linq;
using EDIDocsProcessing.Core.DocsOut.EDIStructures;

namespace EDIDocsProcessing.Core.DocsOut.EDIStructures
{
    /// <summary>
    /// Summary description for EDIUtil.
    /// </summary>
    public class EDIUtilities
    {
        public EDIUtilities()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string GetCurrentDate(int el_id)
        {
            return EDIDateFromDate(DateTime.Today,   el_id);
        }

        public static string GetCurrentTime(int el_id)
        {
            return ConvertTimeToEDITime(DateTime.Today.ToString(), el_id);
        }

        public static DateTime DateFromEDIDateAndTime(string dt_val, int dt_id,
                                                      string tm_val, int tm_id)
        {
            DateTime dte = DateTime.MinValue;
            if (string.IsNullOrEmpty(dt_val)) return dte;
            if (dt_val == "*") return dte;
            if (string.IsNullOrEmpty(tm_val))
                tm_val = "000000";
            tm_val = ConvertEDITimeToTime(tm_val, tm_id);
            int pos = dt_val.IndexOf("/"); 
            string str = dt_val; 
            if (dt_id == EDIXmlElement.DA_DATE)
                dte = DateTime.Parse(str.Substring(2, 2) + "/"
                                     + str.Substring(4, 2) + "/" + str.Substring(0, 2) + " " + tm_val);
            else
                dte = DateTime.Parse(str.Substring(4, 2) + "/"
                                     + str.Substring(6, 2) + "/" + str.Substring(0, 4) + " " + tm_val);
            return dte;
        }

        public static string EDIDateAndTimeFromDate(DateTime dte, ref  string dt_val, int dt_id,
                                                    ref string tm_val, int tm_id)
        {
            tm_val = ConvertTimeToEDITime(dte.ToString("hh:mm:ss"),tm_id);

            dt_val = EDIDateFromDate(dte, dt_id);

            return dt_val;
        }


        public static DateTime DateFromEDIDate(string val, int el_id)
        {
            DateTime dte = DateTime.MinValue;
            if (string.IsNullOrEmpty(val)) return dte;
            if (val == "*") return dte;
            int pos = val.IndexOf("/"); 
            if (pos > 8) return dte;
            string str = val;
            if (el_id == EDIXmlElement.DA_DATE)
                return DateTime.Parse(str.Substring(2, 2) + "/"
                                      + str.Substring(4, 2) + "/" + str.Substring(0, 2));
            return DateTime.Parse(str.Substring(4, 2) + "/"
                                  + str.Substring(6, 2) + "/" + str.Substring(0, 4));
  
        }

        public static string EDIDateFromDate(DateTime dte, int el_num)
        { 
            if (el_num == EDIXmlElement.DA_DATE)
                return dte.ToString("yyMMdd");
            return dte.ToString("yyyyMMdd");
        }

        public static string ConvertEDITimeToTime(string val,  int el_num)
        {
            if (string.IsNullOrEmpty(val)) return "";
            if (val == "*") return val;
            int pos = val.IndexOf(" "); 
            if (pos > 0) return val;
            string str = val;
            if (el_num == EDIXmlElement.DA_TIME)
                return str.Substring(0, 2) + ":"
                       + str.Substring(2, 2);
            return str.Substring(0, 2) + ":"
                   + str.Substring(2, 2) + ":" + str.Substring(4, 2); 
        }

        public static string ConvertTimeToEDITime(string val, int el_num)
        {  
            if (el_num == EDIXmlElement.DA_TIME)
                return DateTime.Parse(val).ToString("hhmm");
            return DateTime.Parse(val).ToString("hhmmss");
        }

        public static void Clean(XElement root, bool dirty)
        {
            if (!dirty)
                root.Remove();
        }

        public static string AddLeftPadding(string buf, int len, char padchar)
        {
            if (buf.Length > len) return buf.Substring(0, len);
            int pad = len - buf.Length;
            for (int i = 0; i < pad; i++)
                buf = padchar.ToString() + buf;
            return buf;
        }

        public static string AddRightPadding(string buf, int len, char padchar)
        {
            if (buf.Length > len) return buf.Substring(0, len);
            int pad = len - buf.Length;
            for (int i = 0; i < pad; i++)
                buf += padchar.ToString();
            return buf;
        }

        public static string GetElementName(string segment_name, int element_index)
        {
            return segment_name + element_index.ToString("0#");
        }

        public static bool IsDateFormat(string str)
        {
            DateTime dte;
            return DateTime.TryParse(str, out dte); 
        }

        public static string CorrectForDate(string val, int elnum)
        {
            if (IsDateFormat(val))
            {
                switch (elnum)
                {
                    case EDIXmlElement.DATE:
                        val =  EDIDateFromDate(DateTime.Parse(val),   elnum) ;
                        break;
                    case EDIXmlElement.DA_DATE:

                        val = EDIDateFromDate(DateTime.Parse(val), elnum);
                        break;
                    case EDIXmlElement.DA_TIME:
                        val =  ConvertTimeToEDITime(val, elnum);
                        break;
                    case EDIXmlElement.TIME:
                        val = ConvertTimeToEDITime(val,   elnum);
                        break;
                }
            }
            return val;
        }
    }
}