namespace EDIDocsIn
{
    using System;
    using System.Linq;
    using AFPST.Common.Services.Logging;
    using EDIDocsProcessing.Common;
    using EDIDocsProcessing.Common.Extensions;
    using EdiMessages;
    public class EdiDocsInWorker
    {
        private readonly IEdiFileProcessingService  _ediFileProcessingService;

        public EdiDocsInWorker(IEdiFileProcessingService  ediFileProcessingService)
        {
            _ediFileProcessingService = ediFileProcessingService;
        }

        public void DoWork(Object state)
        {       
            try
            {
                processFilesFor(TransmissionPath.Initech);
                processFilesFor(TransmissionPath.Edict); 
            }
            catch (Exception ex)
            {
                Logger.Error(this, "Failed processing incoming files", ex);
            }
        }


        private void processFilesFor(TransmissionPath path)
        {
            var msgs =  _ediFileProcessingService.ProcessFiles(path);

            msgs.ForEach(publish); 

            int messageCount = msgs.Count();

            if (messageCount > 0) Logger.Info(this, messageCount + " messages published.");
        }

        private void publish(IEdiMessage obj)
        {
            obj.Publish();
        } 
    }
}