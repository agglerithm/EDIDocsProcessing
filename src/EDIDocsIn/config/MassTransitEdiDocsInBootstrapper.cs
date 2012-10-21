using MassTransit;
using MassTransit.EndpointConfigurators;
using MassTransit.Transports;
using MassTransitContrib;
using StructureMap;

namespace EDIDocsIn.config
{
    public static class MassTransitEdiDocsInBootstrapper
    {
        
                    private const string MessageQueuePath = "msmq://localhost/mt_edi_docs_in";
        private const string SubscriptionServiceUrl = "msmq://localhost/mt_subscriptions";

        public static void Execute()
        {

            MessagePublisher.InitializeServiceBus(MessageQueuePath, SubscriptionServiceUrl);

            ObjectFactory.Configure(x => x.For<IServiceBus>().Add(MessagePublisher.CurrentServiceBus));
        }
   
    }
}