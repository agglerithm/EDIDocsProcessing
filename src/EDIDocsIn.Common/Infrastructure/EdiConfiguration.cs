using System;
using System.Configuration;
using System.IO;

namespace EDIDocsProcessing.Common.Infrastructure
{
    public interface IAFPSTConfiguration
    {
        bool TestMode { get; }
        bool RecreateTables { get; }
        string WorkingFolder { get;  }
        string ArchiveFolder { get; } 
        string DownloadFolder { get; }
        string ElementDelimiter { get; }
        string SegmentDelimiter { get; }
        string InterchangeQualifier { get; }
        string FunctionGroupReceiverID { get; }
        string InterchangeReceiverID { get; }
        string EmailServiceEndpoint { get; }
        string GetSettingBasedOnTestMode(string key);
        string GetUploadFolderFor(string partnerCode);
    }

    public class AFPSTConfiguration : IAFPSTConfiguration
    {
        public bool TestMode
        {
            get { return ConfigurationManager.AppSettings["TestMode"] != "0"; }
        }

        public bool RecreateTables
        {
            get { return ConfigurationManager.AppSettings["RecreateTables"] != "0"; }
        }

        public string WorkingFolder { get { return GetSettingBasedOnTestMode("WorkingFolder"); } }
        public string ArchiveFolder { get { return GetSettingBasedOnTestMode("ArchiveFolder"); } }
        public string GetUploadFolderFor(string partnerCode) 
        {   return GetSettingBasedOnTestMode(string.Format("{0}UploadFolder",partnerCode));  }
        public string DownloadFolder
        {
            get
            {
                var folder = GetSettingBasedOnTestMode("DownloadFolder");
                if (Directory.Exists(folder)) return folder;
                
                //Folder doesn't exist, maybe we're running an Integration Test
                //Let's See if we can figure it out.
                bool isRealativePath = folder.StartsWith(".");
                bool isRunningTestInVisualStudio = Directory.GetCurrentDirectory().EndsWith("Debug");

                if (isRunningTestInVisualStudio && isRealativePath)
                {
                    //Let's try babacking up a couple directories
                    var newfolder = @"..\..\" + folder;
                    if (Directory.Exists(newfolder)) return newfolder;
                }

                throw new ConfigurationErrorsException(string.Format("The DownloadFolder '{0}' does not exist.", folder));
            }
        }
        public string ElementDelimiter { get { return GetSettingBasedOnTestMode("ElementDelimiter"); } }
        public string SegmentDelimiter { get { return GetSettingBasedOnTestMode("SegmentDelimiter"); } }
        public string InterchangeQualifier { get { return GetSettingBasedOnTestMode("InterchangeQualifier"); } }
        public string FunctionGroupReceiverID { get { return GetSettingBasedOnTestMode("FunctionGroupReceiverID"); } }
        public string InterchangeReceiverID { get { return GetSettingBasedOnTestMode("InterchangeReceiverID"); } }
        public string EmailServiceEndpoint { get { return GetSettingBasedOnTestMode("EmailServiceEndpoint"); } }
        public string GetSettingBasedOnTestMode(string key)
        {
            if (TestMode)
            {
                var setting = ConfigurationManager.AppSettings[key + "Test"];
                if (setting != null) return setting;
            }
            return ConfigurationManager.AppSettings[key];
        }
    }
}