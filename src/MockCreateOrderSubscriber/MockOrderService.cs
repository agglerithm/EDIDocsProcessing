using Castle.Core.Logging;
using EDIDocsProcessing.Core;
using EdiMessages;
using MassTransit;

namespace MockCreateOrderSubscriber
{
    class MockOrderService
    {        
        private readonly IServiceBus _bus;
        private UnsubscribeAction _unsubscribeAction;

        private readonly ILogger _logger = NullLogger.Instance;

        public MockOrderService(IServiceBus bus)
        {
            _bus = bus;
        }

        private ILogger Logger
        {
            get { return _logger; } 
        }

        public void Start()
        {
            //IServiceBus bus = ServiceLocator.Current.GetInstance<IServiceBus>();
            _unsubscribeAction = _bus.Subscribe<Subscriber<CreateOrderMessage>>(); 
            Logger.Info("Subscribing with CreateOrderSubscriber");
        }

        public void Stop()
        {
            _unsubscribeAction.Invoke();
            Logger.Info("Unsubscribing CreateOrderSubscriber");
        }
    }
}
