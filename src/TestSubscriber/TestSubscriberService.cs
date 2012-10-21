using MassTransit;
using Microsoft.Practices.ServiceLocation;

namespace TestSubscriber
{
    public class TestSubscriberService
    {
        public void Start()
        {
            IServiceBus bus = ServiceLocator.Current.GetInstance<IServiceBus>();
            bus.Subscribe<Subscriber>();
        }

        public void Stop()
        {
        }
    }
}