using AFPST.Common.Infrastructure;
using EDIDocsOut.config; 
using MassTransit;
using MassTransit.Transports;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using StructureMap;

namespace EDIDocsProcessing.MessagingTests
{
    [SetUpFixture]
    public class FixtureTestSetup
    {
        private const string MessageQueuePath = "msmq://localhost/mt_MessagingTests";
        private const string SubscriptionServiceUrl = "msmq://localhost/mt_subscriptions";
        [SetUp]
        public void BeforeTests()
        {
            ObjectFactory.Configure(x => x.For<IServiceBus>().Use(context =>
                                                                  ServiceBusFactory.New(sbc =>
                                                                  {
                                                                      sbc.ReceiveFrom(MessageQueuePath);
                                                                      sbc.UseMsmq();
                                                                      sbc.UseSubscriptionService(SubscriptionServiceUrl);
                                                                      sbc.SetConcurrentConsumerLimit(1);
                                                                      sbc.Subscribe(subs => subs.LoadFrom(ObjectFactory.Container));
                                                                  })));
        }
    }
}