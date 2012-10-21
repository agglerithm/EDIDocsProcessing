using System;
using System.Data;
using System.IO;

namespace EDIDocsIn.Common.IO
{
    /// <summary>
    /// Summary description for FileSystemDataSet.
    /// </summary>
    ///  
    public class FileEntity
    {
        public string FullPath
        {
            get; set;
        }

        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(FullPath)) return "";
                return Path.GetFileName(FullPath);
            }
        }

        public string ContainingFolder
        {
            get
            {
                if (string.IsNullOrEmpty(FullPath)) return "";
                return Path.GetDirectoryName(FullPath);
            }
        }

        public int Size
        {
            get;
            set; 
        }

        public DateTime Timestamp
        {
            get;
            set; 
        }
    }

    public class FileSystemDataSet
    {
        protected DataTable tbl = new DataTable();
        protected string _path;
        protected string file_mask;
        protected DirectoryInfo dInfo;
        protected FileInfo[] fInfo;
        protected int numCols;
        public DateTime CutoffDate;
        public FileSystemDataSet()
        {
            //Place for storing the file name and size
            tbl.Columns.Add("Path");
            tbl.Columns.Add("Size");
            tbl.Columns.Add("dtMod",typeof(DateTime));
            numCols = 3;
        }

        public FileSystemDataSet(string dir, string fmask, string cutoff_date)
        {
            //Place for storing the file name and size
            tbl.Columns.Add("Path");
            tbl.Columns.Add("Size");
            tbl.Columns.Add("dtMod");
            numCols = 3;
            SearchPath = dir;
            try
            {
                FileMask = fmask;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            CutoffDate = DateTime.Parse(cutoff_date);
        }

        public DataTable Table
        {
            get 
            {
                LoadTable();
                return tbl;
            }
        }

        public string SearchPath
        {
            get { return _path; }
            set
            {
                _path = value;
                dInfo = new DirectoryInfo(_path);
            }

        }

        public string FileMask
        {
            get { return file_mask; }
            set
            {
                file_mask = value;
                fInfo = dInfo.GetFiles(file_mask);
            }
        }

        public void Find(string path, string mask)
        {
            SearchPath = path;
            FileMask = mask;
        }

        public int Count
        {
            get
            {
                return fInfo == null ? 0 : fInfo.Length;
            }
        }

        public string FileName(int Index)
        {
            if (Index < 0 || Index >= Count)
                return null;
            return fInfo[Index].Name;
        }

        public FileStream FileStream(int Index)
        {
            if (Index < 0 || Index >= Count)
                return null;
            return fInfo[Index].OpenRead();
        }

        public virtual void LoadTable()
        {
            if (Count == 0) return;
            tbl.Clear();
            var arr = new string[numCols];
            int count = Count;
            for (var i = 0; i < count; i++)
            {
                DateTime lastWriteTime = fInfo[i].LastWriteTime;
                if (lastWriteTime >= CutoffDate)
                {
                    arr[0] = fInfo[i].Name;
                    arr[1] = fInfo[i].Length.ToString();
                    arr[2] = lastWriteTime.ToString();
                    tbl.Rows.Add(arr);
                }
            }
        }

        protected string GetText(int Index)
        {
            var reader = FileStream(Index);
            if (reader == null) return "";
            var arr = new byte[fInfo[Index].Length];
            reader.Read(arr, 0, arr.Length);
            return ByteToString(arr);
        }

        protected static string ByteToString(byte[] arr)
        {
            return System.Text.Encoding.ASCII.GetString(arr, 0, arr.Length);
        }
    }
}