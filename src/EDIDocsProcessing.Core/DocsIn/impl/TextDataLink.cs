using System;
using System.Data;
using System.IO;

namespace EDIDocsIn.Common.impl
{
    public class TextDataLink
    {
        private readonly string _path;
        protected int def_line;
        protected bool delim;
        protected char[] delStr;
        protected int footer_line = -1;
        protected bool ftr;
        protected bool hdr;
        protected int header_line = -1;
        protected int offset;
        protected string strFooter;
        protected string strHeader;

//        public TextDataLink(string path, char[] delimiter, bool header,
//                            bool footer, int lines_to_skip)
//        {
//            _path = path;
//            offset = lines_to_skip;
//            def_line = offset;
//            if (delimiter == null)
//                delim = false;
//            else
//            {
//                delim = true;
//                delStr = delimiter;
//            }
//
//            hdr = header;
//            ftr = footer;
//        }

        public TextDataLink(string path, char[] delimiter, bool header,
                            bool footer, int lines_to_skip, int definition_line)
        {
            _path = path;
            offset = lines_to_skip;
            def_line = definition_line;
            if (delimiter == null)
                delim = false;
            else
            {
                delim = true;
                delStr = delimiter;
            }

            hdr = header;
            ftr = footer;
        }

        public TextDataLink(string path, char[] delimiter, int headerLine, int footerLine)
        {
            if (delimiter == null)
                delim = false;
            else
            {
                delim = true;
                delStr = delimiter;
            }
            _path = path;
            footer_line = footerLine;
            header_line = headerLine;
            hdr = true;
            ftr = true;
        }

        public string Footer
        {
            get { return strFooter; }
        }

        public string Header
        {
            get { return strHeader; }
        }

        public virtual DataTable GetData(string filename)
        {
            try
            {
                if (!delim) return null;
                string[] buff = File.ReadAllLines(_path + filename);
                buff = CleanLines(buff);
                if (header_line >= 0)
                    return GetArbitraryData(buff);
                DataTable tbl = GetTableStructure(buff);
                string[] line;
                for (int i = offset; i < buff.Length; i++)
                {
                    if (ftr && i == buff.Length - 1)
                        strFooter = buff[i];
                    else
                    {
                        line = buff[i].Split(delStr);
                        tbl.Rows.Add(line);
                    }
                }
                return tbl;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public virtual DataTable GetArbitraryData(string[] buff)
        {
            try
            {
                buff = CleanLines(buff);
                def_line = header_line;
                offset = header_line + 1;
                DataTable tbl = GetTableStructure(buff);
                if(hdr)
                    strHeader = buff[header_line];
                if(ftr)
                    strFooter = buff[footer_line];
                for (int i = offset; i < buff.Length; i++)
                {
                    {
                        if (i != header_line && i != footer_line)
                        {
                            string[] line = buff[i].Split(delStr);
                            tbl.Rows.Add(line);
                        }
                    }
                }
                return tbl;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

//            public DataTable GetData(string filename, int[] widths)
//            {
//                if (delim) return GetData(filename);
//                string[] buff = System.IO.File.ReadAllLines(connection_string + filename);
//                buff = CleanLines(buff);
//                string colname; 
//                tbl = new DataTable();
//                int i; 
//                string [] line = null;
//                if (hdr)
//                {
//                    strHeader = buff[offset];
//                    line = SliceLine(buff[offset], widths); 
//                    offset++;
//                }
//                for (i = 0; i < widths.Length; i++)
//                {
//                    if(hdr) colname = line[i];
//                    else colname = "column" + i.ToString("00");
//                    tbl.Columns.Add(colname);
//                }
//                for (i = offset; i < buff.Length; i++)
//                {
//                    if (ftr && i == buff.Length - 1)
//                        strFooter = buff[i];
//                    else
//                    {
//                        line = SliceLine(buff[i], widths);
//                        tbl.Rows.Add(line);
//                    }
//                }
//                return tbl;
//
//            }

//            protected string[] SliceLine(string line, int[] widths)
//            {
//                string[] arr = new string[widths.Length];
//                int pos = 0;
//                for (int i = 0; i < arr.Length; i++)
//                {
//                    arr[i] = line.Substring(pos, widths[i]).Trim();
//                    pos += widths[i];
//                }
//                return arr;
//            }

        protected virtual DataTable GetTableStructure(string[] lines)
        {
            int i;
            string header = "";
            if (def_line < 0)
            {
                hdr = false;
                def_line = 0;
            }
            header = lines[def_line];
            string[] cols = header.Split(delStr);
            var tmp = new DataTable();
            if (hdr) offset++;
            for (i = 0; i < cols.Length; i++)
            {
                string colname;
                if (hdr) colname = cols[i];
                else colname = "column" + i.ToString("00");
                tmp.Columns.Add(colname);
            }
            return tmp;
        }

        protected string[] CleanLines(string[] lines)
        {
            int i;
            int good_lines = 0;
            for (i = 0; i < lines.Length; i++)
            {
                if (lines[i] != null && lines[i].Trim() != "")
                {
                    lines[good_lines] = lines[i];
                    good_lines++;
                }
                else if (i <= offset) offset--;
            }
            var temp = new string[good_lines];
            for (i = 0; i < good_lines; i++)
                temp[i] = lines[i];
            return temp;
        }

        public bool BooleanScalar(string SQL)
        {
            DataTable tab = GetData(SQL);
            bool ret_val = bool.Parse(tab.Rows[0][0].ToString());
            return ret_val;
        }

        public void Execute(string SQL)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object ExecuteWithKeyValue(string SQL)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int IntScalar(string SQL)
        {
            DataTable tab = GetData(SQL);
            int ret_val = Int32.Parse(tab.Rows[0][0].ToString());
            return ret_val;
        }

        public long LongScalar(string SQL)
        {
            DataTable tab = GetData(SQL);
            long ret_val = long.Parse(tab.Rows[0][0].ToString());
            return ret_val;
        }

        public DataTable RecordSet(string SQL)
        {
            return GetData(SQL);
        }

        public void Refresh(string connectionString, bool header)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string StringScalar(string SQL)
        {
            DataTable tab = GetData(SQL);
            string ret_val = tab.Rows[0][0].ToString();
            return ret_val;
        }
    }
}