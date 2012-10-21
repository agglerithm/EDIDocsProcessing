using System;
using System.Collections.Generic;
using System.IO;

namespace EDIDocsProcessing.Common.IO
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

    public interface IFileSystemDataSet
    {
        IList<FileEntity> Table { get; }
        string SearchPath { get; set; }
        string FileMask { get; set; }
        int Count { get; }
        void LoadTable();
        IList<FileEntity> GetData(string path, string mask, DateTime cutoff);
    }

    public class FileSystemDataSet : IFileSystemDataSet
    {
        private string _path;
        private string _file_mask;
        private readonly IList<FileEntity> _files = new List<FileEntity>();
        private DirectoryInfo _dInfo;
        private FileInfo[] _fInfo;
        public DateTime CutoffDate
        {
            get; private set;
        }
//        public FileSystemDataSet()
//        {
//            //Place for storing the file name and size
//            tbl.Columns.Add("Path");
//            tbl.Columns.Add("Size");
//            tbl.Columns.Add("dtMod",typeof(DateTime));
//            numCols = 3;
//        }

//        public FileSystemDataSet(string dir, string fmask, string cutoff_date)
//        {
//            //Place for storing the file name and size
//            tbl.Columns.Add("Path");
//            tbl.Columns.Add("Size");
//            tbl.Columns.Add("dtMod");
//            numCols = 3;
//            SearchPath = dir;
//            try
//            {
//                FileMask = fmask;
//            }
//            catch(Exception ex)
//            {
//                throw ex;
//            }
//            CutoffDate = DateTime.Parse(cutoff_date);
//        }

        public IList<FileEntity> Table
        {
            get 
            {
                LoadTable();
                return _files;
            }
        }

        public string SearchPath
        {
            get { return _path; }
            set
            {
                _path = value;
                _dInfo = new DirectoryInfo(_path);
            }

        }

        public string FileMask
        {
            get { return _file_mask; }
            set
            {
                _file_mask = value;
                _fInfo = _dInfo.GetFiles(_file_mask);
            }
        }

//        public void Find(string path, string mask)
//        {
//            SearchPath = path;
//            FileMask = mask;
//        }

        public int Count
        {
            get
            {
                return _fInfo == null ? 0 : _fInfo.Length;
            }
        }

 

        public virtual void LoadTable()
        {
            _files.Clear(); 
            if (Count == 0) return;
            foreach(FileInfo file in _fInfo)
            {
                DateTime lastWriteTime = file.LastWriteTime;
                if (lastWriteTime >= CutoffDate)
                {
                    _files.Add(new FileEntity() {  FullPath = Path.Combine(SearchPath, file.Name),
                    Size = (int)file.Length, Timestamp = lastWriteTime}); 
                }
            }
        }

        public IList<FileEntity> GetData(string path, string mask, DateTime cutoff)
        {
            
            SearchPath = path;
            FileMask = mask;
            CutoffDate = cutoff; 
            return Table;
        }

 
    }
}