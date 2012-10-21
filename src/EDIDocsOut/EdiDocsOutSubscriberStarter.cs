using System.Collections.Generic;
using AFPST.Common.Infrastructure;
using AFPST.Common.Services.Logging;
using EDIDocsProcessing.Core;
using EdiMessages;

namespace EDIDocsOut
{

    [CoverageExclude("code not interesting","Greg")]
    public class EdiDocsOutSubscriberStarter : ISubscriptionService
    {
        public void Start(bool runOnce)
        { 
            Logger.Info(this, "Starting EdiDocsOut service.");
        }

 

        public void Stop()
        {
            Logger.Info(this, "Stopping."); 
        }

    }
}