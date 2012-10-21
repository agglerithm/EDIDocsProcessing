using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using AFPST.Common.Data;
using AFPST.Common.Extensions;
using AFPST.Common.IO;

namespace AFPST.Common.Services.imp
{
    public class FileUtilities : IFileUtilities
    {
        public string LastError { get; set; }


        public DataTable GetTableFromTextFile(string path, string delim, bool header,
                                              bool footer, int lines_in, int definition_line )
        {
            string file_only = Path.GetFileName(path);
            string path_only = path.Replace(file_only, "");
            var tdl = new TextDataLink(path_only, delim.ToCharArray(), header, footer, lines_in, definition_line);
            return tdl.GetData(file_only);
        }

 


        public DataTable GetTableFromTextFile(string path, string delim, 
                                              int header_line, int footer_line)
        {
            string path_only;
            string file_only = parse_file_path(path, out path_only);
            var tdl = new TextDataLink(path_only, delim.ToCharArray(), header_line, footer_line);
            return tdl.GetData(file_only);
        }

        private static string parse_file_path(string path, out string path_only)
        {
            string file_only = Path.GetFileName(path);
            path_only = path.Replace(file_only, "");
            return file_only;
        }

        public DataTable GetTableFromExcelFile(string path, string header,
                                               string sheet_name, int sheet_ndx)
        {  
            var xdl = new ExcelDataLink(path, sheet_name, header, sheet_ndx);
            return xdl.GetData();
        }

        public DataTable GetTableFromFolder(string path, string mask, DateTime cutoff)
        { 
            var fsds = new FileSystemDataSet(path,mask,cutoff.ToString());
            return fsds.Table;
        }

        public void SaveTextFile(string path, string text)
        {
            Console.WriteLine("using path:" + path);

            //Creates a new text file with contents "text" at path "path"
            //  (Existing file is overwritten)
            var sw = new StreamWriter(path, false);
            sw.Write(text);
            sw.Close();
        }

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

        public void AppendToFile(string path, string text)
        {
            //Console.WriteLine("using path:" + path);
            var fs = new FileStream(path, FileMode.Append);
            byte[] buff = text.ToByteArray();
            fs.Write(buff,0,buff.Length);
            fs.Close();
        }

        public void CopyFileWithOverwrite(string source, string dest)
        {
            //C 
            File.Copy(source,dest,true);
        }

        public List<FileEntity> GetListFromFolder(string folder, string mask, DateTime cutoff)
        {
            var tbl = GetTableFromFolder(folder, mask, cutoff);
            var lst = new List<FileEntity>();
            foreach(DataRow dr in tbl.Rows)
            {
                lst.Add(new FileEntity{FullPath = dr["Path"].ToString(), 
                    Size = dr["Size"].ToString().CastToInt(), Timestamp = dr["dtMod"].ToString().CastToDateTime()});
            }
            return lst;
        }

        public void TouchFilesInFolder(string folder)
        {
            var dir = Directory.GetFiles(folder); 
            foreach (var file in dir)
            { 
                File.SetLastWriteTime(file, DateTime.Now);
            }
        }

        public void CopyAllFiles(string source_folder, string dest_folder)
        {
            DataTable tbl = this.GetTableFromFolder(source_folder, "*", DateTime.Today.AddYears(-20));
            foreach (DataRow row in tbl.Rows)
                CopyFileWithOverwrite(source_folder + Path.GetFileName(row["Path"].ToString()),
                                      dest_folder + Path.GetFileName(row["Path"].ToString()));
        }
    }
}