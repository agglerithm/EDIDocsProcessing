using System;
using System.Data;

namespace EDIDocsIn.Common.impl
{
    /// <summary>
    /// Summary description for ExcelDataLink.
    /// </summary>
    public class ExcelDataLink : OLEDBWrapper
    {
        protected string file_name;
        protected string current_sht_name = "";
        protected int current_sht_ndx = -1;
        readonly string header = "";
        protected string[] table_names;
        public ExcelDataLink()
        {

        }

        public ExcelDataLink(string fileName, string sheet, string hdr,
                             int sheet_ndx)
        {
            file_name = fileName;
            current_sht_name = sheet;
            current_sht_ndx = sheet_ndx;
            header = hdr;
        }

        public string[] Tables
        {
            get { return table_names; }
        }

        public DataTable GetData()
        {
            if (current_sht_ndx < 0)
                return GetData(file_name, current_sht_name,
                               header); 
            return GetData(file_name, current_sht_ndx,
                           header);
        }
        public DataTable GetData(string fileName, string sheet, string hdr)
        {
            Refresh(fileName, hdr); 
            return RecordSet("SELECT x.* from [" + sheet + "$] x");
        }

        public DataTable GetData(string fileName, int sheet_ndx, string hdr)
        {
            Refresh(fileName, hdr);
            DataTable stbl = cnn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables_Info, null);
            table_names = new string[stbl.Rows.Count];
            for (int i = 0; i < stbl.Rows.Count; i++)
            {
                table_names[i] = stbl.Rows[i][2].ToString();
            }
            return RecordSet("SELECT x.* from [" + table_names[sheet_ndx] + "] x");
        }

        public void Refresh(string filename, string hdr)
        {
            if (hdr == "")
                hdr = "No";
            string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" 
                             + filename + ";Extended Properties=\"Excel 8.0;HDR=" 
                             + hdr + ";IMEX=1\"";
            try
            {
                Refresh(connStr);
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't open file:" + filename + "; " + connStr + ";" + ex.Message);
            }
        }

        public DataTable GetData(string sheet)
        { 
            return RecordSet("SELECT x.* from [" + sheet + "$] x");
        }
    }
}