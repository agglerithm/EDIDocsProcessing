using MassTransit;
using MassTransit.Transports;
using MassTransitContrib;
using StructureMap;  

namespace EDIDocsOut.config
{
    public static class MassTransitEdiDocsOutBootstrapper
    { 
        private const string MessageQueuePath = "msmq://localhost/mt_edi_docs_out";
        private const string SubscriptionServiceUrl = "msmq://localhost/mt_subscriptions";

        public static void Execute()
        {

            MessagePublisher.InitializeServiceBus(MessageQueuePath, SubscriptionServiceUrl);

            ObjectFactory.Configure(x => x.For<IServiceBus>().Add(MessagePublisher.CurrentServiceBus));
        }
    }
}