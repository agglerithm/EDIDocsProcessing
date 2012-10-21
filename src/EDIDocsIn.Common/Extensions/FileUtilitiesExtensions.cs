using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AFPST.Common.Services;

namespace EDIDocsProcessing.Common.Extensions
{
    public static class FileUtilitiesExtensions
    {
        public static void SaveTextAndRename(this IFileUtilities fileUtil, string contents, string workingName, string uploadName)
        {
            var file = File.Create(workingName);
            file.Write(contents.ToByteArray(), 0, contents.Length);
            file.Close();
            fileUtil.MoveFileWithOverwrite(workingName, uploadName);
        }

        public static string QuickFileText(string path)
        {
            var sr = new StreamReader(path);
            var qTxt = sr.ReadToEnd();
            sr.Close();
            return qTxt;
        }
    }
}
