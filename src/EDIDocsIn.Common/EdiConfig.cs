using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using AFPST.Common.Infrastructure;
using AFPST.Common.Infrastructure.impl;
using EDIDocsProcessing.Common.Extensions;

namespace EDIDocsProcessing.Common
{
    public class EdiConfig
    {
        private static IAFPSTConfiguration _config;

        private static void ensureInitialized()
        {
            if (_config != null)
                return;
            _config = new AFPSTConfiguration();
        }

         public static bool RecreateTables( )
        {
             ensureInitialized();
             return _config.RecreateTables();  
        }

        public static string EmailServiceEndpoint( )
        {   
             ensureInitialized();
            return _config.EmailServiceEndpoint( );   }

        public static string WorkingFolder( ) 
        {   
             ensureInitialized();
            return _config.WorkingFolder( ) ;   }

        public static string ArchiveFolder( ) 
        {   
             ensureInitialized();
            return _config.ArchiveFolder( ) ;   }

        public static string GetUploadFolderFor(string partnerCode)
        { 
             ensureInitialized();
           return _config.GetUploadFolderFor( partnerCode);
        }

        public static string GetDownloadFolderFor(TransmissionPath trans)
        {
            ensureInitialized();
            var key = string.Format("{0}DownloadFolder", trans);
            return _config.GetSettingBasedOnTestMode(key); 
        }
        public static string GetDownloadFolderFor(string partnerCode)
        {
            ensureInitialized();
            return _config.GetSettingBasedOnTestMode(string.Format("{0}DownloadFolder", partnerCode));
        }
        public static string DownloadFolder()
        {
            
             ensureInitialized();
            return _config.DownloadFolder();
        }

        public static string GetSenderIdFor(BusinessPartner partner)
        {
            ensureInitialized();
            return _config.GetSettingBasedOnTestMode(string.Format("{0}InterchangeReceiverID", partner.Code));
        }
    }
}
