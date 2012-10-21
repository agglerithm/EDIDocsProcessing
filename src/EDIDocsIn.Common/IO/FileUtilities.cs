using System;
using System.Collections.Generic;
using System.IO;
using EDIDocsProcessing.Common.Extensions;

namespace EDIDocsProcessing.Common.IO
{
    public interface IFileUtilities
    {
        string LastError { get; set; }

//        DataTable GetTableFromTextFile(string path, string delim, bool header,
//                                       bool footer, int lines_in, int definition_line );
//
//        DataTable GetTableFromTextFile(string path, string delim, 
//                                       int header_line, int footer_line);
//
//        DataTable GetTableFromExcelFile(string path, string header,
//                                        string sheet_name, int sheet_ndx);

        //DataTable GetTableFromFolder(string path, string mask, DateTime cutoff);
        //void SaveTextFile(string path, string text);
        void MoveFileWithoutOverwrite(string source, string dest);
        void MoveFileWithOverwrite(string source, string dest);
        //void AppendToFile(string path, string text);
        //void CopyFileWithOverwrite(string source, string dest);
        IList<FileEntity> GetListFromFolder(string folder, string mask, DateTime cutoff);
        //void TouchFilesInFolder(string folder);
        //void CopyAllFiles(string source_folder, string dest_folder);

        void SaveTextAndRename(string contents, string working_name, string dest_name);

        void SaveText(string contents, string dest_name);
    }

    public class FileUtilities :  IFileUtilities
    {
        private readonly IFileSystemDataSet _fsds;
        public string LastError { get; set; }


//        public DataTable GetTableFromTextFile(string path, string delim, bool header,
//                                              bool footer, int lines_in, int definition_line )
//        {
//            string file_only = Path.GetFileName(path);
//            string path_only = path.Replace(file_only, "");
//            var tdl = new TextDataLink(path_only, delim.ToCharArray(), header, footer, lines_in, definition_line);
//            return tdl.GetData(file_only);
//        }

 


//        public DataTable GetTableFromTextFile(string path, string delim, 
//                                              int header_line, int footer_line)
//        {
//            string path_only;
//            string file_only = parse_file_path(path, out path_only);
//            var tdl = new TextDataLink(path_only, delim.ToCharArray(), header_line, footer_line);
//            return tdl.GetData(file_only);
//        }

//        private static string parse_file_path(string path, out string path_only)
//        {
//            string file_only = Path.GetFileName(path);
//            path_only = path.Replace(file_only, "");
//            return file_only;
//        }

//        public DataTable GetTableFromExcelFile(string path, string header,
//                                               string sheet_name, int sheet_ndx)
//        {  
//            var xdl = new ExcelDataLink(path, sheet_name, header, sheet_ndx);
//            return xdl.GetData();
//        }

        public FileUtilities(IFileSystemDataSet fsds)
        {
            _fsds = fsds;
        }
 

//        public void SaveTextFile(string path, string text)
//        {
//            Console.WriteLine("using path:" + path);
//
//            //Creates a new text file with contents "text" at path "path"
//            //  (Existing file is overwritten)
//            var sw = new StreamWriter(path, false);
//            sw.Write(text);
//            sw.Close();
//        }

        public void MoveFileWithoutOverwrite(string source, string dest)
        {
            //Console.WriteLine("using path:" + dest);

            if (File.Exists(dest))
            {
                string fname = Path.GetFileNameWithoutExtension(dest);
                string ext = Path.GetExtension(dest);
                string pth = dest.Replace(Path.GetFileName(dest), "");
                dest = pth + fname + "x" + ext;
            }
            File.Move(source, dest);
        }

        public void MoveFileWithOverwrite(string source, string dest)
        {
            //Console.WriteLine("using source path:" + source + " and dest path: " + dest);
            
            if (File.Exists(dest))
                File.Delete(dest);
            File.Move(source, dest);
        }
        public static string QuickFileText(string path)
        {
            var sr = new StreamReader(path);
            var qTxt = sr.ReadToEnd();
            sr.Close();
            return qTxt;
        }
 

//        public void CopyFileWithOverwrite(string source, string dest)
//        {
//            //C 
//            File.Copy(source,dest,true);
//        }

        public IList<FileEntity> GetListFromFolder(string folder, string mask, DateTime cutoff)
        {
            return _fsds.GetData(folder, mask, cutoff);
        }

//        public void TouchFilesInFolder(string folder)
//        {
//            var dir = Directory.GetFiles(folder); 
//            foreach (var file in dir)
//            { 
//                File.SetLastWriteTime(file, DateTime.Now);
//            }
//        }


        public void SaveTextAndRename(string contents, string working_name, string dest_name)
        {
            var file = File.Create(working_name);
            file.Write(contents.ToByteArray(),0,contents.Length);
            file.Close();
            MoveFileWithOverwrite(working_name,dest_name);
        }

        public void SaveText(string contents, string dest_name)
        {
            var file = File.Create(dest_name);
            file.Write(contents.ToByteArray(), 0, contents.Length);
            file.Close();
        }
    }
}