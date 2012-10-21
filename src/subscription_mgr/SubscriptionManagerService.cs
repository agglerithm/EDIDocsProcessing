using Castle.Core.Logging;
using MassTransit;

namespace subscription_mgr
{
    class SubscriptionManagerService
    {
        private readonly IServiceBus _bus;
        private UnsubscribeAction _unsubscribeAction;

        private readonly ILogger _logger = NullLogger.Instance;

        public SubscriptionManagerService(IServiceBus bus)
        {
            _bus = bus;
        }

        private ILogger Logger
        {
            get { return _logger; }
        }

        public void Start()
        {
 
        }

        public void Stop()
        {
 
        }
    }
}