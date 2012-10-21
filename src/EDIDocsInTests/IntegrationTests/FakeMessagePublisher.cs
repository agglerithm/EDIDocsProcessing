using MassTransit;
using MassTransitContrib;

namespace EDIDocsProcessing.Tests.IntegrationTests
{
    public class FakeMessagePublisher : IMessagePublisher
    {
        public void Publish<T>(T msg) where T : class
        {
             
        }

        public IServiceBus GetBus()
        {
            return new FakeServiceBus();
        }
    }
}