using EDIDocsProcessing.Common.Publishers;
using EdiMessages;
using Microsoft.Practices.ServiceLocation;
using System.Threading;
using EDIDocsProcessing.Common; 
using EDIDocsProcessing.Core;
using InitechEDIDocsIn;

namespace EDIDocsIn
{
    public class EdiDocsInService : ISubscriptionService
    {
        private readonly IEdiMessagePublisher _messagePublisher;
        private readonly IEdiFileProcessingService  _ediFileProcessingService;

        private EdiDocsInWorker _ediDocsInWorker;
        private Timer _timer;

        
        public EdiDocsInService()
            : this(ServiceLocator.Current.GetInstance<IEdiFileProcessingService >(), ServiceLocator.Current.GetInstance<IEdiMessagePublisher>())
        {
        }

        public EdiDocsInService(IEdiFileProcessingService  ediFileProcessingService,
                                IEdiMessagePublisher messagePublisher)
        {
            _ediFileProcessingService = ediFileProcessingService;
            _messagePublisher = messagePublisher;

        }
        
        public void Start()
        {
            Start(false);
        }

        public void Start(bool runOnce)
        {
            _ediDocsInWorker = new EdiDocsInWorker(_ediFileProcessingService);
            if (runOnce)
                _ediDocsInWorker.DoWork(null);
            else
            {
                _timer = new Timer(_ediDocsInWorker.DoWork, null, 0, 30000);  
            }
        }

        public void Stop()
        {
            _timer.Dispose();
        }
    }
}