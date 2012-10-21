using System.Configuration;
using AFPST.Common.Infrastructure;
using AFPST.Common.Infrastructure.impl;
using Castle.Core;

namespace EDIDocsProcessing.Common
{
    public class CommonSetup
    {
        public static void Setup()
        {
            bool testMode = ConfigurationManager.AppSettings["TestMode"] != "0";
            var lifestyleType = LifestyleType.Transient;
            Container.Register<IAFPSTConfiguration, AFPSTConfiguration>("config", lifestyleType);
 
        }
    }
}
