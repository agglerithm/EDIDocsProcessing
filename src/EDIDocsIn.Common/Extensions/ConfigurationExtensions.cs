using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using AFPST.Common.Infrastructure;

namespace EDIDocsProcessing.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static bool RecreateTables(this IAFPSTConfiguration config)
        {
             return config.GetSettingBasedOnTestMode("RecreateTables")   != "0";  
        }

        public static string EmailServiceEndpoint(this IAFPSTConfiguration config)
        {   return config.GetSettingBasedOnTestMode("EmailServiceEndpoint");   }

        public static string WorkingFolder(this IAFPSTConfiguration config) 
        {   return config.GetSettingBasedOnTestMode("WorkingFolder");   }

        public static string ArchiveFolder(this IAFPSTConfiguration config) 
        {   return config.GetSettingBasedOnTestMode("ArchiveFolder");   }

        public static string GetUploadFolderFor(this IAFPSTConfiguration config,string partnerCode)
        {
            return config.GetSettingBasedOnTestMode(string.Format("{0}UploadFolder", partnerCode));
        }

        public static string GetUploadFolderFor(this IAFPSTConfiguration config, TransmissionPath path)
        {
            return config.GetSettingBasedOnTestMode(string.Format("{0}UploadFolder", path)); 
        }
        public static string DownloadFolder(this IAFPSTConfiguration config)
        {
            
                var folder = config.GetSettingBasedOnTestMode("DownloadFolder");
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
}
