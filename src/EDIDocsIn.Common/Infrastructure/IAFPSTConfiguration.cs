using System;

namespace AFPST.Common.Infrastructure
{
    public interface IAFPSTConfiguration
    {
        string GetSettingBasedOnTestMode(string key);
        string GetSettingBasedOnERP(string key, string ERP);
        string EncodedUserName { get; }
        string EncodedPassword { get; }
        string WWTTempPath { get; }
        bool TestMode { get; }
        int AppID { get; }
        [Obsolete("This is for testing only")]
        string WWTUploadURLTest { get; }
        [Obsolete("This is for testing only")]
        string WWTOutputPathTest { get; }
        [Obsolete("This is for testing only")]
        void SetTestModeOverride(bool? testMode);
        string ConnectionString { get;  }
        string GetDBServer(string type);
    }
}