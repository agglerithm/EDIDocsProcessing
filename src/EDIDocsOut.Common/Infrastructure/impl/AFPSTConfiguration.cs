using System;
using System.Configuration;


namespace AFPST.Common.Infrastructure.impl
{
    public class AFPSTConfiguration : IAFPSTConfiguration
    {
        private bool? _testmodeOverride = null;

        public string GetSettingBasedOnTestMode(string key)
        {
            if (TestMode)
            {
                var setting = ConfigurationManager.AppSettings[key+"Test"];
                if (setting != null) return setting;
            }
            return ConfigurationManager.AppSettings[key];
        }

        public string GetSettingBasedOnERP(string key, string ERP)
        {
            var setting = ConfigurationManager.AppSettings[key + ERP];
            if (setting != null) return setting;
            return ConfigurationManager.AppSettings[key];
        }

        public string EncodedUserName
        {
            get { return ConfigurationManager.AppSettings["EncodedUserName"]; }
        }

        public string EncodedPassword
        {
            get { return ConfigurationManager.AppSettings["EncodedPassword"]; }
        }

        public string WWTTempPath
        {
            get { return ConfigurationManager.AppSettings["WWTTempPath"]; }
        }

        public bool TestMode
        {
            get
            {
                if (_testmodeOverride.HasValue) return _testmodeOverride.Value;
                return ConfigurationManager.AppSettings["TestMode"] != "0";
            }
        }

        public int AppID
        {
            get { return int.Parse(ConfigurationManager.AppSettings["AppNum"]); }
        }

        public string ConnectionString
        {
            get
            {
                if (TestMode)
                    return ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
                return ConfigurationManager.ConnectionStrings["Production"].ConnectionString;
            }
        }

        public string GetDBServer(string type)
        {
            return GetSettingBasedOnTestMode(type + "Server");    
        }

        [Obsolete("This is for testing only")]
        public string WWTUploadURLTest
        {
            get { return ConfigurationManager.AppSettings["WWTUploadURLTest"]; }
        }

        [Obsolete("This is for testing only")]
        public string WWTOutputPathTest
        {
            get { return ConfigurationManager.AppSettings["WWTOutputPathTest"]; }
        }

        [Obsolete("This is for testing only")]
        public void SetTestModeOverride(bool? testMode)
        {
            _testmodeOverride = testMode;
        }


    }
}